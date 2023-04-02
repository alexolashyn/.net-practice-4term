using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace task2
{
    using System.ComponentModel.DataAnnotations;
   
    public class Validation
    {
        private bool is_valid;
        private Dictionary <string, string> errors;

        public bool Is_valid { get => is_valid; }
        public Dictionary <string, string> Errors { get => errors; }
        public Validation()
        {
            is_valid = true;
            errors = new Dictionary<string, string>();
        }
        public void validate(object auto)
        {

            var type = auto.GetType();
            var properties = type.GetProperties();
            foreach(var property in properties)
            {
                var value = property.GetValue(auto);
                var name = property.Name;
                if (name == "Repaired_at")
                {
                    property.SetValue(auto, value);
                    value = property.GetValue(auto);
                }
                if (value == "error_flag")
                {
                    is_valid = false;
                    errors.Add(name, $"Inappropriate value of {name}\'s field");
                }
            }
        }
        public void show_errors() {
            foreach (KeyValuePair<string, string> pair in errors)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }
        
    }
}
