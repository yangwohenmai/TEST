using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Net.Http;

namespace MoreThread  
{
    class Program
    {
        //static SemaphoreSlim semLim = new SemaphoreSlim(4); //3表示最多只能有三个线程同时访问
        public static void Main(string[] args)
        {
            GetStringAsync();
            //GetVal(6, 0);
            Console.ReadLine();
        }

        public static void GetStringAsync()
        {
            try
            {
                HttpClient hc = new HttpClient();
                var task1 = hc.GetStringAsync("http://www.1111.com");
                var task2 = hc.GetStringAsync("http://www.2222.com");
                var task3 = hc.GetStringAsync("http://www.3333.com");
                Task.WaitAll(task1, task2, task3);
                Console.WriteLine("执行成功");
            }
            //AggregateException类表示应用程序执行期间发生的一个或多个错误。（即：所有的内部异常）
            catch (AggregateException ae)
            {
                //InnerExceptions表示获取导致当前异常的System.Exception实例的只读集合。
                foreach (Exception item in ae.InnerExceptions)
                {
                    Console.WriteLine(item.Message);
                }
                Console.ReadKey();
            }
        }












        private static int Sum(int x, int y)
        {
            return x / y;
        }
        public static void GetVal(int x, int y)
        {
            Task<string> t = Task.Run<string>(() => { return Sum(x, y).ToString(); });
            Thread.Sleep(3000);
            Console.WriteLine("异常信息：" + t.Exception.InnerException.Message);
            //t.ContinueWith(r => { Console.WriteLine("异常信息：" + t.Exception.InnerException.Message); });
            //t.ContinueWith(r => { Console.WriteLine("异常信息：" + t.Exception.InnerException.Message); }, TaskContinuationOptions.OnlyOnFaulted);
            //ContinueWith方法是完成时异步执行的延续任务，这个延续认为是带一个参数的WriteLlog方法
            //t.ContinueWith(r => { WriteLog("异常信息：" + t.Exception.InnerException.StackTrace); }, TaskContinuationOptions.OnlyOnFaulted);
            //askContinuationOptions.OnlyOnFaulted表示：指定只应在延续任务前面的任务引发了未处理异常的情况下才安排延续任务。 此选项对多任务延续无效。【即：只有在发生异常的情况下才将异常信息记录到日志】
        }
        static void WriteLog(string logStr)
        {
            System.IO.File.AppendAllText(@"D:\Log.txt", logStr + "\r\n");
        }
    }
}
