using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace task5
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
                Context c = new Context(new Draft());
                int i = 0;
                foreach (var property in properties)
                {
                    if (property.Name == "ThisState")
                    {
                        property.SetValue(el, c);
                        continue;
                    }
                    property.SetValue(el, Convert.ChangeType(fields[i++], property.PropertyType));
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
            ev.Description = $"Id: {selected_id}";
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
        public void Show(Event ev, bool draft = false, bool moderated = false)
        {
            if (data.Any())
            {
                Context toCompare = new Context(new Published());
                if (draft)
                {
                    toCompare = new Context(new Draft());
                }
                if (moderated)
                {
                    toCompare = new Context(new Moderated());
                }
                ev.Description = "[";
                foreach (T el in data)
                {
                    if (data_type.GetProperty("ThisState").GetValue(el).Equals(toCompare))
                    {
                        Console.WriteLine(el);
                        ev.Description += $"{el}\n";
                    }

                }
                if (Regex.IsMatch(ev.Description, @"\[\w+"))
                {
                    ev.Description = ev.Description.Remove(ev.Description.Length - 1, 1);
                }
                ev.Description += "]";
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
            if (el != null && data_type.GetProperty("ThisState").GetValue(el).Equals(new Context(new Published())))
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
            return Console.ReadLine();
        }
        public string FileInput()
        {
            string filename = GetInput("Enter filename: ");
            string pattern = @"C:\np_4term\task5\task5\";
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
                    if (property.GetValue(data[i]).ToString().Contains(selected_value) && data_type.GetProperty("ThisState").GetValue(data[i]).Equals(new Context(new Published())))
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
            int temp_index = data.FindIndex(x => data_type.GetProperty("Id").GetValue(x, null).ToString() == selected_id);
            if (temp_index != -1)
            {
                T temp = (T)Activator.CreateInstance(data_type, data[temp_index]);
                Auto auto = temp as Auto;
                auto.ThisState.Request('d');
                ev.Description += $" previous: {temp}";
                var selected_field = GetInput("Select a field to edit by: ");
                var properties = data_type.GetProperties();
                foreach (var property in properties)
                {
                    if (property.Name == selected_field)
                    {
                        var selected_value = GetInput("Enter new value: ");
                        property.SetValue(auto, Convert.ChangeType(selected_value, property.PropertyType));
                    }
                }
                temp = (T)Activator.CreateInstance(data_type, auto);
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
        public void Publish(Event ev)
        {
            string message = "1 - to publish all\n2 - to publish all on moderation\n3 - to publish by id\n";
            string choice = GetInput(message);
            if (choice == "3")
            {
                string selected_id = GetInput("Enter id: ");
                int temp_index = data.FindIndex(x => data_type.GetProperty("Id").GetValue(x, null).ToString() == selected_id);
                if (temp_index != -1)
                {
                    Auto auto = data[temp_index] as Auto;
                    auto.ThisState.Request('p');
                    data[temp_index] = (T)Activator.CreateInstance(data_type, auto);
                }
            }
            else if (new String(message.Where(Char.IsDigit).ToArray()).Contains(Char.Parse(choice)))
            {
                for (int i = 0; i < data.Count; i++)
                {
                    Auto auto = data[i] as Auto;
                    if (choice == "2" && auto.ThisState.Equals(new Context(new Moderated())) || choice == "1")
                    {
                        auto.ThisState.Request('p');
                    }
                    data[i] = (T)Activator.CreateInstance(data_type, auto);
                }
                ev.Description = $"{this}";
            }
            else
            {
                Console.WriteLine("There is no such an option in the menu!");
                ev.Description = "Wrong action";
            }
        }
        public void Moderate(Event ev)
        {
            string message = "1 - to moderete all\n2 - to moderate by id\n";
            string choice = GetInput(message);
            if (choice == "2")
            {
                string selected_id = GetInput("Enter id: ");
                int temp_index = data.FindIndex(x => data_type.GetProperty("Id").GetValue(x, null).ToString() == selected_id);
                if (temp_index != -1)
                {
                    Auto auto = data[temp_index] as Auto;
                    if (auto.ThisState.Equals(new Context(new Draft())))
                    {
                        auto.ThisState.Request('m');
                        data[temp_index] = (T)Activator.CreateInstance(data_type, auto);
                    }
                }
            }
            else if (message.Where(Char.IsDigit).ToString().Contains(choice))
            {
                for (int i = 0; i < data.Count; i++)
                {

                    Auto auto = data[i] as Auto;
                    if (auto.ThisState.Equals(new Context(new Draft())))
                    {
                        auto.ThisState.Request('m');
                        data[i] = (T)Activator.CreateInstance(data_type, auto);
                    }
                }
                ev.Description = $"{this}";
            }
            else
            {
                Console.WriteLine("There is no such an option in the menu!");
                ev.Description = "Wrong action";
            }
        }
    }
}
