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
        //static SemaphoreSlim semLim = new SemaphoreSlim(4); //3表示最多只能有三个线程同时访问
        public static void Main(string[] args)
        {
            //1.无参数线程
            for (int i = 0; i < 10; i++)
            {
                new Thread(aa).Start();
                //Thread t = new Thread(aa);
                //t.start();
            }


            //2.无参数线程
            for (int i = 0; i < 10; i++)
            {
                new Thread(() => aa()).Start();
            }


            //3.无参数线程
            for (int i = 0; i < 10; i++)
            {
                new Thread(new ThreadStart(aa));
            }


            //4.有参数线程
            for (int i = 0; i < 10; i++)
            {
                new Thread(() => SemaphoreTest(i)).Start();
            }


            //5.Action直接调用
            for (int i = 0; i < 10; i++)
            {
                new Thread(() => { Console.WriteLine("线程启动"); });
            }
        }

        /// <summary>
        /// 无参
        /// </summary>
        static void aa()
        {
        }

        /// <summary>
        /// 有参
        /// </summary>
        /// <param name="i"></param>
        static void SemaphoreTest(int i)
        {
            Console.WriteLine("线程" + i + "进入准备111111:" + DateTime.Now.ToString());
            //semLim.Wait();
            Console.WriteLine("线程" + i + "开始执行22222222:"+DateTime.Now.ToString());
            Thread.Sleep((2 + 2 * i) * 1000);
            Console.WriteLine("线程" + Thread.CurrentThread.ManagedThreadId.ToString() + "执行完毕33333333:" + DateTime.Now.ToString());
            //semLim.Release();
        }
    }
}
