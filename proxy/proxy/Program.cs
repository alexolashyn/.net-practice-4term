namespace proxy
{
    internal class Program
    {
        static void GetUser(User user)
        {
            Registration user_registration = new Registration(user);
            while (user_registration.IsActive == false)
            {
                Console.Write("Choose your role(customer or admin): ");
                string user_role = Console.ReadLine();
                if (user_role == "customer")
                {
                    Console.WriteLine("1 - Sign-up\n2 - Login");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("Enter user data in format: first_name, last_name, email, password");
                            string[] fields = Console.ReadLine().Split(", ");
                            user_registration.ThisUser.CreateUser(fields, user_role);
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
                else if (user_role == "admin")
                {
                    user_registration.Login(user_role);
                }
                else
                {
                    Console.WriteLine($"There is no option \"{user_role}\" in list of roles. Try again!");
                }
            }
        }
        static public void NewCollection(Event ev, User user)
        {
            Collection<Cinema> new_collection = new Collection<Cinema>();
            string filename = new_collection.FileInput();
            ev.S1 = "Collection<Cinema>";
            ev.S2 = "Colection is created!";
            Menu(new_collection, filename, user);
        }

        static void Menu<T>(Collection<T> collection, string filename, User user) where T : class
        {
            bool control = true;
            
            while (control)
            {
                Console.WriteLine("Choose an option:\n1 - to search\n2 - to sort\n3 - to delete element\n4 - to add element\n5 - to edit\n6 - to view collection\n7 - to view element by id\n8 - to create Cinema collection\n9 - to exit");
                string user_choice = Console.ReadLine();
                Event ev = new Event();
                switch (user_choice)
                {
                    case "9":
                        Console.WriteLine("The session is over!");
                        control = false;
                        continue;
                    case "1":
                        if(PermissionProxy.CheckPermission(user, "Search", ev))
                        {
                            collection.Search(ev);
                        }
                        break;
                    case "2":
                        if (PermissionProxy.CheckPermission(user, "Sort", ev))
                        {
                            collection.Sort(ev);
                        }
                        break;
                    case "3":
                        if (PermissionProxy.CheckPermission(user, "Pop", ev))
                        {
                            collection.Pop(ev);
                        }
                        break;
                    case "4":
                        if (PermissionProxy.CheckPermission(user, "AddInto", ev))
                        {
                            collection.AddInto(ev);
                        }
                        break;
                    case "5":
                        if (PermissionProxy.CheckPermission(user, "Edit", ev))
                        {
                            collection.Edit(ev);
                        }
                        break;
                    case "6":
                        if (PermissionProxy.CheckPermission(user, "Show", ev))
                        {
                            collection.Show(ev);
                        }
                        break;
                    case "7":
                        if (PermissionProxy.CheckPermission(user, "ShowById", ev))
                        {
                            collection.ShowById(ev);
                        }
                        break;
                    case "8":
                        if (PermissionProxy.CheckPermission(user, "NewCollection", ev))
                        {
                            NewCollection(ev, user);
                        }
                        break;
                    default:
                        Console.WriteLine($"There is no option '{user_choice}' in the menu. Try again!");
                        break;
                }
                ProxyLogger.UpdateEvents(ev);
                collection.FileOutput(filename);
            }
        }
        static void Main(string[] args)
        {
            User user = new User();
            GetUser(user);
            Collection<Auto> collection = new Collection<Auto>();
            string filename = collection.FileInput();
            Menu(collection, filename, user);
        }
    }
}