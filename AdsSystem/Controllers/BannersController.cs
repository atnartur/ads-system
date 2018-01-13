using System;
using System.Collections.Generic;
using System.Linq;
using AdsSystem.Libs;
using AdsSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AdsSystem.Controllers
{
    public class BannersController : CrudController<Banner>
    {
        protected override string Title => "Баннеры";
        protected override string ViewBase => "Banners";
        protected override Func<Db, DbSet<Banner>> DbSet => db => db.Banners;
        protected override string[] RequiredFields => new[] { "Name", "Width", "Height", "Type", "Author" };
        public static RouterDictionary GetRoutes() => GetRoutes("Banners");

        public override string Index()
        {
            using (var db = Db.Instance)
            {
                if (Request.Headers["X-Requested-With"].Count > 0 &&
                    Request.Headers["X-Requested-With"][0] == "XMLHttpRequest")
                {
                    Response.Headers["Content-type"] = "application/json; charset=utf-8";
                    var query = DbSet(db).Cast<Banner>();

                    if (Request.Query["zoneId"].Count > 0)
                    {
                        try
                        {
                            var zoneId = int.Parse(Request.Query["ZoneId"][0]);
                            query = db.BannersZones.Where(x => x.ZoneId == zoneId).Select(x => x.Banner);
                        }
                        catch (FormatException) { }
                    }

                    if (User.Role == UserRole.Advertiser)
                        query = query.Where(x => x.Advertiser == User);

                    if (Request.Query["search"].Count > 0)
                    {
                        var search = Request.Query["search"][0];
                        query = query.Where(x => x.Name.ToLower().Contains(search.ToLower()));
                    }

                    var res = query.Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.ClicksCount,
                        x.ViewsCount,
                        AdvertiserName = x.Advertiser.Name,
                        Zones = x.BannersZones.Select(y => new { Id = y.ZoneId, y.Zone.Name }).ToList()
                    });
                    return JsonConvert.SerializeObject(res);
                }
                else
                    Vars.Add("Zones", db.Zones.ToArray());

                return View(ViewBase + "/Index", Vars);
            }
        }

        protected override void Save(Banner model, Db db, HttpRequest request)
        {
            model.Name = Request.Form["Name"][0];
            model.Link = Request.Form["Link"][0];
            model.Priority = int.Parse(Request.Form["Priority"][0]);
            model.Html = Request.Form["Html"][0];
            model.MaxImpressions = int.Parse(Request.Form["MaxImpressions"][0]);

            if (Request.Form["Advertiser"].Count > 0 && Request.Form["Advertiser"][0] != "")
                model.Advertiser = db.Users.Find(int.Parse(Request.Form["Advertiser"][0]));
            if (Request.Form["StartTime"].Count > 0 && Request.Form["StartTime"][0] != "")
                model.StartTime = DateTime.Parse(Request.Form["StartTime"][0]);
            if (Request.Form["EndTime"].Count > 0 && Request.Form["EndTime"][0] != "")
                model.EndTime = DateTime.Parse(Request.Form["EndTime"][0]);

            if (Request.Form["IsActive"].Count > 0)
                model.IsActive = Request.Form["IsActive"][0] == "on";
            if (Request.Form["IsArchived"].Count > 0)
                model.IsArchived = Request.Form["IsArchived"][0] == "on";

            db.Entry(User).State = EntityState.Unchanged;
            model.Author = User;

            Banner.BannerType type;
            Enum.TryParse(Request.Form["Type"][0], out type);
            model.Type = type;
        }

        protected override void AfterSaveHook(Banner model, Db db, HttpRequest request)
        {
            if (request.Form.Files.Count > 0 && request.Form.Files["Image"] != null)
                model.ImageFormat = FileStorage.PutFile(request.Form.Files["Image"], model);

            var havingZones = db.BannersZones.Where(x => x.BannerId == model.Id).Select(x => x.ZoneId).ToList();

            var zonesFromUser = request.Form
                .Where(x => x.Key.StartsWith("zones"))
                .Select(x => int.Parse(x.Key.Replace("zones", "").TrimStart('[').TrimEnd(']')))
                .ToList();

            // те зоны, которые пользователь вычеркнул - удаляем
            foreach (var id in havingZones.Where(x => !zonesFromUser.Contains(x)))
                db.Remove(db.BannersZones.Find(model.Id, id));

            db.SaveChanges();

            // те зоны, которые пользователь добавил - добавляем
            foreach (var id in zonesFromUser.Where(x => !havingZones.Contains(x)))
                db.Add(new BannersZones { BannerId = model.Id, ZoneId = id });

            db.SaveChanges();
        }

        protected override void PreEditHook(Banner model, ref Dictionary<string, object> vars)
        {
            Vars.Add("types", Enum.GetNames(typeof(Banner.BannerType)));
            if (model.ImageFormat != null)
                vars["ImageLink"] = "/" + FileStorage.GetLink(model, model.ImageFormat);

            using (var db = Db.Instance)
            {
                var havingZones = new List<int>();

                var advertiserId = 0;
                
                if (model != null && model.Id != null && model.Id != 0)
                {
                    havingZones = db.BannersZones.Where(x => x.BannerId == model.Id).Select(x => x.ZoneId).ToList();

                    var conn = db.Database.GetDbConnection();
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT AdvertiserId FROM Banners WHERE Id = " + model.Id;
                    advertiserId = (int) cmd.ExecuteScalar();
                    Vars.Add("Advertizer", advertiserId);
                    conn.Close();
                }

                Vars.Add("zones", db.Zones.Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Width,
                    x.Height,
                    IsEnabled = havingZones.Contains(x.Id)
                }).ToList());

                Vars.Add("advertizers", db.Users.Where(x => x.Role == UserRole.Advertiser).Select(x => new
                {
                    x.Id,
                    x.Name,
                    Selected = advertiserId == x.Id
                }).ToList());
            }
        }
    }
}