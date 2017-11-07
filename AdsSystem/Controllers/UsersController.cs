using System;
using System.Collections.Generic;
using System.Linq;
using AdsSystem.Models;

namespace AdsSystem.Controllers
{
    public class UsersController : ControllerBase
    {
        public string Index()
        {
            using (var db = Db.Instance)
            {
                var vars = new Dictionary<string, object>()
                {
                    {"title", "Пользователи"},
                    {"list", db.Users.Cast<User>()}
                };    
                return View("Users/Index", vars);
            }
        }

        // @TODO: не получилось разобраться с рефлексией, поэтому такой костыль
        public string Edit() => Edit(null);

        public string Edit(string id = null)
        {
            Console.WriteLine(id);
            var user = new User();
            var vars = new Dictionary<string, object>();
            using (var db = Db.Instance) 
            {
                int uid;
                if (id != null && int.TryParse(id, out uid))
                    user = db.Users.Find(uid);
                
                if (Request.Method == "POST")
                {
                    var errors = new List<string>();
                    foreach (var field in new[] {"Email", "Name"})
                    {
                        if (Request.Form[field] == "")
                            errors.Add("Поле '" + User.Labels[field] + "' не заполнено");
                    }
                            
                    if (errors.Count > 0)
                        vars.Add("errors", errors);
                    else
                    {
                        user.Email = Request.Form["Email"][0];
                        user.Name = Request.Form["Name"][0];
                        if (Request.Form["Pass"][0] != "")
                            user.Pass = Request.Form["Pass"][0];

                        Console.WriteLine(user.Email + user.Name);
                        if (id == null)
                            db.Add(user);
                        else
                            db.Update(user);
                        db.SaveChanges();
                        return Redirect("/users");
                    }
                }
                vars.Add("info", user);
            }
            vars.Add("title", (id == null ? "Добавление" : "Изменение") + " пользователя");
            return View("Users/Edit", vars);
        }
        
        public string Delete(string id)
        {
            using (var db = Db.Instance) 
            {
                int uid;
                if (id != null && int.TryParse(id, out uid))
                {
                    var user = db.Users.Find(uid);
                    db.Remove(user);
                    db.SaveChanges();
                }
            }
            return Redirect("/users");
        }
    }
}