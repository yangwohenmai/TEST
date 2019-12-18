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
            TaskTest3();
            //效果更好
            //TaskTest2();

            //线程调度测试
            //TaskTest1();

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

        /// <summary>
        /// 多线程测试2
        /// 效果更好的测试
        /// </summary>
        public static void TaskTest2()
        {

            //限制同时只有5个并发线程
            TaskFactory fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(10));

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

        public static void TaskTest3()
        {
            //var hp = new HttpWebRequestTest.HttpRequestClient();
            //string reslut = hp.httpGet("https://www.hkex.com.hk/?sc_lang=EN", HttpWebRequestTest.HttpRequestClient.defaultHeaders);
            ////定位token字符串头
            //int index_head = reslut.IndexOf("evLtsLs");
            //if (index_head == -1)
            //{
            //    //LogUtil.Error("Common", "MainFrom", "DoWork", "获取token失败");
            //    Console.WriteLine("获取token失败");
            //    //进入失败队列

            //}
            //string InitToken = reslut.Substring(index_head, 100);
            ////定位token字符串尾
            //int index_last = InitToken.IndexOf('"');
            ////截取token
            //string Token = reslut.Substring(index_head, index_last);
            //string list = @"00001,00002,00003,00004,00005,00006,00007,00008,00009,00010,00011,00012,00014,00015,00016,00017,00018,00019,00020,00021,00022,00023,00024,00025,00026,00027,00028,00029,00030,00031,00032,00033,00034,00035,00036,00037,00038,00039,00040,00041,00042,00043,00045,00046,00047,00048,00050,00051,00052,00053,00055,00056,00057,00058,00059,00060,00061,00062,00063,00064,00065,00066,00067,00068,00069,00070,00071,00072,00073,00075,00076,00077,00078,00079,00080,00081,00082,00083,00084,00085,00086,00087,00088,00089,00090,00091,00092,00093,00094,00095,00096,00097,00098,00099,00100,00101,00102,00103,00104,00105,00106,00107,00108,00109,00110,00111,00112,00113,00114,00115,00116,00117,00118,00119,00120,00122,00123,00124,00125,00126,00127,00128,00129,00130,00131,00132,00133,00135,00136,00137,00138,00139,00141,00142,00143,00144,00145,00146,00147,00148,00149,00150,00151,00152,00153,00154,00155,00156,00157,00158,00159,00160,00161,00162,00163,00164,00165,00166,00167,00168,00169,00171,00172,00173,00174,00175,00176,00177,00178,00179,00180,00181,00182,00183,00184,00185,00186,00187,00188,00189,00190,00191,00193,00194,00195,00196,00197,00198,00199,00200,00201,00202,00204,00205,00206,00207,00208,00209,00210,00211,00212,00213,00214,00215,00216,00217,00218,00219,00220,00221,00222,00223,00224,00225,00226,00227,00228,00229,00230,00231,00232,00233,00234,00235,00236,00237,00238";

            //string[] strlist = list.Split(',');
            //TaskFactory fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(20));
            //Task[] tasks = new Task[strlist.Length];
            //for (int i = 0; i < strlist.Length; i++)
            //{
            //    tasks[i] = fac.StartNew(s =>
            //    {
            //        //Console.WriteLine(strlist[i] + "开始执行");
            //        string link = string.Format("https://www1.hkex.com.hk/hkexwidget/data/getequityquote?sym={0}&token={1}&lang=eng&qid=NULL&callback=0",
            //    Convert.ToInt32(strlist[i]), Token);
            //        //从港交所接口获取数据
            //        var hp1 = new HttpWebRequestTest.HttpRequestClient();
            //        string data = hp1.httpGet(link, HttpWebRequestTest.HttpRequestClient.defaultHeaders);
            //        Console.WriteLine(strlist[i] + "执行完成");
            //    }, i);

            //}


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
