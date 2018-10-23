using System;
using System.Collections.Generic;

namespace TestDll
{
    public class Class1
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Hello World!");
            Console.ReadLine();



            Dictionary<string, string> s = new Dictionary<string, string>();
            s.Add("1", "1");
            s.Add("2", "2");
            s.Add("3", "3");
            s.Add("4", "4");
            s.Add("5", "5");
            s.Add("6", "6");
            s.Add("7", "7");
            s.Add("8", "8");
            s.Add("9", "9");

            Dictionary<string, string> s1 = new Dictionary<string, string>();
            s1.Add("1", "1");
            s1.Add("2", "2");
            s1.Add("3", "3");
            s1.Add("4", "4");
            s1.Add("5", "5");
            s1.Add("6", "6");
            s1.Add("7", "7");
            s1.Add("8", "8");
            s1.Add("9", "9");

            foreach (var item in s)
            {
                s1.Add("0", "0");
            }



        }
    }
}
