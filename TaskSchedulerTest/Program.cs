using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //线程池调度测试
            //TaskTest1();

            //有最大并发数量的线程池
            //TaskTest2();

            //解决LimitedConcurrencyLevelTaskScheduler创建线程缓慢的问题
            TaskTest3();

            //解决TaskFactory创建线程缓慢的问题
            //TaskTest4();

            //线程调度器
            //TaskScheduler();
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
        /// 普通的线程池，没有任何限制
        /// 按顺序一个一个的创建线程，直到全部完成
        /// 每个线程创建时间间隔为1s
        /// 1000个线程可能要花1000s才能创建完成，效率低下
        /// </summary>
        public static void TaskTest1()
        {
            TaskFactory fac = new TaskFactory();
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

        /// <summary>
        /// 设置了线程最大并发数量的线程池
        /// 同一时间内，系统中保持有n个线程并发执行
        /// 但是每次创建线程时间间隔仍然是1s中
        /// 整个过程花在线程创建的时间大约n秒，低效
        /// </summary>
        public static void TaskTest2()
        {
            //限制同时只有5个并发线程
            TaskFactory fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(20));

            //TaskFactory fac = new TaskFactory();
            //总共创建1000个线程，所有线程进入线程池等待
            for (int i = 0; i < 1000; i++)
            {
                //向线程池中添加一个线程
                fac.StartNew(s =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("启动线程：" + s);
                    //b++;
                    //每个线程存活20秒，当前线程结束后，线程池会从新分配5个线程
                    Thread.Sleep(10000);
                    Console.WriteLine("Current Index {0}, ThreadId {1}", s, Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("结束线程：" + s);

                }, i);
            }
            Console.ReadLine();
        }

        /// <summary>
        /// 通过SetMinThreads，设置线程池最小数量，解决TaskFactory启动线程缓慢的问题
        /// 在达到线程池最小数量前，可以将所有线程瞬间创建完成，没有等待时间
        /// 若并发数量超过线程池最小数量时，创建线程速度变慢，间隔时间约1s
        /// </summary>
        public static void TaskTest3()
        {
            ThreadPool.SetMinThreads(20, 1000);
            //限制同时只有5个并发线程
            TaskFactory fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(40));

            //TaskFactory fac = new TaskFactory();
            //总共创建1000个线程，所有线程进入线程池等待
            for (int i = 0; i < 1000; i++)
            {
                //向线程池中添加一个线程
                fac.StartNew(s =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("启动线程：" + s);
                    //b++;
                    //每个线程存活20秒，当前线程结束后，线程池会从新分配5个线程
                    Thread.Sleep(10000);
                    Console.WriteLine("Current Index {0}, ThreadId {1}", s, Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("结束线程：" + s);

                }, i);
            }
            Console.ReadLine();
        }


        /// <summary>
        /// 通过设置TaskFactory参数，可以加速线程启动过程
        /// 瞬间将所有线程都启动起来，但此时无法设置最大并发线程参数
        /// 若强行添加最大并发数量参数LimitedConcurrencyLevelTaskScheduler
        /// 则线程池加速启动效果失效
        /// </summary>
        public static void TaskTest4()
        {
            TaskFactory fac = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.LongRunning);
            //总共创建1000个线程，所有线程进入线程池等待
            for (int i = 0; i < 1000; i++)
            {
                //向线程池中添加一个线程
                fac.StartNew(s =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("启动线程：" + s);
                    //b++;
                    //每个线程存活20秒，当前线程结束后，线程池会从新分配5个线程
                    Thread.Sleep(10000);
                    Console.WriteLine("Current Index {0}, ThreadId {1}", s, Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("结束线程：" + s);

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
