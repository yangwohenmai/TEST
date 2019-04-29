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
            Console.WriteLine("简单的线程池:");
            bool W2K = false;

            int MaxCount = 10;// 允许线程池中运行最多 10 个线程 

            ManualResetEvent eventX = new ManualResetEvent(false);// 新建 ManualResetEvent 对象并且初始化为无信号状态 
            Console.WriteLine("入队 {0} 项到线程池", MaxCount);

            Alpha oAlpha = new Alpha(MaxCount);// 注意初始化 oAlpha 对象的 eventX 属性 
            oAlpha.eventX = eventX;
            Console.WriteLine("Queue to Thread Pool 0");
            try
            {
                
                // 这里要用到 Windows 2000 以上版本才有的 API，所以可能出现 NotSupp ortException 异常 
                ThreadPool.QueueUserWorkItem(new WaitCallback(oAlpha.Beta), new SomeState(0));// 将工作项装入线程池 
                W2K = true;
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("These API's may fail when called on a non-Wind ows 2000 system.");
                W2K = false;
            }
            if (W2K) // 如果当前系统支持 ThreadPool 的方法. 
            {
                for (int iItem = 1; iItem < MaxCount*5; iItem++)
                {
                    // 插入队列元素 
                    Console.WriteLine("插入队元素进入缓冲池 {0}", iItem);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(oAlpha.Beta), new SomeState(iItem));
                }   
                Console.WriteLine("等待线程结束");

                eventX.WaitOne(Timeout.Infinite, true);// 等待事件的完成，即线程调用 ManualResetEvent.Set() 方法 

                // WaitOne() 方法使调用它的线程等待直到 eventX.Set() 方法被调用 
                Console.WriteLine("该线程已经结束 (Event fired)");
                Console.WriteLine();
                Console.WriteLine("Load across threads");
                foreach (object o in oAlpha.HashCount.Keys)
                {
                    Console.WriteLine("{0} {1}", o, oAlpha.HashCount[o]);
                }
            }
            Console.ReadLine();
        }
    }


    public class Alpha
    {
        public Hashtable HashCount;
        public ManualResetEvent eventX;
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
            // 输出当前线程的 hash 编码值和 Cookie 的值 
            Console.WriteLine(" {0} {1} :", Thread.CurrentThread.GetHashCode(), ((SomeState)state).Cookie);
            Console.WriteLine("HashCount.Count=={0}, Thread.CurrentThread.GetHash Code()=={1}", HashCount.Count,
                Thread.CurrentThread.GetHashCode());
            lock (HashCount)
            {
                // 如果当前的 Hash 表中没有当前线程的 Hash 值，则添加之 
                if (!HashCount.ContainsKey(Thread.CurrentThread.GetHashCode()))
                    HashCount.Add(Thread.CurrentThread.GetHashCode(), 0);
                HashCount[Thread.CurrentThread.GetHashCode()] = ((int)HashCount[Thread.CurrentThread.GetHashCode()]) + 1;
            }

            Thread.Sleep(2000);
            // Interlocked.Increment() 操作是一个原子操作，具体请看下面说明 
            Interlocked.Increment(ref iCount);
            if (iCount == iMaxCount)
            {
                Console.WriteLine();
                Console.WriteLine("Setting eventX ");
                eventX.Set();
            }
        }
    }
}
