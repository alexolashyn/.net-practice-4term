using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace task3
{
    internal class Collection<T> where T : class
    {
        private List<T> data;
        private Type data_type;
        public List<T> Data { get => data; }
        public Type Data_type { get => data_type; }

        public Collection()
        {
            data = new List<T>();
            data_type = typeof(T);
        }
        public bool Push(T element)
        {
            Validation validator = new Validation();
            validator.validate(element);
            if (validator.Is_valid)
            {
                data.Add(element);
                return true;
            }
            validator.show_errors();
            return false;

        }
        public void AddInto()
        {
            string[] temp = this.Get_input("Enter new element: ").Split(", ");
            Type[] types = { typeof(string[]) };
            var constructorInfoObj = data_type.GetConstructor(types);
            object new_el = constructorInfoObj.Invoke(new object[] { temp });
            this.Push((T)Convert.ChangeType(new_el, data_type));
        }
        public void Pop(T element = null)
        {
            if (element == null)
            {
                Console.Write("Enter id of the element, which you\'d like to remove: ");
                var selected_id = Console.ReadLine();
                for (int i = 0; i < data.Count(); i++)
                {
                    if (data[i].GetType().GetProperty("Id").GetValue(data[i], null).ToString() == selected_id)
                    {
                        data.Remove(data[i]);
                        break;
                    }
                }
            }
            else
            {
                data.Remove(element);
            }

        }
        public void Show()
        {
            if (data.Any())
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    Console.WriteLine($"---------------------------------\n{data[i]}");
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
            string filename = Get_input("Enter filename: ");
            string pattern = @"C:\np_4term\task3\task3\";
            while (!File.Exists(pattern + filename))
            {
                filename = Get_input("Incorrect filename! Enter new filename: ");
            }
            pattern += filename;
            string[] lines = File.ReadAllLines(pattern);
            Type[] types = { typeof(string[]) };
            var constructorInfoObj = data_type.GetConstructor(types);
            if (constructorInfoObj != null)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] fields_value = lines[i].Split(", ");
                    object new_el = constructorInfoObj.Invoke(new object[] { fields_value });
                    this.Push((T)Convert.ChangeType(new_el, data_type));
                }
            }
            else
            {
                Console.WriteLine("There is no such constructor!");
            }
        }
        public void Output()
        {
            string filename = @"C:\np_4term\task3\task3\Output.txt";
            File.WriteAllText(filename, string.Empty);
            using (TextWriter tw = new StreamWriter(filename))
            {
                foreach (var item in data)
                {
                    tw.WriteLine(item);
                }
            }
        }
        public void Sort()
        {
            var selected_field = Get_input("Select a field to sort by: ");
            var properties = data_type.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == selected_field)
                {
                    data = data.OrderBy(el => property.GetValue(el).ToString().ToLower()).ToList();
                    break;
                }
            }
        }
        public void Search()
        {
            var selected_value = Get_input("Enter what you\'d like to find: ");
            var properties = data_type.GetProperties();
            for (int i = 0; i < data.Count(); i++)
            {
                foreach (var property in properties)
                {
                    if (property.GetValue(data[i]).ToString().Contains(selected_value))
                    {
                        Console.WriteLine(data[i]);
                        break;
                    }
                }
            }
        }
        public void Edit()
        {
            var selected_id = Get_input("Enter id of the element, which you\'d like to edit: ");
            int temp_index = -1;
            for (int i = 0; i < data.Count(); i++)
            {
                if (data_type.GetProperty("Id").GetValue(data[i], null).ToString() == selected_id)
                {
                    temp_index = i;
                    break;
                }
            }
            if (temp_index != -1)
            {
                var selected_field = Get_input("Select a field to edit by: ");
                var properties = data_type.GetProperties();
                foreach (var property in properties)
                {
                    if (property.Name == selected_field)
                    {
                        var selected_value = Get_input($"Enter new value of {selected_field}\'s field: ");
                        T temp = (T)Activator.CreateInstance(data_type, data[temp_index]);
                        property.SetValue(temp, Convert.ChangeType(selected_value, property.PropertyType), null);
                        T result = (T)Activator.CreateInstance(data_type, temp);
                        if (this.Push(result))
                        {
                            this.Pop(data[temp_index]);
                        }
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("There is no element with such id");
            }
        }
    }
}
