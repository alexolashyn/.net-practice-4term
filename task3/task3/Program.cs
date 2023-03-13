using System.Reflection;
using System.ComponentModel.Design;

namespace task3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool control = true;
            Collection<Cinema> collection = new Collection<Cinema>();
            collection.Input();

            while (control)
            {
                Console.WriteLine("Choose an option:\n1 - to search\n2 - to sort\n3 - to delete element\n4 - to add element\n5 - to edit\n6 - print all elements\n7 - to exit");
                string user_choice = Console.ReadLine();
                switch (user_choice)
                {
                    case "7":
                        Console.WriteLine("The session is over!");
                        control = false;
                        break;
                    case "1":
                        collection.Search();
                        break;
                    case "2":
                        collection.Sort();
                        break;
                    case "3":
                        collection.Pop();
                        collection.Output();
                        break;
                    case "4":
                        collection.AddInto();
                        collection.Output();
                        break;
                    case "5":
                        collection.Edit();
                        collection.Output();
                        break;
                    case "6":
                        collection.Show();
                        break;
                    default:
                        Console.WriteLine($"There is no option '{user_choice}' in the menu. Try again!");
                        break;
                }

            }
        }
    }
}