using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace task3
{
    enum Country
    {
        Ukraine,
        USA,
        Italy,
        France,
        German,
        India
    }
    internal class Cinema
    {
        private string id;
        private string titel;
        private string country;
        private string start_date;
        private string end_date;
        private string duration;


        public string Id { get => id; set => id = Int32.TryParse(value, out int result) && result > 0 ? value : "error_flag"; }

        public string Titel { get => titel; set => titel = Regex.IsMatch(value, @"^[A-Z][a-z]+(\s[a-z]+)?(\s[0-9])?$") ? value : "error_flag"; }

        public string Country { get => country; set => country = Enum.IsDefined(typeof(Country), value) ? value : "error_flag"; }

        public string Start_date
        {
            get => start_date; set => start_date = DateTime.TryParseExact(
         value, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result) ? value : "error_flag";
        }

        public string End_date
        {
            get => end_date; set => end_date = DateTime.TryParseExact(
         value, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result) && String.Compare(value, start_date) > 0 ? value : "error_flag";
        }

        public string Duration { get => duration; set => duration = Double.TryParse(value, out double result) && result > 0.5 ? value : "error_flag"; }

        public Cinema(Cinema other_cinema)
        {
            var _properties = other_cinema.GetType().GetProperties();
            List<string> values = new List<string>();
            foreach (var property in _properties)
            {
                values.Add((property.GetValue(other_cinema)).ToString());
            }
            var properties = this.GetType().GetProperties();
            int i = 0;
            foreach (var property in properties)
            {
                property.SetValue(this, Convert.ChangeType(values[i++], property.PropertyType), null);
            }
        }

        public Cinema(string[] fields_value)
        {
            var properties = this.GetType().GetProperties();
            if (fields_value.Length == properties.Length)
            {
                int i = 0;
                foreach (var property in properties)
                {
                    property.SetValue(this, Convert.ChangeType(fields_value[i++], property.PropertyType), null);
                }
            }
            else
            {
                foreach (var property in properties)
                {
                    property.SetValue(this, Convert.ChangeType("error_flag", property.PropertyType), null);
                }
                Console.WriteLine($"Inappropriate amount of values! Given: {fields_value.Length}, Expected: {properties.Length}");
            }
        }
        public override string ToString()
        {
            var properties = this.GetType().GetProperties();
            List<string> values = new List<string>();
            foreach (var property in properties)
            {
                values.Add((property.GetValue(this)).ToString());
            }
            string output = $"";
            foreach (string value in values)
            {
                output += $"{value}, ";
            }
            return output;
        }
    }
}
