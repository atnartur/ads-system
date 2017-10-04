using System;
using System.Collections.Generic;
using System.IO;

namespace AdsSystem
{
    public class View
    {
        private string _name;
        private string _content;
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
                _content = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "../../", "Views", _name + ".html"));
            }
            catch (Exception e)
            {
                _content = e.Message;
            }
        }

        public override string ToString()
        {
            var content = _content;
            foreach (var keyValuePair in _vars) 
                content = content.Replace("{" + keyValuePair.Key + "}", keyValuePair.Value);
            return content;
        }
    }
}