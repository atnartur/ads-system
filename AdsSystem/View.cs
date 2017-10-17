using System;
using System.Collections.Generic;
using System.IO;
using HandlebarsDotNet;

namespace AdsSystem
{
    public class View
    {
        private string _name;
        private Dictionary<string, string> _vars = new Dictionary<string, string>();

        public View(string name)
        {
            _name = name;
        }
        
        public View(string name, Dictionary<string, string> vars)
        {
            _name = name;
            _vars = vars;
        }

        private string _open(string name) => 
            File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Views", name + ".hbs"));

        public override string ToString()
        {
            _vars["content"] = Handlebars.Compile(_open(_name))(_vars);
            return Handlebars.Compile(_open("layout"))(_vars);
        }
    }
}