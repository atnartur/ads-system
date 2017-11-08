using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using HeyRed.Mime; 

namespace AdsSystem
{
    public class Router
    {
        private static Dictionary<string, string> Routes = new Dictionary<string, string>()
        {
            {@"GET ^\/$", "IndexController.Index"},
            {@"GET ^\/login$", "IndexController.Login"},
            {@"POST ^\/login$", "IndexController.LoginHandler"},
            {@"GET ^\/users$", "UsersController.Index"},
            {@"GET ^\/users/edit$", "UsersController.Edit"},
            {@"POST ^\/users/edit$", "UsersController.Edit"},
            {@"GET ^\/users/edit/([0-9]+)$", "UsersController.Edit"},
            {@"POST ^\/users/edit/([0-9]+)$", "UsersController.Edit"},
            {@"GET ^\/users/delete/([0-9]+)$", "UsersController.Delete"},
        };

        private static void _res(HttpResponse res, string body)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            res.Body.Write(buffer, 0, buffer.Length);
            res.Body.Close();
        }

        private static void _invokeAction(string selectedAction, HttpRequest request, HttpResponse response, object[] parameters = null)
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
            
            bool isNeedInvokeAction = true;
            string res = "";
            
            var beforeActionMethod = cls.GetMethod("BeforeAction");
            if (beforeActionMethod != null)
                isNeedInvokeAction = (bool) beforeActionMethod.Invoke(controller, new [] { request.Method, action[0], action[1] });
            
            if (isNeedInvokeAction)
            {
                parameters = parameters ?? new object[] { };
                var paramsCheck = parameters != null && parameters.Length > 0;
                var a = Enumerable.Repeat(typeof(object), parameters.Length).ToArray<object>();
                var b = controller.GetType().GetMethod(action[1]).GetParameters().Select(x => Type.Missing).ToArray();
                
//                var method = cls.GetMethod(action[1], paramsCheck ? a : b); 
//                var method = cls.GetMethod(action[1], Enumerable.Repeat(Type.Missing, parameters.Length).ToArray());
                var method = cls.GetTypeInfo().GetDeclaredMethod(action[1]);
                
//                cls.InvokeMember(action[1], BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, )
                res = (string) method.Invoke(controller, parameters);
//                foreach (var parameter in parameters)
//                {
//                    Console.WriteLine(parameter);
//                }
//                if (paramsCheck)
//                {
                //paramsCheck ? Enumerable.Repeat(typeof(string), parameters.Length).ToArray() : new Type[] {});
//                }
//                else
//                {
//                    var method = cls.GetMethod(action[1]); 
                //paramsCheck ? Enumerable.Repeat(typeof(string), parameters.Length).ToArray() : new Type[] {});
//                    res = (string) method.Invoke(controller, new object[]{});
//                }
//                Console.WriteLine(parameters.Length);
//                Console.WriteLine(paramsCheck);
                
//                Console.WriteLine(method);
                
//                res = (string) method.Invoke(controller, paramsCheck ? parameters : new object[]{});
            }
            response = (HttpResponse) resProp.GetValue(controller);

            _res(response, res);
        }
        
        public static void Dispatch(HttpRequest request, HttpResponse response)
        {
            try
            {
                var url = request.Path.Value;
                string selectedAction = null;
                string[] parameters = null;

                foreach (var route in Routes)
                {
                    var key = route.Key.Split(' ');
                    var httpMethod = key[0];

                    if (request.Method != httpMethod)
                        continue;

                    var r = Regex.Matches(url, key[1]);
                    var check = r.Count > 0;
                    Console.WriteLine(check + " " + url);
                    if (check)
                    {
                        selectedAction = route.Value;
                        parameters = r.SelectMany(x => x.Groups).Skip(1).Select(x => x.Value).ToArray();
                        break;
                    }
                }
                
                if (selectedAction != null)
                    // ReSharper disable once CoVariantArrayConversion
                    _invokeAction(selectedAction, request, response, parameters);
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
                                response.Body.Write(buffer, 0, buffer.Length);
                            }
                        }
                        response.Body.Close();
                    }
                }
                Console.WriteLine(request.Method + " " + url + " " + (selectedAction != null ? "ok" : "error") +
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