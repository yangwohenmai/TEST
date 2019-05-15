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
        static SemaphoreSlim semLim = new SemaphoreSlim(4); //3表示最多只能有三个线程同时访问


        public static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(()=>SemaphoreTest(i)).Start();
            }
            Console.Read();

            //new Thread(new ThreadStart(aa));
            //new Thread(aa).Start();
            //new Thread(()=>aa()).Start();
        }

        static void aa()
        {
        }

        static void SemaphoreTest(int i)
        {
            Console.WriteLine("线程" + i + "进入准备111111:" + DateTime.Now.ToString());
            semLim.Wait();
            Console.WriteLine("线程" + i + "开始执行22222222:"+DateTime.Now.ToString());
            Thread.Sleep((2 + 2 * i) * 1000);
            Console.WriteLine("线程" + Thread.CurrentThread.ManagedThreadId.ToString() + "执行完毕33333333:" + DateTime.Now.ToString());
            semLim.Release();
        }
    }
}
