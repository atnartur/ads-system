using System;
using AdsSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdsSystem.Controllers
{
    public class ZonesController : CrudController<Zone>
    {
        protected override string Title => "Зоны";

        protected override Func<Db, DbSet<Zone>> DbSet => db => db.Zones;
        protected override string[] RequiredFields { get; }
        protected override void Save(Zone model, HttpRequest request)
        {
            model.Name = request.Form["Name"][0];
            model.Width = int.Parse(request.Form["Width"][0]);
            model.Height = int.Parse(request.Form["Heignt"][0]);
        }
    }
}