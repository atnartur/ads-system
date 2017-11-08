using System;
using System.Collections.Generic;
using System.IO;
using HandlebarsDotNet;

namespace AdsSystem
{
    public class View
    {
        private string _name;
        private Dictionary<string, object> _vars = new Dictionary<string, object>()
        {
            {"body_classes", "hold-transition skin-blue sidebar-mini"}
        };

        public View(string name)
        {
            _name = name;
        }
        
        public View(string name, Dictionary<string, object> vars)
        {
            _name = name;
            foreach (var keyValuePair in vars)
                _vars[keyValuePair.Key] = keyValuePair.Value;
        }

        private string _open(string name) => 
            File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Views", name + ".hbs"));

        public override string ToString()
        {
            var wrap = Handlebars.Compile(_open("wrap"));
            var layout = Handlebars.Compile(_open("layouts/" + _vars.GetValueOrDefault("layout", "main")));
            var template = Handlebars.Compile(_open(_name));
            
            _vars["content"] = template(_vars);
            _vars["content"] = layout(_vars);
            return wrap(_vars);
        }
    }
}