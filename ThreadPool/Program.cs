using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace ThreadPoolTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //ThreadAction();
            IoThread();
            //ThreadUseAndConstruction();
            Console.ReadLine();
        }


        public static void ThreadAction()
        {
            List<Action> actions = new List<Action>();
            actions.Add(() => { Console.WriteLine("A-1"); });
            actions.Add(() => { Console.WriteLine("A-2"); });
            actions.Add(() => { Console.WriteLine("A-3"); });
            actions.Add(() => { Console.WriteLine("A-4"); });
            for (var i = 0; i < 10; i++)
            {
                foreach (var action in actions)
                {
                    ThreadPool.QueueUserWorkItem(state => action(), null);
                    //var tempAction = action;
                    //ThreadPool.QueueUserWorkItem(state => tempAction(), null);
                }
            }
        }

        /// <summary>
        /// 使用WaitCallback委托
        /// </summary>
        public static void ThreadUseAndConstruction()
        {
            ThreadPool.SetMinThreads(5, 5); // set min thread to 5
            ThreadPool.SetMaxThreads(15, 15); // set max thread to 12

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //WaitCallback 委托：表示线程池线程要执行的回调方法
            //WaitCallback 只是一个用在线程里的委托方法，
            WaitCallback callback = index =>
            {
                Console.WriteLine(String.Format("{0}: Task {1} started", watch.Elapsed, index));
                Thread.Sleep(1000);
                Console.WriteLine(String.Format("{0}: Task {1} finished", watch.Elapsed, index));

            };

            for (int i = 0; i < 50; i++)
            {
                ThreadPool.QueueUserWorkItem(callback, i);
                Thread.Sleep(10000);
            }

        }

        /// <summary>
        /// 线程阻塞
        /// </summary>
        public static void IoThread()
        {
            ThreadPool.SetMinThreads(5, 2);
            ThreadPool.SetMaxThreads(5, 2);

            //定义成false阻塞线程
            ManualResetEvent waitHandle = new ManualResetEvent(false);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            WebRequest request = HttpWebRequest.Create("https://www.baidu.com/");
            request.BeginGetResponse(ar =>
            {
                var response = request.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get");

            }, null);

            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(index =>
                {
                    Console.WriteLine(String.Format("{0}: Task {1} started", watch.Elapsed, index));
                    //WaitOne会等待Set()方法释放信号，才继续执行
                    waitHandle.WaitOne();
                    Console.WriteLine(String.Format("{0}: Task {1} finish", watch.Elapsed, index));

                }, i);
            }
            Thread.Sleep(10000);
            Console.WriteLine("释放");
            waitHandle.Set();
        }
    }
}
