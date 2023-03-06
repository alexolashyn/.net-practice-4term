using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace task2
{
    internal class Collection
    {
        private List<Auto> autos;
        public List<Auto> Autos { get => autos; }

        public Collection()
        {
            autos = new List<Auto>();
        }
        public Collection(List<Auto> auto_collection)
        {
            this.autos = auto_collection;
        }
        public bool Push(Auto auto)
        {
            Validation validator = new Validation();
            validator.validate(auto);
            if (validator.Is_valid)
            {
                autos.Add(auto);
                return true;
            }
            validator.show_errors();
            return false;
            
        }
        public void AddInto()
        {
            string[] temp = this.Get_input("Enter new auto: ").Split(" ");
            Auto new_auto = new Auto(temp);
            this.Push(new_auto);
        }
        public void Pop(Auto auto = null)
        {
            if (auto == null)
            {
                Console.Write("Enter id of the element, which you\'d like to remove: ");
                var selected_id = Console.ReadLine();
                for (int i = 0; i < autos.Count(); i++)
                {
                    if (autos[i].Id == selected_id)
                    {
                        autos.Remove(autos[i]);
                    }
                }
            }
            else
            {
                autos.Remove(auto);
            }
            
        }
        public void Show()
        {
            if (autos.Any())
            {
                for (int i = 0; i < autos.Count(); i++)
                {
                    Console.WriteLine($"---------------------------------\n{autos[i]}");
                }
            }
            else
            {
                Console.WriteLine("Your collection is empty!");
            }
        }
        
        public string Get_input(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            return input;
        }
        public void Input()
        {
            string filename = this.Get_input("Enter filename: ");
            string pattern = @"C:\4term\task2\task2\";
            while (!File.Exists(pattern + filename)) {
                filename = this.Get_input("Incorrect filename! Enter new filename: ");
            }
            pattern += filename;
            string[] lines = File.ReadAllLines(pattern);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields_value = lines[i].Split(" ");
                Auto temp = new Auto(fields_value);
                this.Push(temp);
            }
        }
        public void Output()
        {
            string filename = @"C:\4term\task2\task2\Output.txt";
            File.WriteAllText(filename, string.Empty);
            using (TextWriter tw = new StreamWriter(filename))
            {
                foreach (var item in autos)
                {
                    tw.WriteLine(item);
                }
            }
        }
        public void Sort()
        {
            var selected_field = this.Get_input("Select a field to sort by: ");
            var properties = autos[0].GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == selected_field)
                {
                    autos = autos.OrderBy(auto => auto.GetType().GetProperty(selected_field).GetValue(auto).ToString().ToLower()).ToList();
                    break;
                }
            }
        }
        public void Search()
        {
            var selected_value = this.Get_input("Enter what you\'d like to find: ");
            var properties = autos[0].GetType().GetProperties();
            for (int i = 0; i < autos.Count(); i++)
            {
                foreach (var property in properties)
                {
                    if (property.GetValue(autos[i]).ToString().Contains(selected_value))
                    {
                        Console.WriteLine(autos[i]);
                    }
                }
            }
        }
        public void Edit()
        {
            var selected_id = this.Get_input("Enter id of the element, which you\'d like to edit: ");
            var selected_field = this.Get_input("Select a field to edit by: ");
            var selected_value = this.Get_input($"Enter new value of {selected_field}\'s field: ");
            for (int i = 0; i < autos.Count(); i++)
            {
                if (autos[i].Id == selected_id)
                {
                    Auto temp = new Auto(autos[i]);
                    var propertyInfo = temp.GetType().GetProperty(selected_field);
                    propertyInfo.SetValue(temp, Convert.ChangeType(selected_value, propertyInfo.PropertyType), null);
                    if (this.Push(temp))
                    {
                        this.Pop(autos[i]);
                    }
                    break;
                }
            }
        }
    }
}
