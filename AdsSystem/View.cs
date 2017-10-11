using System;
using System.Collections.Generic;
using System.IO;
using HandlebarsDotNet;

namespace AdsSystem
{
    public class View
    {
        private string _name;
        private string _content;
        private Func<object, string> _template;
        private Dictionary<string, string> _vars = new Dictionary<string, string>();

        public View(string name)
        {
            _name = name;
            _read();
        }
        
        public View(string name, Dictionary<string, string> vars)
        {
            _name = name;
            _vars = vars;
            _read();
        }

        private void _read()
        {
            try
            {
                var fileContent = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Views", _name + ".hdbs"));
                _template = Handlebars.Compile(fileContent);
            }
            catch (Exception e)
            {
                _template = a => e.Message;
            }
        }

        public override string ToString()
        {
//            var content = _content;
//            foreach (var keyValuePair in _vars) 
//                content = content.Replace("{" + keyValuePair.Key + "}", keyValuePair.Value);
            return _template(_vars);
        }
    }
}