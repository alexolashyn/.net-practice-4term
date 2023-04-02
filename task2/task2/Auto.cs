using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace task2
{
    enum Brand
    {
        toyota,
        bmw,
        mercedes,
        audi,
        mazda,
        honda
    }
    internal class Auto
    {
        private string id;
        private string brand;
        private string model;
        private string registration_number;
        private string bought_at;
        private string repaired_at;
        private string car_mileage;

        public string Id { get => id; set => id = Int32.TryParse(value, out int result) && result > 0 ? value: "error_flag"; }

        public string Brand { get => brand; set => brand = Enum.IsDefined(typeof(Brand), value) ? value : "error_flag"; }

        public string Model { get => model; set => model = Regex.IsMatch(value, @"^[a-z]+([0-9]+)?$") ? value : "error_flag"; }

        public string Registration_number { get => registration_number; set => registration_number = Regex.IsMatch(value, @"^[A-Z]{2}[0-9]{4}[A-Z]{2}$") ? value : "error_flag"; }

        public string Bought_at { get => bought_at; set => bought_at = DateTime.TryParseExact(
         value, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result) ? value: "error_flag"; }

        public string Repaired_at { get => repaired_at; set => repaired_at = DateTime.TryParseExact(
         value, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result) && String.Compare(value, bought_at)>0 ? value: "error_flag"; }

        public string Car_mileage { get => car_mileage; set => car_mileage = Int32.TryParse(value, out int result) && result > 0 ? value : "error_flag"; }
        
        public Auto(Auto other_auto)
        {
            var _properties = other_auto.GetType().GetProperties();
            string[] names = _properties.Select(p => p.Name).ToArray();
            List<string> values = new List<string>();
            foreach (var property in _properties)
            {
                values.Add((property.GetValue(other_auto)).ToString());
            }
            var properties = this.GetType().GetProperties();
            int i = 0;
            foreach (var property in properties)
            {
                property.SetValue(this, Convert.ChangeType(values[i++], property.PropertyType), null);
            }
        }

        public Auto(string[] fields_value)
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
                Console.WriteLine("Something went wrong!");
            }
        }
        public override string ToString()
        {
            var properties = this.GetType().GetProperties();
            string[] names = properties.Select(p => p.Name).ToArray();
            List<string> values = new List<string>();
            foreach (var property in properties)
            {
                values.Add((property.GetValue(this)).ToString());
            }
            string output = $"";
            foreach (string value in values){
                output += $"{value}, ";
            }
            return output;
        }
    }
}