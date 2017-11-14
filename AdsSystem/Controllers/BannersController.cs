using System;
using AdsSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdsSystem.Controllers
{
//    public class BannersController : CrudController<Banner>
//    {
//        protected override string Title => "Баннеры";
//        protected override string ViewBase => "Banners";
//        protected override Func<Db, DbSet<Banner>> DbSet => db => db.Banners;
//        protected override string[] RequiredFields => new[] {"Name", "Width", "Height", "Type", "Author"};
//        public static RouterDictionary GetRoutes() => GetRoutes("Banners");
//        protected override void Save(Banner model, HttpRequest request)
//        {
//            model.Name = Request.Form["Name"][0];
//            model.Width = int.Parse(Request.Form["Width"][0]);
//            model.Height = int.Parse(Request.Form["Height"][0]);
//            
//            Banner.BannerType type;
//            Enum.TryParse(Request.Form["Type"][0], out type);
//            model.Type = type;
//            
//            using (var db = Db.Instance)
//            {
//                model.Author = db.Users.Find(Request.Form["Name"][0]);
//            }
//        }
//    }
}