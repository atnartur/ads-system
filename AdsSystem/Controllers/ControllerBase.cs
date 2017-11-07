using System.Collections.Generic;
using AdsSystem.Libs;
using AdsSystem.Models;
using Microsoft.AspNetCore.Http;

namespace AdsSystem.Controllers
{
    public class ControllerBase
    {
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        protected User User = null;

        protected string Redirect(string url, int code = 302)
        {
            Response.StatusCode = code;
            Response.Headers["Location"] = url;
            return "";
        }
        
        public bool BeforeAction(string method, string controller, string action)
        {
            string idStr;
            string token;
            Request.Cookies.TryGetValue("uid", out idStr);
            Request.Cookies.TryGetValue("token", out token);

            int id;
            int.TryParse(idStr, out id);
            User = Auth.Check(id, token);
            
            if (User == null)
            {
                if (controller != "IndexController" || controller == "IndexController" && !action.Contains("Login"))
                {
                    Redirect("/login");
                    return false;
                }
            }
            return true;
        }

        
        public string View(string name)
        {
            return View(name, new Dictionary<string, object>());
        }
    
        public string View(string name, Dictionary<string, object> vars)
        {
            return new View(name, vars).ToString();
        
        }
    }
}