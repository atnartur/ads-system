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

        private void Helpers()
        {
            Handlebars.RegisterHelper("ifCond", (writer, context, args) =>
            {
                if (args.Length != 5)
                {
                    writer.Write("ifCond:Wrong number of arguments");
                    return;
                }

                if (args[0] == null || args[0].GetType().Name == "UndefinedBindingResult")
                {
                    writer.Write("ifCond:args[0] undefined");
                    return;
                }
                if (args[1] == null || args[1].GetType().Name == "UndefinedBindingResult")
                {
                    writer.Write("ifCond:args[1] undefined");
                    return;
                }
                if (args[2] == null || args[2].GetType().Name == "UndefinedBindingResult")
                {
                    writer.Write("ifCond:args[2] undefined");
                    return;
                }

                if (args[0].GetType().Name == "String")
                {
                    string val1 = args[0].ToString();
                    string val2 = args[2].ToString();

                    switch (args[1].ToString())
                    {
                        case ">":
                            writer.Write(val1.Length > val2.Length ? args[3] : args[4]);
                            break;
                        case "=":
                        case "==":
                            writer.Write(val1 == val2 ? args[3] : args[4]);
                            break;
                        case "<":
                            writer.Write(val1.Length < val2.Length ? args[3] : args[4]);
                            break;
                        case "!=":
                        case "<>":
                            writer.Write(val1 != val2 ? args[3] : args[4]);
                            break;
                    }
                }
                else
                {
                    float val1 = float.Parse(args[0].ToString());
                    float val2 = float.Parse(args[2].ToString());

                    switch (args[1].ToString())
                    {
                        case ">":
                            writer.Write(val1 > val2 ? args[3] : args[4]);
                            break;
                        case "=":
                        case "==":
                            writer.Write(val1 == val2 ? args[3] : args[4]);
                            break;
                        case "<":
                            writer.Write(val1 < val2 ? args[3] : args[4]);
                            break;
                        case "<=":
                            writer.Write(val1 <= val2 ? args[3] : args[4]);
                            break;
                        case ">=":
                            writer.Write(val1 >= val2 ? args[3] : args[4]);
                            break;
                        case "!=":
                        case "<>":
                            writer.Write(val1 != val2 ? args[3] : args[4]);
                            break;
                    }
                }
            });
        }
        
        public override string ToString()
        {
            Helpers();
            var wrap = Handlebars.Compile(_open("wrap"));
            var layout = Handlebars.Compile(_open("layouts/" + _vars.GetValueOrDefault("layout", "main")));
            var template = Handlebars.Compile(_open(_name));
            
            _vars["content"] = template(_vars);
            _vars["content"] = layout(_vars);
            return wrap(_vars);
        }
    }
}