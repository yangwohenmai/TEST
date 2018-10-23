using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadText
{
    class Program
    {
        static void Main(string[] args)
        {
            //string str = System.IO.File.ReadAllText("C:\\Users\\admin\\Desktop\\基金实时估值\\CurrentNav.txt");
            FileInfo fi = new FileInfo("C:\\Users\\admin\\Desktop\\基金实时估值\\CurrentNav.txt");
            string a = fi.CreationTime.ToString("yyyyMMdd");
            Console.WriteLine(fi.CreationTime.ToString());
            Console.ReadKey();
        }
    }
}
