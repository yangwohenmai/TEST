using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace MoreThread  
{
    class Program
    {
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 
        private  LinkedList<Task> _taskslist = new LinkedList<Task>(); // protected by lock(_tasks) 
        static int _maxDegreeOfParallelism = 0;
        static int _delegatesQueuedOrRunning = 0;
        static int count = 0;
        static EventWaitHandle _tollStation = new ManualResetEvent(false);//改为ManualResetEvent,车闸默认关闭
        public static void Main(string[] args)
        {
            //Action A = new Action();

            int MaxThread = 0;
            Console.WriteLine("Main thread: Queuing an asynchronous operation.");
            AutoResetEvent asyncOpIsDone = new AutoResetEvent(false);
            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(t=>()), i);
            }
            //ThreadPool.QueueUserWorkItem(new WaitCallback(MyAsyncOperation), 1);
            //ThreadPool.QueueUserWorkItem(new WaitCallback(MyAsyncOperation), 2);
            Console.WriteLine("Main thread: Performing other operations.");

            
            Console.WriteLine("Main thread: Waiting for asynchronous operation to complete.");
            //asyncOpIsDone.WaitOne();

            Console.ReadLine();










            #region
            Console.WriteLine("简单的线程池:");
            bool W2K = false;

            int MaxCount = 5;// 允许线程池中运行最多 10 个线程 

            AutoResetEvent eventX = new AutoResetEvent(true);// 新建 ManualResetEvent 对象并且初始化为无信号状态 
            Console.WriteLine("入队 {0} 项到线程池", MaxCount);

            Alpha oAlpha = new Alpha(MaxCount);// 注意初始化 oAlpha 对象的 eventX 属性 
            oAlpha.eventX = eventX;
            Console.WriteLine("Queue to Thread Pool 0");
            //线插入一条试试，判断系统是否支持线程池
            //try
            //{
                
            //    // 这里要用到 Windows 2000 以上版本才有的 API，所以可能出现 NotSupp ortException 异常 
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(oAlpha.Beta), new SomeState(0));// 将工作项装入线程池 
            //    W2K = true;
            //}
            //catch (NotSupportedException)
            //{
            //    Console.WriteLine("These API's may fail when called on a non-Wind ows 2000 system.");
            //    W2K = false;
            //}
            if (true) // 如果当前系统支持 ThreadPool 的方法. 
            {
                for (int iItem = 0; iItem < MaxCount * 10; iItem++)
                {
                    // 插入队列元素 
                    Console.WriteLine("插入队元素进入缓冲池 {0}", iItem);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(oAlpha.Beta), new SomeState(iItem+100000));
                }   
                Console.WriteLine("等待线程结束");

                //eventX.WaitOne(Timeout.Infinite, true);// 等待事件的完成，即线程调用 ManualResetEvent.Set() 方法 

                // WaitOne() 方法使调用它的线程等待直到 eventX.Set() 方法被调用 
                Console.WriteLine("该线程已经结束 (Event fired)");
                Console.WriteLine();
                Console.WriteLine("Load across threads");
                //foreach (object o in oAlpha.HashCount.Keys)
                //{
                //    Console.WriteLine("{0} {1}", o, oAlpha.HashCount[o]);
                //}
            }
            Console.ReadLine();
            #endregion
        }



        private static ILog log = new LogHelper("SeqService"); // 日志
        private static Dictionary<int, DateTime> dic_thread = new Dictionary<int, DateTime>(); // 线程列表

        private static long Num = 0; // 线程个数
        private static object lock_Num = 0;  // 共享数据-锁

        /// <summary>
        /// 在线申请流水号
        /// </summary>
        /// <returns></returns>
        //[WebGet(UriTemplate = "GetSeqNum/Json", ResponseFormat = WebMessageFormat.Json)]
        public string GetSeqNumber()
        {
            lock (lock_Num)
            {
                Num++;
                int id_thread = System.Threading.Thread.CurrentThread.ManagedThreadId;
                DateTime now = DateTime.Now;
                if (!dic_thread.TryGetValue(id_thread, out now))
                {
                    dic_thread.Add(id_thread, DateTime.Now);
                }

            }
            string ret = DateTime.Now.ToString("yyyyMMdd") + Num.ToString(new string('0', 9));

            log.Info(string.Format("{0}, Thread={1}/{2}", ret, System.Threading.Thread.CurrentThread.ManagedThreadId, dic_thread.Count));
            return ret;
        }






        /// <summary>
        /// 多线程调用WCF
        /// </summary>
        /// <param name="select">调用WCF的方式，1=Restful，2=Tcp</param>
        /// <param name="num"></param>
        static void DoTest_MultiThread(string select, long num)
        {
            int n_max_thread = 10; // 设置并行最大为10个线程
            int n_total_thread = 0; // 用来控制：主程序的结束执行，当所有任务线程执行完毕

            ILog log_add = new LogHelper("Add_Thread");
            ILog log_del = new LogHelper("Del_Thread");
            ILog log_wait = new LogHelper("Wait_Thread");
            ILog log_set = new LogHelper("Set_Thread");
            ILog log_for = new LogHelper("For_Thread");

            Console.Title = string.Format("调用WCF的方式 => {0}, 调用次数=> {1}"
                , select == "1" ? "Restful" : "Socket"
                , num);

            List<int> list_Thread = new List<int>();

            System.Threading.AutoResetEvent wait_sync = new System.Threading.AutoResetEvent(false); // 用来控制：并发最大个数线程=n_max_thread
            System.Threading.AutoResetEvent wait_main = new System.Threading.AutoResetEvent(false); // 用来控制：主程序的结束执行，当所有任务线程执行完毕

            DateTime date_step = DateTime.Now;
            for (long i = 0; i < num; i++)
            {
                Num_Query_Static++;
                if (i > 0 && (i + 1 - 1) % n_max_thread == 0) // -1 表示第max个线程尚未开始
                {
                    //log_wait.Info(string.Format("thread n= {0},for i= {1}", dic_Thread.Count, i + 1));
                    wait_sync.WaitOne(); // 每次并发10个线程，等待处理完毕后，在发送下一次并发线程
                }
                log_for.Info(string.Format("thread n= {0},for i= {1}", list_Thread.Count, i + 1));

                System.Threading.ThreadPool.QueueUserWorkItem
                    ((data) =>
                    {
                        int id = System.Threading.Thread.CurrentThread.ManagedThreadId;
                        System.Threading.Monitor.Enter(list_Thread);
                        list_Thread.Add(id);
                        System.Threading.Monitor.Exit(list_Thread);

                        log_add.Info(string.Format("id={0}, count={1}", id, list_Thread.Count)); // 日志

                if (select == "1") // Restful方式调用
                {
                    Query_Htty();
                }
                else
                {
                    Query_Socket();
                }

                n_total_thread += 1;
                if (list_Thread.Count == (n_max_thread) || n_total_thread == num)
                {
                    list_Thread.Clear();
            //log_set.Info(string.Format("thread n= {0},for i= {1}", dic_Thread.Count, i + 1));
            //wait_sync.Set(); 
                if (n_total_thread != num)
                {
                    wait_sync.Set(); // 任务线程，继续执行
                }
                else
                {
                    wait_main.Set(); // 主程序线程，继续执行
                }
                }
                }, list_Thread);
            }

            wait_main.WaitOne();

            Console.WriteLine(string.Format("总测试{0}次，总耗时{1}, 平均耗时{2}"
                , num
                , (DateTime.Now - date_step).ToString()
                , (DateTime.Now - date_step).TotalMilliseconds / num));

            Query_Thread();
        }


        static void MyAsyncOperation(Object state)
        {
            Console.WriteLine("启用一个线程" + state.ToString());

            //
            Thread.Sleep(5000);    // Sleep for 5 seconds to simulate doing work

            // Signal that the async operation is now complete.
            Console.WriteLine(state.ToString());
            // 同步操作已经完成的操作
            //((AutoResetEvent)state).Set();
            Console.WriteLine("结束一个线程" + state.ToString());

        }
    }

















    public class Alpha
    {
        public Hashtable HashCount;
        public AutoResetEvent eventX;
        public static int iCount = 0;
        public static int iMaxCount = 0;
        public Alpha(int MaxCount)
        {
            HashCount = new Hashtable(MaxCount);
            iMaxCount = MaxCount;
        }

        /// <summary>
        /// 线程池里的线程将调用 Beta()方法
        /// </summary>
        /// <param name="state"></param> 
        public void Beta(Object state)
        {
            eventX.WaitOne();
            eventX.Reset();
            // 输出当前线程的 hash 编码值和 Cookie 的值 
            Console.WriteLine(" {0} {1} :", Thread.CurrentThread.GetHashCode(), ((SomeState)state).Cookie);
            Console.WriteLine("HashCount.Count=={0}, Thread.CurrentThread.GetHash Code()=={1}", HashCount.Count, Thread.CurrentThread.GetHashCode());
            //lock (HashCount)
            //{
            //    // 如果当前的 Hash 表中没有当前线程的 Hash 值，则添加之 
            //    if (!HashCount.ContainsKey(Thread.CurrentThread.GetHashCode()))
            //        HashCount.Add(Thread.CurrentThread.GetHashCode(), 0);
            //    HashCount[Thread.CurrentThread.GetHashCode()] = ((int)HashCount[Thread.CurrentThread.GetHashCode()]) + 1;
            //}
            
            Thread.Sleep(1000);
            // Interlocked.Increment() 操作是一个原子操作，具体请看下面说明 
            Interlocked.Increment(ref iCount);
            if (iCount == iMaxCount)
            {
                Console.WriteLine();
                Console.WriteLine("Setting eventX ");
                eventX.Set();
            }
            //eventX.Set();
        }
    }
}
