using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace proxy
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
        public override string ToString()
        {
            return String.Join('\n', data);
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
        public void SettingValues(string[] fields, IList<PropertyInfo> properties, object el)
        {
            try
            {
                if (fields.Length != properties.Count)
                {
                    throw new Exception("Amount of given values is not equal to amount of properties");
                }
                int i = 0;
                foreach (var property in properties)
                {
                    try
                    {
                        property.SetValue(el, Convert.ChangeType(fields[i++], property.PropertyType));
                    }
                    catch
                    {
                        property.SetValue(el, Convert.ChangeType("-1", property.PropertyType));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void AddInto(Event ev)
        {
            string[] fields = GetInput("Enter new element: ").Split(", ");
            ConstructorInfo constructorInfo = data_type.GetConstructor(Type.EmptyTypes);
            if (constructorInfo != null)
            {
                
                object el = constructorInfo.Invoke(new object[] { });
                IList<PropertyInfo> properties = data_type.GetProperties();
                SettingValues(fields, properties, el);
                ev.Description = String.Join(' ', fields);
                this.Push((T)Convert.ChangeType(el, data_type));
                ev.Description += $" [{this}]";
            }
            else
            {
                Console.WriteLine($"There is no default constructor in {data_type.Name}\'s class");
                ev.Description += " Element is not added!";
            }
        }
        public void Pop(Event ev)
        {
            Console.Write("Enter id of the element, which you\'d like to remove: ");
            var selected_id = Console.ReadLine();
            ev.Description = selected_id;
            var selected_el = data.Find(x => data_type.GetProperty("Id").GetValue(x, null).ToString() == selected_id);
            if (selected_el != null)
            {
                data.Remove(selected_el);
                ev.Description += $" [{this}]";
            }
            else
            {
                ev.Description += " There is no element with such id!";
                Console.WriteLine("There is no element with such id!");
            }
        }
        public void Show(Event ev)
        {
            if (data.Any())
            {
                Console.WriteLine(this);
                ev.Description = $"[{this}]";
            }
            else
            {
                Console.WriteLine("Your collection is empty!");
                ev.Description = "Your collection is empty!";
            }
        }
        public void ShowById(Event ev)
        {
            string selected_id = GetInput("Enter id of element which you\'d like to view: ");
            ev.Description = $"Id: {selected_id}";
            T el = data.Find(x => data_type.GetProperty("Id").GetValue(x).ToString() == selected_id);
            if (el != null)
            {
                ev.Description += $" [{el}]";
                Console.WriteLine(el);
            }
            else
            {
                ev.Description += " There is no element with such id!";
                Console.WriteLine("There is no element with such id!");
            }
        }

        public string GetInput(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            return input;
        }
        public string FileInput()
        {
            string filename = GetInput("Enter filename: ");
            string pattern = @"C:\4term\proxy\proxy\";
            while (!File.Exists(pattern + filename))
            {
                filename = GetInput("Incorrect filename! Enter new filename: ");
            }
            pattern += filename;
            string[] lines = File.ReadAllLines(pattern);
            ConstructorInfo constructorInfo = data_type.GetConstructor(Type.EmptyTypes);
            if (constructorInfo != null)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] fields_value = lines[i].Split(", ");
                    object el = constructorInfo.Invoke(new object[] { });
                    IList<PropertyInfo> properties = el.GetType().GetProperties();
                    SettingValues(fields_value, properties, el);
                    this.Push((T)Convert.ChangeType(el, data_type));
                }
            }
            else
            {
                Console.WriteLine($"There is no default constructor in {data_type.Name}\'s class");
            }
            return pattern;
        }
        public void FileOutput(string filename)
        {
            using (TextWriter tw = new StreamWriter(filename))
            {
                foreach (var item in data)
                {
                    tw.WriteLine(item);
                }
            }
        }
        public void Sort(Event ev)
        {
            var selected_field = GetInput("Select a field to sort by: ");
            ev.Description = $"Sort by {selected_field}";
            var properties = data_type.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == selected_field)
                {
                    data = data.OrderBy(el => data_type.GetProperty(selected_field).GetValue(el).ToString().ToLower()).ToList();
                    ev.Description += $" [{this}]";
                    break;
                }
            }
        }
        public void Search(Event ev)
        {
            var selected_value = GetInput("Enter what you\'d like to find: ");
            ev.Description = $"Search by \"{selected_value}\"";
            var properties = data_type.GetProperties();
            for (int i = 0; i < data.Count(); i++)
            {
                foreach (var property in properties)
                {
                    if (property.GetValue(data[i]).ToString().Contains(selected_value))
                    {
                        ev.Description += $" [{data[i]}]";
                        Console.WriteLine(data[i]);
                        break;
                    }
                }
            }
        }
        public void Edit(Event ev)
        {
            var selected_id = GetInput("Enter id of the element, which you\'d like to edit: ");
            ev.Description = selected_id;
            int temp_index = data.FindIndex(x=> data_type.GetProperty("Id").GetValue(x, null).ToString() == selected_id);
            if (temp_index != -1)
            {
                T temp = (T)Activator.CreateInstance(data_type, data[temp_index]);
                ev.Description += $" previous: {temp}";
                var selected_field = GetInput("Select a field to edit by: ");
                IList<PropertyInfo> properties = data_type.GetProperties();
                foreach (var property in properties)
                {
                    if (property.Name == selected_field)
                    {
                        var selected_value = GetInput($"Enter new value of {selected_field}\'s field: ");
                        try
                        {
                            property.SetValue(temp, Convert.ChangeType(selected_value, property.PropertyType), null);
                        }
                        catch
                        {
                            property.SetValue(temp, Convert.ChangeType("-1", property.PropertyType), null);
                        }
                        break;
                    }
                }
                if (Push(temp))
                {
                    ev.Description += $" new: {temp}";
                    data.Remove(data[temp_index]);
                }
            }
            else
            {
                Console.WriteLine("There is no element with such id");
            }
        }
    }
}
