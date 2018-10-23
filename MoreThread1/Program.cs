using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoreThread1
{
    class Program
    {
        static void Main(string[] args)
        {

            for (int i = 1; i <= 9; i++)//向线程池中排入9个工作线程
            {
                //QueueUserWorkItem()方法：将工作任务排入线程池。
                ThreadPool.QueueUserWorkItem(new WaitCallback(Fun), i);
                // Fun 表示要执行的方法(与WaitCallback委托的声明必须一致)。
                // i   为传递给Fun方法的参数(obj将接受)。
            }

            for (int i = 1; i <= 999999999; i++)
            {
                i++;
            }
            //输出计算结果
            for (int i = 1; i <= 9; i++)
            {
                Console.WriteLine("线程{0}: {0}! = {1}", i, result[i]);
            }

            Console.ReadLine();
        }

        // 用于保存每个线程的计算结果
        static int[] result = new int[10];


        //注意：由于WaitCallback委托的声明带有参数，
        //      所以将被调用的Fun方法必须带有参数，即：Fun(object obj)。
        static void Fun(object obj)
        {
            int n = (int)obj;

            //计算阶乘
            int fac = 1;
            for (int i = 1; i <= n; i++)
            {
                fac *= i;
            }
            //保存结果
            result[n] = fac;
        }

    }
}
