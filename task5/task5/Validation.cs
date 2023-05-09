using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task5
{
    internal class Validation
    {
        private bool is_valid;
        private Dictionary<string, string> errors;

        public bool Is_valid { get => is_valid; }
        public Dictionary<string, string> Errors { get => errors; }
        public Validation()
        {
            is_valid = true;
            errors = new Dictionary<string, string>();
        }
        public void validate(object element)
        {
            var properties = element.GetType().GetProperties();
            foreach (var property in properties)
            {
                var name = property.Name;
                var value = property.GetValue(element);
                if (value != null && value.ToString() != "-1")
                {
                    continue;
                }
                is_valid = false;
                errors.Add(name, $"Inappropriate value of {name}\'s field");
            }
        }
        public void show_errors()
        {
            foreach (KeyValuePair<string, string> pair in errors)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }

    }
}
