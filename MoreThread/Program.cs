using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace MoreThread
{

    /// <summary>
    /// 这是用来保存信息的数据结构，将作为参数被传递
    /// </summary>
    public class SomeState
    {
        public int Cookie;
        public SomeState(int iCookie)
        {
            Cookie = iCookie;
        }
    }
    class Program
    {
        public static void Main(string[] args)
        {



            Console.WriteLine("Main thread: Queuing an asynchronous operation.");
            AutoResetEvent asyncOpIsDone = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(MyAsyncOperation), 1);
            ThreadPool.QueueUserWorkItem(new WaitCallback(MyAsyncOperation), 2);
            Console.WriteLine("Main thread: Performing other operations.");

            
            Console.WriteLine("Main thread: Waiting for asynchronous operation to complete.");
            //asyncOpIsDone.WaitOne();

            Console.ReadLine();











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
