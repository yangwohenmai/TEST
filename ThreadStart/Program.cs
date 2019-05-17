using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadStartTest
{
    class Program
    {
        //static SemaphoreSlim semLim = new SemaphoreSlim(4); //3表示最多只能有三个线程同时访问
        public static void Main(string[] args)
        {
            ParameterizedThreadStart p = m =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("参数为{0},{1},{2},当前线程为{3}", ((Tuple<string, int, bool>)m).Item1, ((Tuple<string, int, bool>)m).Item2, ((Tuple<string, int, bool>)m).Item3, Thread.CurrentThread.ManagedThreadId);
            };
            List<string> list = new List<string>();
            list.Add("aaa");
            //Tuple tp = new Tuple<string, int, string, int, bool>("1", 2, "3", 4, false);
            var tp = Tuple.Create<string, int, bool>("1", 2, false);
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(p);
                t.Start(tp);
            }
            Console.ReadLine();


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


            //ParameterizedThreadStart p = m =>
            //{
            //    Thread.Sleep(1000);
            //    Console.WriteLine("参数为{0},当前线程为{1}", ((List<string>)m)[0], Thread.CurrentThread.ManagedThreadId);
            //};
            //List<string> list = new List<string>();
            //list.Add("aaa");
            //for (int i = 0; i < 10; i++)
            //{
            //    Thread t = new Thread(p);
            //    t.Start(list);
            //}
            Console.ReadLine();


            #region 等待线列表中所有线程执行完毕
            List<Thread> tList = new List<Thread>();
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(() => SemaphoreTest(i));
                t.Start();
                //将已经启动的线程加入集合中
                tList.Add(t);
            }

            //线程启动后，通过这个方法等待列表中所有Thread执行完成，
            foreach (Thread t in tList)
            {
                //遍历列表中所有Thread，给其添加Join方法使其等待
                t.Join();
            }
            Console.WriteLine("等待结束");
            #endregion

            Console.ReadLine();


        }

        /// <summary>
        /// 无参
        /// </summary>
        static void aa()
        {
            Console.WriteLine("我没有参数");
        }

        /// <summary>
        /// 有参
        /// </summary>
        /// <param name="i"></param>
        static void SemaphoreTest(int i)
        {
            Console.WriteLine("线程" + i + "进入准备111111:" + DateTime.Now.ToString());
            //semLim.Wait();
            Console.WriteLine("线程" + i + "开始执行22222222:" + DateTime.Now.ToString());
            Thread.Sleep((2 + 2 * i) * 1000);
            Console.WriteLine("线程" + Thread.CurrentThread.ManagedThreadId.ToString() + "执行完毕33333333:" + DateTime.Now.ToString());
            //semLim.Release();
        }
    }
}
