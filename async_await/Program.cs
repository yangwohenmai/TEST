using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace async_await
{
    class Program
    {
        //创建计时器
        private static readonly Stopwatch Watch = new Stopwatch();
        public static bool control = true;
        private static void Main(string[] args)
        {
            //启动计时器
            Watch.Start();
            Task.Run(() => {
                while (control)
                {
                    Thread.Sleep(2000);
                    Console.WriteLine("."); 
                }
            });
            const string url1 = "http://www.cnblogs.com/";
            

            //Task<int> t1 = CountCharactersAsync(1, url1);
            //Task<int> t2 = CountCharactersAsync1(2, url1);
            //Task<int> t3 = CountCharactersAsync2(3, url1);
            int t4 = CountCharactersAsync3(4, url1);


            //const string url2 = "http://www.cnblogs.com/liqingwen/";
            //Task<int> t2 = CountCharactersAsync(2, url2);

            
            //t1.GetAwaiter();
            //t2.GetAwaiter();
            //int aa = t1.Result;
            //t1.GetAwaiter();

            //Task.WhenAll(t1).ContinueWith(o => Console.WriteLine("执行完成，进入ContinueWith,{0}mm", Watch.ElapsedMilliseconds));
            //Task.WaitAll(t1);


            Console.WriteLine("主进程开始休眠5s，{0}mm",Watch.ElapsedMilliseconds);
            Thread.Sleep(5000);
            Console.WriteLine("主进程休眠结束5s，{0}mm", Watch.ElapsedMilliseconds);



            //Console.WriteLine("主进程t1 获取等待的返回值：{0},{1}mm", t1.Result, Watch.ElapsedMilliseconds);
            Console.WriteLine("主进程下面开始等待await，{0}", Watch.ElapsedMilliseconds);
            //Console.WriteLine("主进程t3 获取等待的返回值：{0},{1}mm", t3.Result, Watch.ElapsedMilliseconds);
            //Console.WriteLine("主进程t4 获取等待的返回值：{0},{1}mm", t4, Watch.ElapsedMilliseconds);
            

            Console.WriteLine("主进程再次休眠，{0}", Watch.ElapsedMilliseconds);
            Thread.Sleep(5000);
            Console.WriteLine("主进程休眠结束，{0}", Watch.ElapsedMilliseconds);



            control = false;
            Console.Read();
        }

        /// <summary>
        /// 统计字符个数
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private static async Task<int> CountCharactersAsync(int id, string address)
        {
            Console.WriteLine("线程1开始调用 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
            
            Console.WriteLine("线程1开始调用await，{0}mm", Watch.ElapsedMilliseconds);
            var result = await Task.Run(()=>gosleep15());

            Console.WriteLine("线程1先睡1秒，{0}mm", Watch.ElapsedMilliseconds);
            Thread.Sleep(1000);
            Console.WriteLine("线程1先睡1秒，{0}mm", Watch.ElapsedMilliseconds);
            Console.WriteLine("线程1调用完成 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
            
            return result;
        }


        private static async Task<int> CountCharactersAsync1(int id, string address)
        {
            Console.WriteLine("线程2开始调用 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);

            Console.WriteLine("线程2先睡2秒，{0}mm", Watch.ElapsedMilliseconds);
            Thread.Sleep(2000);
            Console.WriteLine("线程2睡完2秒，{0}mm", Watch.ElapsedMilliseconds);
            Console.WriteLine("线程2开始await，{0}mm", Watch.ElapsedMilliseconds);
            var result = await Task.Run(() => gosleep20());

            Console.WriteLine("线程2调用完成 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);

            return result;
        }

        /// <summary>
        /// 如果只写async不写await，住
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private static async Task<int> CountCharactersAsync2(int id, string address)
        {
            Console.WriteLine("线程3开始调用 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);

            Console.WriteLine("线程3先睡3秒，{0}mm", Watch.ElapsedMilliseconds);
            Thread.Sleep(3000);
            Console.WriteLine("线程3睡完3秒，{0}mm", Watch.ElapsedMilliseconds);
            Console.WriteLine("线程3准备睡20秒，{0}mm", Watch.ElapsedMilliseconds);
            var result = Task.Run(() => gosleep20());
            int re = 666;
            Console.WriteLine("此时re等于{0},时间{1}", re, Watch.ElapsedMilliseconds);
            Console.WriteLine("线程3还有20s，我还能再睡会 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
            Thread.Sleep(3000);
            Console.WriteLine("线程3我又睡了3s， id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
            re = result.Result;
            Console.WriteLine("此时re等于{0},时间{1}", re, Watch.ElapsedMilliseconds);
            Console.WriteLine("线程3调用完成 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);

            return re;
        }

        private static async Task<int> CountCharactersAsync27(int id, string address)
        {
            
            var result = Task.Run(() => gosleep20());
            int re = result.Result;
            return result.Result;
        }

        /// <summary>
        /// 如果只写async不写await，住
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private static int CountCharactersAsync3(int id, string address)
        {
            Console.WriteLine("线程4开始调用 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);

            Console.WriteLine("线程4先睡3秒，{0}mm", Watch.ElapsedMilliseconds);
            Thread.Sleep(3000);
            Console.WriteLine("线程4睡完3秒，{0}mm", Watch.ElapsedMilliseconds);

            Console.WriteLine("线程4准备睡20秒，{0}mm", Watch.ElapsedMilliseconds);
            var result = Task.Run(() => gosleep20());
            int re = 666;
            Console.WriteLine("此时re等于{0},时间{1}", re, Watch.ElapsedMilliseconds);
            Console.WriteLine("线程4还有20s，我还能再睡会 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
            Thread.Sleep(3000);
            Console.WriteLine("线程4我又睡了3s， id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
            re = result.Result;
            Console.WriteLine("此时re等于{0},时间{1}", re, Watch.ElapsedMilliseconds);
            Console.WriteLine("线程4调用完成 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);

            return re;
        }


        public static int gosleep15()
        {
            Console.WriteLine("线程开始睡15秒，{0}mm", Watch.ElapsedMilliseconds);
            Thread.Sleep(15000);
            Console.WriteLine("线程睡完15秒，{0}mm", Watch.ElapsedMilliseconds);
            return 15;
        }

        public static int gosleep20()
        {
            Console.WriteLine("线程开始睡20秒，{0}mm", Watch.ElapsedMilliseconds);
            Thread.Sleep(20000);
            Console.WriteLine("线程睡完20秒，{0}mm", Watch.ElapsedMilliseconds);
            return 20;
        }


    }
}
