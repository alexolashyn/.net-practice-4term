namespace task5
{
    internal class Program
    {
        static Registration GetUser()
        {
            User user = new User();
            Registration user_registration = new Registration(user);
            Console.Write("Choose your role(customer, admin or manager): ");
            string user_role = Console.ReadLine();
            string choice = "";
            if (user_role == "customer")
            {
                Console.WriteLine("1 - Sign-up\n2 - Login");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter user data in format: first_name, last_name, email, password");
                        List<string> fields = Console.ReadLine().Split(", ").ToList();
                        fields.Add("customer");
                        var properties = user_registration.ThisUser.GetType().GetProperties();
                        int i = 0;
                        foreach (var property in properties)
                        {
                            property.SetValue(user_registration.ThisUser, Convert.ChangeType(fields[i++], property.PropertyType));
                        }
                        user_registration.SignUp();
                        break;
                    case "2":
                        user_registration.Login(user_role);
                        break;
                    default:
                        Console.WriteLine($"There is no option \"{choice}\" in menu. Try again!");
                        break;
                }
            }
            else if (user_role == "admin" || user_role == "manager")
            {
                user_registration.Login(user_role);
            }
            else
            {
                Console.WriteLine($"There is no option \"{user_role}\" in list of roles. Try again!");
            }
            return user_registration;
        }

        static void Main(string[] args)
        {
            Registration user = GetUser();
            while (!user.IsActive)
            {
                user = GetUser();
            }
            Collection<Auto> collection = new Collection<Auto>();
            string filename = collection.FileInput();
            bool control = true;
            while (control)
            {
                Console.WriteLine("Choose an option:\n1 - to search\n2 - to sort\n3 - to delete element\n4 - to add element\n5 - to edit\n6 - to view collection\n7 - to view element by id\n8 - to view drafts\n9 - to view items om moderation\n10 - to send for moderation\n11 - to publish\n12 - to logout\n13 - to exit");
                string user_choice = Console.ReadLine();
                Event ev = new Event();
                switch (user_choice)
                {
                    case "13":
                        Console.WriteLine("The session is over!");
                        control = false;
                        continue;
                    case "1":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "Search", ev))
                        {
                            collection.Search(ev);
                        }
                        break;
                    case "2":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "Sort", ev))
                        {
                            collection.Sort(ev);
                        }
                        break;
                    case "3":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "Pop", ev))
                        {
                            collection.Pop(ev);
                        }
                        break;
                    case "4":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "AddInto", ev))
                        {
                            collection.AddInto(ev);
                        }
                        break;
                    case "5":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "Edit", ev))
                        {
                            collection.Edit(ev);
                        }
                        break;
                    case "6":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "Show", ev))
                        {
                            collection.Show(ev);
                        }
                        break;
                    case "7":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "ShowById", ev))
                        {
                            collection.ShowById(ev);
                        }
                        break;
                    case "8":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "ShowDrafts", ev))
                        {
                            collection.Show(ev, draft: true);
                        }
                        break;
                    case "9":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "ShowModerated", ev))
                        {
                            collection.Show(ev, moderated: true);
                        }
                        break;
                    case "10":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "Moderation", ev))
                        {
                            collection.Moderate(ev);
                        }
                        break;
                    case "11":
                        if (PermissionProxy.CheckPermission(user.ThisUser, "Publishing", ev))
                        {
                            collection.Publish(ev);
                        }
                        break;
                    case "12":
                        user.Logout();
                        ev.ActionName = "Logout";
                        ev.UserName = String.Join(' ', user.ThisUser.Firstname, user.ThisUser.Lastname);
                        ev.Description = $"User {ev.UserName} is not active";
                        while (!user.IsActive)
                        {
                            user = GetUser();
                        }
                        break;
                    default:
                        Console.WriteLine($"There is no option '{user_choice}' in the menu. Try again!");
                        ev.ActionName = "Wrong Action";
                        ev.UserName = String.Join(' ', user.ThisUser.Firstname, user.ThisUser.Lastname);
                        ev.Description = $"There is no option '{user_choice}' in the menu";
                        break;
                }
                ProxyLogger.UpdateEvents(ev);
            }
        }
    }
}