using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using HeyRed.Mime; 

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

        private static void _invokeAction(string selectedAction, HttpListenerRequest request,
            HttpListenerResponse response)
        {
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
        
        public static void Dispatch(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                var regex = new Regex(@"(^[\w]+:[0-9]+)");
                var url = regex.Replace(request.Url.AbsoluteUri, "");
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
                
                if (selectedAction != null)
                    _invokeAction(selectedAction, request, response);
                else
                {
                    var urlArr = url.Substring(1).Split('?');
                    var staticFilePath = Path.Combine(Environment.CurrentDirectory, "public", urlArr[0]);
                    
                    if (!File.Exists(staticFilePath))
                        _invokeAction("ErrorController.E404", request, response);
                    else
                    {
                        selectedAction = "static";
                        
                        string mime = MimeTypesMap.GetMimeType(Path.GetFileName(url));
                        response.Headers["Content-Type"] = mime;
                        
                        using (var sr = new StreamReader(staticFilePath))
                        {
                            while (!sr.EndOfStream)
                            {
                                byte[] buffer = Encoding.UTF8.GetBytes(sr.ReadLine() + "\n");
                                response.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        response.Close();
                    }
                }
                Console.WriteLine(request.HttpMethod + " " + url + " " + (selectedAction != null ? "ok" : "error") +
                                  " " + DateTime.Now + ":" + DateTime.Now.Millisecond);
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