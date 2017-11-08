using System;
using AdsSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdsSystem.Controllers
{
    public class UsersController : CrudController<User>
    {
        protected override string Title => "Пользователи";
        protected override string ViewBase => "Users";
        protected override Func<Db, DbSet<User>> DbSet => db => db.Users;
        protected override string[] RequiredFields => new[] {"Email", "Name"}; 
        protected override void Save(User model, HttpRequest request)
        {
            model.Email = Request.Form["Email"][0];
            model.Name = Request.Form["Name"][0];
            if (Request.Form["Pass"][0] != "")
                model.Pass = Request.Form["Pass"][0];
        }

        public static RouterDictionary GetRoutes() => GetRoutes("Users");
    }
}