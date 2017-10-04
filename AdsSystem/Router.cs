using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace AdsSystem
{
    public class Router
    {
        private static Dictionary<string, string> Routes = new Dictionary<string, string>()
        {
            {@"GET ^\/$", "IndexController.Index"}
        };

        private static void _res(HttpListenerResponse res, string body)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            res.OutputStream.Write(buffer, 0, buffer.Length);
            res.Close();
        }
        
        public static void Dispatch(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                var url = request.Url.AbsolutePath;
                Console.WriteLine(request.Url.AbsolutePath);
                string selectedAction = null;

                foreach (var route in Routes)
                {
                    var key = route.Key.Split(' ');
                    var httpMethod = key[0];

                    if (request.HttpMethod != httpMethod)
                        continue;

                    var r = Regex.Matches(url, key[1]);
                    var check = r.Count > 0;
                    Console.WriteLine(check + " " + url);
                    if (check)
                    {
                        selectedAction = route.Value;
                        break;
                    }
                }

                Console.WriteLine(request.HttpMethod + " " + url + " " + (selectedAction != null ? "ok" : "error") +
                                  " " + DateTime.Now + ":" + DateTime.Now.Millisecond);

                if (selectedAction == null)
                    selectedAction = "ErrorController.E404";
                
                var action = selectedAction.Split('.');
                var baseNamespace = typeof(Router).Namespace.Split('.')[0];
                var cls = Type.GetType(baseNamespace + ".Controllers." + action[0]);

                if (cls == null)
                {
                    response.StatusCode = 500;
                    _res(response, "cannot find action class");
                    return;
                }

                var controller = Activator.CreateInstance(cls);

                var reqProp = cls.GetProperty("Request");
                var resProp = cls.GetProperty("Response");

                if (reqProp == null || resProp == null)
                {
                    response.StatusCode = 500;
                    _res(response, "controller is not contains Request and Response attributes");
                    return;
                }

                reqProp.SetValue(controller, request);
                resProp.SetValue(controller, response);

                var method = cls.GetMethod(action[1]);
                string res = (string) method.Invoke(controller, new object[] { });

                response = (HttpListenerResponse) resProp.GetValue(controller);
                _res(response, res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                response.StatusCode = 500;
                _res(response, "Exception: " + e.Message);
            }
        } 
    }
}