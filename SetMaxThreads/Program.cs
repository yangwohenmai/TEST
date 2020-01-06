using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace SetMaxThreads
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // List<Action> actions = new List<Action>();
            // actions.Add(() => { Console.WriteLine("A-1"); });
            // actions.Add(() => { Console.WriteLine("A-2"); });
            // actions.Add(() => { Console.WriteLine("A-3"); });
            // actions.Add(() => { Console.WriteLine("A-4"); });
            // for (var i = 0; i < 10; i++)
            // {
            //     foreach (var action in actions)
            //     {
            //         var tempAction = action;
            //         ThreadPool.QueueUserWorkItem(state => tempAction(), null);
            //     }
            // }
            IoThread();
            //ThreadUseAndConstruction();
            Console.ReadLine();
        }

        public static void ThreadUseAndConstruction()
        {
            ThreadPool.SetMinThreads(5, 5); // set min thread to 5
            ThreadPool.SetMaxThreads(15, 15); // set max thread to 15

            Stopwatch watch = new Stopwatch();
            watch.Start();

            WaitCallback callback = index =>
            {
                Console.WriteLine(String.Format("{0}: Task {1} started", watch.Elapsed, index));
                Thread.Sleep(10000);
                Console.WriteLine(String.Format("{0}: Task {1} finished", watch.Elapsed, index));

            };

            for (int i = 0; i < 20; i++)
            {
                ThreadPool.QueueUserWorkItem(callback, i);
            }
        }



        public static void IoThread()
        {
            ThreadPool.SetMinThreads(12, 12);
            ThreadPool.SetMaxThreads(12, 12);

            ManualResetEvent waitHandle = new ManualResetEvent(false);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            WebRequest request = HttpWebRequest.Create("http://www.hao123.com/");
            request.BeginGetResponse(ar =>
            {
                var response = request.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get");
            }, null);

            WebRequest request1 = HttpWebRequest.Create("http://www.baidu.com/");
            request1.BeginGetResponse(ar =>
            {
                var response1 = request1.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get1");

            }, null);


            WebRequest request2 = HttpWebRequest.Create("http://www.baidu.com/");
            
            request2.BeginGetResponse(ar =>
            {
                var response2 = request2.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get2");

            }, null);
            
            //Thread.Sleep(1000);
            ////request1.Abort();
            //RequestCachePolicy policy = new RequestCachePolicy(RequestCacheLevel.Revalidate);
            //WebRequest request3 = HttpWebRequest.Create("http://www.baidu.com/");
            //request3.CachePolicy = policy;
            //request3.BeginGetResponse(ar =>
            //{
            //    var response3 = request3.EndGetResponse(ar);
            //    Console.WriteLine(watch.Elapsed + ": Response Get3");

            //}, null);

            ////request3.
            ////Thread.Sleep(1000);
            ////request3.Abort();
            //WebRequest request4 = HttpWebRequest.Create("http://www.baidu.com/");
            //request4.BeginGetResponse(ar =>
            //{
            //    var response4 = request4.EndGetResponse(ar);
            //    Console.WriteLine(watch.Elapsed + ": Response Get4");

            //}, null);

            try
            {
                WebRequest request5 = WebRequest.Create("http://www.baidu.com/");
                //request5.Timeout = 5000;
                var response5 = request5.GetResponse();
            }
            catch(Exception e)
            {
                string a = "";
            }


            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(index =>
                {
                    Console.WriteLine(String.Format("{0}: Task {1} started", watch.Elapsed, index));
                    waitHandle.WaitOne();

                }, i);
            }

            waitHandle.WaitOne();
        }
    }
}
