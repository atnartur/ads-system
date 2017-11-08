using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdsSystem.Models;
using Microsoft.AspNetCore.Http;

namespace AdsSystem.Controllers
{
    public abstract class CrudController<T> : ControllerBase where T : class, IModel, new()
    {
        protected abstract string Title { get; }
        protected abstract string ViewBase { get; }
        protected abstract Func<Db, DbSet<T>> DbSet { get; }
        protected abstract string[] RequiredFields { get; } 

        protected abstract void Save(T model, HttpRequest request);

        protected static RouterDictionary GetRoutes(string viewBase)
        {
            var url = viewBase.ToLower();
            return new RouterDictionary()
            {
                {@"GET ^\/" + url + "$", viewBase + "Controller.Index"},
                {@"GET ^\/" + url + "/edit$", viewBase + "Controller.Edit"},
                {@"POST ^\/" + url + "/edit$", viewBase + "Controller.Edit"},
                {@"GET ^\/" + url + "/edit/([0-9]+)$", viewBase + "Controller.Edit"},
                {@"POST ^\/" + url + "/edit/([0-9]+)$", viewBase + "Controller.Edit"},
                {@"GET ^\/" + url + "/delete/([0-9]+)$", viewBase + "Controller.Delete"},
            };
        }
        
        public string Index()
        {
            using (var db = Db.Instance)
            {
                var vars = new Dictionary<string, object>()
                {
                    {"title", Title},
                    {"list", DbSet(db).Cast<T>()}
                };    
                return View(ViewBase + "/Index", vars);
            }
        }

        public string Edit(string id = null)
        {
            Console.WriteLine(id);
            T item = new T();
            var vars = new Dictionary<string, object>();
            using (var db = Db.Instance) 
            {
                int uid;
                if (id != null && int.TryParse(id, out uid))
                    item = DbSet(db).Find(uid);
                
                if (Request.Method == "POST")
                {
                    var errors = new List<string>();
                    foreach (var field in RequiredFields)
                    {
                        if (Request.Form[field] == "")
                            errors.Add("Поле '" + item.Labels[field] + "' не заполнено");
                    }
                            
                    if (errors.Count > 0)
                        vars.Add("errors", errors);
                    else
                    {
                        Save(item, Request);
                        
                        if (id == null)
                            db.Add(item);
                        else
                            db.Update(item);
                        db.SaveChanges();
                        return Redirect("/" + ViewBase.ToLower());
                    }
                }
                vars.Add("info", item);
            }
            vars.Add("title", Title +  " - " + (id == null ? "Добавление" : "Изменение"));
            return View(ViewBase + "/Edit", vars);
        }
        
        public string Delete(string id)
        {
            using (var db = Db.Instance) 
            {
                int uid;
                if (id != null && int.TryParse(id, out uid))
                {
                    var user = DbSet(db).Find(uid);
                    db.Remove(user);
                    db.SaveChanges();
                }
            }
            return Redirect("/" + ViewBase.ToLower());
        }
    }
}