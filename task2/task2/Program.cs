using System.Reflection;
using System.ComponentModel.Design;

namespace task2
{
    internal class Program
    {
        public static bool Menu(Collection collection)
        {
            Console.WriteLine("Choose an option:\n1 - to search\n2 - to sort\n3 - to delete auto\n4 - to add auto\n5 - to edit\n6 - print all autos\n7 - to exit");
            string user_choice = Console.ReadLine();
            string[] options = { "1", "2", "3", "4", "5", "6", "7" };
            if (!(options.Contains(user_choice)))
            {
                Console.WriteLine($"There is no option '{user_choice}' in the menu. Try again!");
                return Menu(collection);
            }
            if (user_choice == "7")
            {
                Console.WriteLine("The session is over!");
                return false;
            }
            if (user_choice == "1")
            {
                collection.Search();
            }
            if (user_choice == "2")
            {
                collection.Sort();
            }
            if (user_choice == "3")
            {
                collection.Pop();
                collection.Output();
            }
            if (user_choice == "4")
            {
                collection.AddInto();
                collection.Output();
            }
            if (user_choice == "5")
            {
                collection.Edit();
                collection.Output();
            }
            if (user_choice == "6")
            {
                collection.Show();
            }
            return Menu(collection);
        }
        static void Main(string[] args)
        {
            Collection collection = new Collection();
            collection.Input();
            Menu(collection);
        }
    }
}