using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace task1
{
    internal class Program
    {
        public static int Integer_input(string message)
        {
            Console.Write(message);
            try
            {
                int result = Int32.Parse(Console.ReadLine());
                return result;
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return Integer_input(message);
            }
        }
        public static int Size_input()
        {
            try
            {
                int result = Integer_input("Enter size of your arrays: ");
                if (result < 2)
                {
                    throw new Exception("Size of your arrays cannot be less than 2!");
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Size_input();
            }

        }
        public static Dictionary<char, int>Interval_input()
        {
            try
            {
                Dictionary<char, int> interval_endpoints = new Dictionary<char, int>();
                int a = Integer_input("Enter the start of the interval: ");
                int b = Integer_input("Enter the end of the interval: ");
                if (b <= a)
                {
                    throw new Exception("The start should be lower then the end!");
                }
                interval_endpoints.Add('a', a);
                interval_endpoints.Add('b', b);
                return interval_endpoints;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Interval_input();
            }
        }
        public static int[] Arrays_input(int[] arr)
        {
            Console.Write("Input your array: ");
            string temp = Console.ReadLine();
            bool match = Regex.IsMatch(temp, @"^((-?\d)+\s){" + (arr.Length - 1) + @"}(-?\d)+$");
            if (match)
            {
                string[] temp_arr = temp.Split(" ");
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = Int32.Parse(temp_arr[i]);
                }
                return arr;
            }
            Console.WriteLine("Format is incorrect. Try again!");
            return Arrays_input(arr);
        }
        public static void Arrays_generation(int[] arr, Dictionary<char, int> interval_endpoints)
        {
            var rnd = new Random();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rnd.Next(interval_endpoints['a'], interval_endpoints['b']+1);
            }
        }
        public static Dictionary<string, int>Max_element_position(int[] arr)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("element", arr.Max());
            result.Add("position", arr.ToList().IndexOf(result["element"]));
            return result;
        }
        public static bool Negative_check(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > 0)
                {
                    return false;
                }
            }
            return true;
        }
        public static int[] Arrays_checking(int[] arr1, int[] arr2)
        {
            int k = Integer_input("Enter the number, which you\'d like to find out in the first array: ");
            Dictionary<string, int> max1 = Max_element_position(arr1);
            Dictionary<string, int> max2 = Max_element_position(arr2);
            double length = arr1.Length / 2;
            if ((k != max1["element"]) || (Math.Round(length) < max1["position"]) || (!Negative_check(arr2)))
            {
                return arr2;
            }
            for (int i = 0; i < arr2.Length; i++)
            {
                if (i != max2["position"])
                {
                    arr2[i] = (int)Math.Pow(arr2[i], 3);
                    continue;
                }
                break;
            }
            return arr2;
        }
        public static bool Menu()
        {
            int user_choice = Integer_input("\n1 - to enter elements of arrays\n2 - to get arrays randomly generated\n3 - to exit\n");
            int[] options = { 1, 2, 3 };
            if (!(options.Contains(user_choice)))
            {
                Console.WriteLine($"There is no option '{user_choice}' in the menu. Try again!");
                return Menu();
            }
            if (user_choice == 3)
            {
                Console.WriteLine("The session is over!");
                return false;
            }
            int size = Size_input();
            int[] arr1 = new int[size];
            int[] arr2 = new int[size];
            int[] arr3 = new int[size];
            if (user_choice == 1)
            {
                arr1 = Arrays_input(arr1);
                arr2 = Arrays_input(arr2);
                arr3 = Arrays_input(arr3);
            }
            else
            {
                Dictionary<char, int> interval = Interval_input();
                Arrays_generation(arr1, interval);
                Arrays_generation(arr2, interval);
                Arrays_generation(arr3, interval);
            }
            int[]result = Arrays_checking(arr1, arr2);
            for (int i = 0; i < result.Length; i++)
            {
                Console.Write($"{result[i]} ");
            }
            return Menu();
        }
        static void Main(string[] args)
        {
            Menu();
        }
    }
}