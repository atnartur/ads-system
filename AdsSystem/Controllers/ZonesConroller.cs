using System;
using AdsSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdsSystem.Controllers
{
    public class ZonesController : CrudController<Zone>
    {
        protected override string Title => "Зоны";
        protected override string ViewBase => "Zones";
        protected override Func<Db, DbSet<Zone>> DbSet => db => db.Zones;
        protected override string[] RequiredFields => new[] {"Name", "Width", "Height"};
        protected override void Save(Zone model, Db db, HttpRequest request)
        {
            model.Name = request.Form["Name"][0];
            model.Width = int.Parse(request.Form["Width"][0]);
            model.Height = int.Parse(request.Form["Height"][0]);
        }
        
        public static RouterDictionary GetRoutes() => GetRoutes("Zones");
    }
}