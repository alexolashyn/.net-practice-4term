using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace proxy
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


        public string Id { get => id; set => id = Int32.TryParse(value, out int result) && result > 0 ? value : "-1"; }

        public string Titel { get => titel; set => titel = Regex.IsMatch(value, @"^[A-Z][a-z]+(\s[a-z]+)?(\s[0-9])?$") ? value : "-1"; }

        public string Country { get => country; set => country = Enum.IsDefined(typeof(Country), value) ? value : "-1"; }

        public string Start_date
        {
            get => start_date; set => start_date = DateTime.TryParseExact(
         value, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result) ? value : "-1";
        }

        public string End_date
        {
            get => end_date; set => end_date = DateTime.TryParseExact(
         value, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result) && String.Compare(value, start_date) > 0 ? value : "-1";
        }

        public string Duration { get => duration; set => duration = Double.TryParse(value, out double result) && result > 0.5 ? value : "-1"; }

        public Cinema(Cinema other_cinema)
        {
            id = other_cinema.id;
            titel = other_cinema.titel;
            country = other_cinema.country;
            start_date = other_cinema.start_date;
            end_date = other_cinema.end_date;
            duration = other_cinema.duration;
        }
        public Cinema()
        {

        }
        public override string ToString()
        {
            return $"{id}, {titel}, {country}, {start_date}, {end_date}, {duration}";
        }
    }
}

