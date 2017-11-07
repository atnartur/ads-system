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
        
        public string Edit()
        {
            return View("Users/Edit");
        }
        
        public string Delete()
        {
            return "";
        }
    }
}