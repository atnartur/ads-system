using System;
using System.Collections.Generic;

namespace AdsSystem.Controllers
{
    public class IndexController : ControllerBase
    {   
        public string Index()
        {
            Response.StatusCode = 302;
            Response.Headers["Location"] = "/login";
            return "";//new View("Index").ToString();
        }

        public string Login()
        {
            return new View("Login", new Dictionary<string, string>()
            {
                {"layout", "empty"},
                {"body_classes", "hold-transition login-page"}
            }).ToString();
        }

        public string LoginHandler()
        {
            Console.WriteLine(Request.Form["login"] + " " + Request.Form["pass"]);
            return "";
        }
    }
}