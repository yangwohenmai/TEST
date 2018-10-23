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
    }
    delegate bool D();//定义委托类型
    delegate bool D2(int i);//定义委托类型
}
