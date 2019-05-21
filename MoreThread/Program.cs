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
        public static void Main(string[] args)
        {
            //通过Wait捕获异常
            //WaitException();
            //通过WaitAll捕获异常
            //WaitAllException();
            //通过ContinueWith设置TaskContinuationOptions参数来捕获异常
            ContinueWithException(6, 0);
            //直接从线程结果中获取异常信息
            //GetException(6, 0);
            Console.WriteLine("主线程完毕");
            Console.ReadLine();
        }


        #region 通过Wait捕获异常
        /// <summary>
        /// 通过wait可以捕获Task内部的异常
        /// </summary>
        public static void WaitException()
        {
            try
            {
                //和线程不同，Task中抛出的异常可以捕获，但是也不是直接捕获，而是由调用Wait()方法或者访问Result属性的时候，由他们获得异常，将这个异常包装成AggregateException类型,或者直接以Exception，抛出捕获。

                //默认情况下，Task任务是由线程池线程异步执行。要知道Task任务的是否完成，可以通过task.IsCompleted属性获得，也可以使用task.Wait来等待Task完成。
                Task t = Task.Run(() => TestException());
                t.Wait();
            }
            catch (Exception ex)
            {
                var a = ex.Message; //a的值为：发生一个或多个错误。
                var b = ex.GetBaseException(); //b的值为：Task异常测试
                Console.WriteLine(a + "|*|" + b);
            }
        }
        static void TestException()
        {
            throw new Exception("Task异常测试");
        }
        #endregion




        #region 用WaitAll可以通过catch获取WaitAll等待的，所有线程的，内部的异常
        public static void WaitAllException()
        {
            try
            {
                HttpClient hc = new HttpClient();
                var task1 = hc.GetStringAsync("http://www.1111.com");
                var task2 = hc.GetStringAsync("http://www.2222.com");
                var task3 = Task.Run(() => { int a = 1, b = 0; int c = a / b; });
                //只有WaitAll可以通过AggregateException捕获程序内部所有异常，WaitAny，WhenAll，WhenAny都不能捕获
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
        #endregion





        #region 通过ContinueWith设置TaskContinuationOptions参数来捕获异常
        public static void ContinueWithException(int x, int y)
        {
            Task<string> t = Task.Run<string>(() => 
            {
                Thread.Sleep(3000);
                Console.WriteLine("我是线程还在异步执行");
                return Sumt(x, y).ToString();
            });
            
            //NotOnFaulted表示如果没有异常，才会执行ContinueWith内部的代码，但此时线程不会阻塞
            //t.ContinueWith(r => 
            //{
            //    string Exception = Convert.ToString(t.Exception); 
            //    Console.WriteLine("异常信息1：" + Exception);
            //}, TaskContinuationOptions.NotOnFaulted);
            //Console.WriteLine("继续异步执行1");

            //OnlyOnFaulted表示如果有异常，才会执行ContinueWith内部的代码，但此时线程不会被阻塞
            t.ContinueWith(r =>
            {
                //Thread.Sleep(3000);
                string Exception = Convert.ToString(t.Exception);
                Console.WriteLine("异常信息2：" + Exception);
            }, TaskContinuationOptions.OnlyOnFaulted);
            
            Console.WriteLine("继续异步执行2");


            //askContinuationOptions.OnlyOnFaulted表示：指定只应在延续任务前面的任务引发了未处理异常的情况下才安排延续任务。 此选项对多任务延续无效。【即：只有在发生异常的情况下才将异常信息记录到日志】
        }
        private static int Sumt(int x, int y)
        {
            return x / y;
        }
        #endregion






        #region 对于线程，外层的catch抓不住线程的报错，通过t.Exception对象获取线程的异常
        public static void GetException(int x, int y)
        {
            Task<string> t = Task.Run<string>(() => { return Sum(x, y).ToString(); });
            Thread.Sleep(8000);
            //t.ContinueWith(r => { Console.WriteLine("异常信息：" + t.Exception.InnerException.Message); });
            if (t.IsCompleted == true)
            {
                if (t.Exception != null)//Exception对象不为null
                    if (t.Exception.InnerExceptions.Count > 0)//内部异常信息数量大于0
                        if (!string.IsNullOrEmpty(t.Exception.Message))//ErrorMessage不为空
                            Console.WriteLine("异常信息：" + t.Exception.InnerException.Message);
            }


            //t.ContinueWith(r => { Console.WriteLine("异常信息：" + t.Exception.InnerException.Message); });
            
            //t.ContinueWith(r => { WriteLog("异常信息：" + t.Exception.InnerException.StackTrace); }, TaskContinuationOptions.OnlyOnFaulted);
            //askContinuationOptions.OnlyOnFaulted表示：指定只应在延续任务前面的任务引发了未处理异常的情况下才安排延续任务。 此选项对多任务延续无效。【即：只有在发生异常的情况下才将异常信息记录到日志】
        }
        private static int Sum(int x, int y)
        {
            return x / y;
        }
        #endregion
    }
}
