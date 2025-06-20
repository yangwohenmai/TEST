using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lambda
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            int oddNumbers = numbers.Count(n => n % 2 == 1);
            var firstNumbersLessThan6 = numbers.TakeWhile(n => n < 6);
            
            var firstSmallNumbers = numbers.TakeWhile((n, index) => n >= index); //传参方式

            Program test = new Program();
            test.TestMethod(5);

            // Prove that del2 still has a copy of  
            // local variable j from TestMethod.  
            bool result = test.del2(10);//调用函数

            // Output: True  
            Console.WriteLine(result);

            Console.ReadKey();  
        }


        D del;//声明委托类型的变量
        D2 del2;//声明委托类型的变量
        public void TestMethod(int input)
        {
            int j = 0;
            del = () => { j = 10; return j > input; };//定义一个函数但是不执行

            del2 = (x) => { return x == j; };//定义一个函数但是不执行

            Console.WriteLine("j = {0}", j); 
            bool boolResult = del();//调用函数

            
            Console.WriteLine("j = {0}. b = {1}", j, boolResult);
            
        }

        public static void Test2()
        {
            Stock stock = new Stock();
            stock.tdate = 1;
            stock.symbol = "11";
            stock.price = 111.0;
            Stock stock2 = new Stock();
            stock2.tdate = 2;
            stock2.symbol = "22";
            stock2.price = 222.0;
            SortedList<int, Stock> sortedList = new SortedList<int, Stock>();
            sortedList.Add(1, stock);
            sortedList.Add(2, stock2);
            var a = (from item in sortedList where item.Value.price != double.NaN select item.Value.symbol).ToList();
            var a1 = (from item in sortedList where item.Value.price != double.NaN select item.Value).ToDictionary(x => x.symbol, x=>x.price);
            var dictionary = sortedList.Select((key, value) => new { Key = key, Value = key }).ToDictionary(x => x.Key, x => x.Key);
            var dictionary1 = sortedList.ToDictionary(x => x.Key, x => x.Value.symbol);
            //var a1 = (from item in sortedList where item.Value.price != double.NaN select item.Value.price).ToDictionary(p => p.symbol, p => p.price);
        }
    }

    public class Stock
    {
        public int tdate;
        public string symbol;
        public double price;
    }

    delegate bool D();//定义委托类型
    delegate bool D2(int i);//定义委托类型
}
