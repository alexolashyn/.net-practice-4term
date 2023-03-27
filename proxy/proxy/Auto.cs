using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace proxy
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
        private int id;
        private string brand;
        private string model;
        private string registration_number;
        private string bought_at;
        private string repaired_at;
        private int car_mileage;

        public int Id { get => id; set => id = value > 0 ? value : -1; }

        public string Brand { get => brand; set => brand = Enum.IsDefined(typeof(Brand), value) ? value : "-1"; }

        public string Model { get => model; set => model = Regex.IsMatch(value, @"^[a-z]+([0-9]+)?$") ? value : "-1"; }

        public string Registration_number { get => registration_number; set => registration_number = Regex.IsMatch(value, @"^[A-Z]{2}[0-9]{4}[A-Z]{2}$") ? value : "-1"; }

        public string Bought_at
        {
            get => bought_at;
            set
            {
                if (repaired_at!=null && String.Compare(value, repaired_at) > 0)
                {
                    value = "-1";
                }
                bought_at = DateTime.TryParseExact(value, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result) ? value : "-1";
            }
        }

        public string Repaired_at
        {
            get => repaired_at; set => repaired_at = DateTime.TryParseExact(
         value, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result) && String.Compare(value, bought_at) > 0 ? value : "-1";
        }

        public int Car_mileage { get => car_mileage; set => car_mileage = value > 0 ? value : -1; }

        public Auto(Auto other_auto)
        {
            id = other_auto.id;
            brand = other_auto.brand;
            model = other_auto.model;   
            registration_number = other_auto.registration_number;
            bought_at = other_auto.bought_at;
            repaired_at = other_auto.repaired_at;
            car_mileage = other_auto.car_mileage;
        }

        public Auto()
        {
            
        }
        public override string ToString()
        {
            return $"{id}, {brand}, {model}, {registration_number}, {bought_at}, {repaired_at}, {car_mileage}"; 
        }
    }
}
