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
        protected Dictionary<string, object> Vars = new Dictionary<string, object>();

        protected abstract void Save(T model, Db db, HttpRequest request);

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

        public CrudController()
        {
            Vars["title"] = Title;
        }
        
        public virtual string Index()
        {
            using (var db = Db.Instance)
            {
                Vars.Add("list", DbSet(db).Cast<T>());
                return View(ViewBase + "/Index", Vars);
            }
        }

        protected virtual void PreEditHook(T model, ref Dictionary<string, object> vars) {}
        protected virtual void AfterSaveHook(T model, Db db, HttpRequest request) {}

        public string Edit(string id = null)
        {
            Console.WriteLine(id);
            T item = new T();
            
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
                        Vars.Add("errors", errors);
                    else
                    {
                        Save(item, db, Request);
                        
                        if (id == null)
                            db.Add(item);
                        else
                            db.Update(item);
                        db.SaveChanges();
                        
                        AfterSaveHook(item, db, Request);
                        
                        return Redirect("/" + ViewBase.ToLower());
                    }
                }
                PreEditHook(item, ref Vars);
                Vars.Add("info", item);
            }
            Vars["title"] = Title +  " - " + (id == null ? "Добавление" : "Изменение");
            return View(ViewBase + "/Edit", Vars);
        }
        
        public string Delete(string id)
        {
            using (var db = Db.Instance) 
            {
                int uid;
                if (id != null && int.TryParse(id, out uid))
                {
                    var item = DbSet(db).Find(uid);
                    db.Remove(item);
                    db.SaveChanges();
                }
            }
            return Redirect("/" + ViewBase.ToLower());
        }
    }
}