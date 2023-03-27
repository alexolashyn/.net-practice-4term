using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Reflection;
using System.Text.Json;

namespace proxy
{
    
    internal class User
    {
        private string firstname;
        private string lastname;
        private string email;
        private string password;
        private string role;
        public string Firstname
        { get { return firstname; } set { firstname = Regex.IsMatch(value, @"^[A-Z][a-z]+(-[A-Z][a-z]+)?$") ? value : "-1"; } }
        public string Lastname { get { return lastname; } set { lastname = Regex.IsMatch(value, @"^[A-Z][a-z]+(-[A-Z][a-z]+)?$") ? value : "-1"; } }
        
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = Regex.IsMatch(value, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") ? value : "-1";
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = Regex.IsMatch(value, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$") ? value : "-1";
            }
        }
        public string Role
        {
            get
            {
                return role;
            }
            set
            {
                role = value == "admin" || value == "customer" ? value : "-1";
            }
        }
        public User() { }

        public void CreateUser(string[] fields, string role = null)
        {
            try
            {
                Firstname = fields[0];
                Lastname = fields[1];
                Email = fields[2];
                Password = fields[3];
                Role = role;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public override string ToString()
        {
            return $"{firstname}, {lastname}, {email}, {password}, {role}";
        }
    }
    internal class Registration
    {
        private Validation validator;
        private bool isActive;
        private User user;
        public Registration(User new_user)
        {
            validator = new Validation();
            isActive = false;
            user = new_user;
        }
        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }
        public User ThisUser
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }

        public void SignUp()
        {
            validator.validate(user);
            if (!validator.Is_valid)
            {
                validator.show_errors();
            }
            else
            {
                List<User> lst = new List<User>();
                string filename = @"C:\np_4term\proxy\proxy\RegistratedUsers.json";
                using (StreamReader r = new StreamReader(filename))
                {
                    string json = r.ReadToEnd();
                    var users = JsonSerializer.Deserialize<List<User>>(json);
                    users.Add(user);
                    lst = users;
                }
                string output = JsonSerializer.Serialize(lst);
                File.WriteAllText(filename, output);
                isActive = true;
            }
            validator = new Validation();
        }
        public void Login(string role)
        {
            string filename = @"C:\np_4term\proxy\proxy\RegistratedUsers.json";
            if (role == "admin")
            {
                filename = @"C:\np_4term\proxy\proxy\admins.json";
            }
            Console.Write("Enter your email: ");
            string email = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();
            using (StreamReader r = new StreamReader(filename))
            {
                string json = r.ReadToEnd();
                var users = JsonSerializer.Deserialize<List<User>>(json);
                foreach (var u in users)
                {
                    if (u.Email == email && u.Password == password)
                    {
                        var properties = user.GetType().GetProperties();
                        foreach (var property in properties)
                        {
                            property.SetValue(user, u.GetType().GetProperty(property.Name).GetValue(u));
                        }
                        isActive = true;
                        break;
                    }
                }

            }
        }
        public void Logout()
        {
            isActive = false;
        }

    }
}
