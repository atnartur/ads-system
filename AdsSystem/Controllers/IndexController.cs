using System.Collections.Generic;
using System.Security.Authentication;
using AdsSystem.Libs;

namespace AdsSystem.Controllers
{
    public class IndexController : ControllerBase
    {   
        public string Index()
        {
            return Redirect("/users");
        }

        private Dictionary<string, object> _loginViewParams = new Dictionary<string, object>()
        {
            {"layout", "empty"},
            {"body_classes", "hold-transition login-page"}
        }; 
        
        public string Login()
        {
            if (User != null)
                return Redirect("/");
            return new View("Login", _loginViewParams).ToString();
        }

        public string LoginHandler()
        {
            if (User != null)
                return Redirect("/");
            try
            {
                var user = Auth.Login(Request.Form["email"], Request.Form["pass"]);
                Auth.Remember(Response, user);
                Response.Redirect("/");
            }
            catch (AuthenticationException)
            {
                _loginViewParams["error"] = "Неправильный логин или пароль";                
            }
            return new View("Login", _loginViewParams).ToString();
        }
    }
}