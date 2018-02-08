using System;
using System.Collections.Generic;
using System.Linq;
using AdsSystem.Libs;
using Microsoft.EntityFrameworkCore;

namespace AdsSystem.Controllers
{
    public class ApiController : ControllerBase
    {
        public string Get(string zoneId)
        {
            int id;
            int.TryParse(zoneId, out id);
            Response.StatusCode = 400;

            using (var db = Db.Instance)
            {
                var where = db.BannersZones.Where(x => x.ZoneId == id);
                var count = where.Count();

                if (count == 0)
                {
                    Response.StatusCode = 404;
                    return "";
                }

                var res = where.Skip(new Random().Next(count)).First();

                if (res == null)
                    return "";

                var banner = db.Banners.Find(res.BannerId);
                var zone = db.Zones.Find(res.ZoneId);

                var view = new Models.View();

                db.Entry(banner).State = EntityState.Unchanged;
                db.Entry(zone).State = EntityState.Unchanged;

                view.BannerId = banner.Id;
                view.Zone = zone;
                view.UserAgent = Request.Headers["User-Agent"];

                db.Add(view);
                db.SaveChanges();

                var linkBase = "http://" + Request.Host;
                var clickLink = linkBase + "/api/click/" + view.Id;

                Response.StatusCode = 200;
                return View("ApiReturn", new Dictionary<string, object>
                {
                    {"noStatic", true},
                    {"layout", "empty"},
                    {"Height", zone.Height},
                    {"Width", zone.Width},
                    {"ClickLink", clickLink},
                    {"Type", banner.Type},
                    {"Html", banner.Html},
                    {"ImageLink", banner.ImageFormat != null ? (linkBase + "/" + FileStorage.GetLink(banner, banner.ImageFormat)) : ""}
                });
            }
        }

        public string Click(string viewId)
        {
            int id;
            int.TryParse(viewId, out id);
            using (var db = Db.Instance)
            {
                var view = db.Views
                    .Where(x => x.Id == id)
                    //.Include(x => x.Banner)
                    .FirstOrDefault();
                view.IsClicked = true;
                db.Attach(view);
                db.SaveChanges();

                Response.Redirect(db.Banners.FirstOrDefault(x => x.Id == view.BannerId).Link);
                return "";
            }
        }
    }
}