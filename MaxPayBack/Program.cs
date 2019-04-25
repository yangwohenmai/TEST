using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxPayBack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("3,1,6,5,4,3,6,4,8,2");
            string listtext = Console.ReadLine();
            string[] Datelist = listtext.Split(',');
            decimal max = decimal.MinValue;
            decimal min = decimal.MaxValue;
            decimal payback = decimal.MinValue;
            string oldmax = "";
            string oldmin = "";
            SortedList<decimal, decimal> SOR = new SortedList<decimal, decimal>();
            List<decimal> res = new List<decimal>();
            foreach (var new_date in Datelist)
            {
                decimal dec_new_date = Convert.ToDecimal(new_date);
                if (max < dec_new_date)
                {
                    max = dec_new_date;
                    min = dec_new_date;
                }
                if (min > dec_new_date)
                {
                    min = dec_new_date;
                }
                if ((max - min) / max > payback && max != min)
                {
                    oldmax = max.ToString();
                    oldmin = min.ToString();
                    payback = (max - min) / max;
                }
            }
            Console.WriteLine("最大回撤" + payback.ToString());
            Console.WriteLine("最大值" + oldmax.ToString());
            Console.WriteLine("最小值" + oldmin.ToString());
            Console.ReadLine();
        }
    }
}
