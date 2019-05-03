using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulerTest
{
    class Program
    {
        static object b1 = new object();
        static void Main(string[] args)
        {
            //线程调度测试
            //TaskTest1();

            //线程调度器
            TaskScheduler();
        }


        /// <summary>
        /// 线程调度器
        /// </summary>
        public static void TaskScheduler()
        {
            try
            {
                while (true)
                {
                    //设置线程最大并发量为5
                    LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(5);
                    TaskFactory factory = new TaskFactory(lcts);
                    Task[] spiderTask = new Task[] {
                        factory.StartNew(() =>
                        {
                            //立刻执行线程
                            Console.WriteLine("{0} Start on thread {1}", "111", Thread.CurrentThread.ManagedThreadId);
                            Console.WriteLine("{0} Finish  on thread {1}", "111", Thread.CurrentThread.ManagedThreadId);
                        }),
                        factory.StartNew(() =>
                        {
                            //等待3s执行此线程
                            Thread.Sleep(TimeSpan.FromSeconds(3));
                            Console.WriteLine("{0} Start on thread {1}", "222", Thread.CurrentThread.ManagedThreadId);
                            Console.WriteLine("{0} Finish  on thread {1}", "222", Thread.CurrentThread.ManagedThreadId);
                        }),
                        factory.StartNew(() =>
                        {
                            //等待5s执行此线程
                            Thread.Sleep(TimeSpan.FromSeconds(5));
                            Console.WriteLine("{0} Start on thread {1}", "333", Thread.CurrentThread.ManagedThreadId);
                            Console.WriteLine("{0} Finish  on thread {1}", "333", Thread.CurrentThread.ManagedThreadId);
                        })
                        };
                    Task.WaitAll(spiderTask);
                    //每隔1s循环一次队列
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
            }
            catch (AggregateException ex)
            {
                foreach (Exception inner in ex.InnerExceptions)
                {
                    //Log.Logger.Error(inner.Message);
                }
            }
        }



        /// <summary>
        /// 多线程测试1
        /// </summary>
        public static void TaskTest1()
        {
            TaskFactory fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(5));

            //TaskFactory fac = new TaskFactory();
            for (int i = 0; i < 1000; i++)
            {

                fac.StartNew(s =>
                {
                    //Thread.Sleep(1000);
                    Console.WriteLine("启动线程：" + s);
                    //Write_txt("启动线程：" + b);
                    Thread.Sleep(20000);
                    Console.WriteLine("Current Index {0}, ThreadId {1}", s, Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("结束线程：" + s);
                    //Write_txt("结束线程：" + b);

                }, i);
            }

            Console.ReadLine();
        }

        //private static void Write_txt(string log)
        //{
        //    lock (b1)
        //    {
        //        string path = "D:\\YSJS-BK-Finance\\logs1\\";//文件路径
        //        string logFileName = Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + "PEQ4004BLError.log");//全部路径
        //        if (!Directory.Exists(path))//若文件夹不存在则新建文件夹   
        //            Directory.CreateDirectory(path); //新建文件夹   

        //        File.AppendAllText(logFileName, DateTime.Now.ToString() + " ");
        //        File.AppendAllText(logFileName, log);
        //        File.AppendAllText(logFileName, Convert.ToChar(13).ToString());
        //        File.AppendAllText(logFileName, Convert.ToChar(10).ToString());
        //    }


        //}

    }
}
