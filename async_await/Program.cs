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
  
        private static void Main(string[] args)
        {
            //启动计时器
            Watch.Start();
  
            const string url1 = "http://www.cnblogs.com/";
            const string url2 = "http://www.cnblogs.com/liqingwen/";
  

            Task<int> t1 = CountCharactersAsync(1, url1);
            Task<int> t2 = CountCharactersAsync1(11, url1);
            //Task<int> t2 = CountCharactersAsync(2, url2);



            Task.WaitAll(t1, t2);
            Task.WhenAll(t1);
  
            for (var i = 0; i < 3; i++)
            {
                ExtraOperation(i + 1);
            }

            Console.WriteLine("开始休眠5s，{0}",Watch.ElapsedMilliseconds);
            Thread.Sleep(5000);
            Console.WriteLine("休眠结束5s，{0}", Watch.ElapsedMilliseconds);

            Console.WriteLine("来个循环，{0}", Watch.ElapsedMilliseconds);
            for (var i = 0; i < 3; i++)
            {
                ExtraOperation(i + 1);
            }

            
            Console.WriteLine("等待await，{0}", Watch.ElapsedMilliseconds);
            Console.WriteLine("{0} 获取等待的返回值：{1}", url1, t1.Result);

            Console.WriteLine("再次休眠，{0}", Watch.ElapsedMilliseconds);
            Thread.Sleep(5000);
            Console.WriteLine("休眠结束，{0}", Watch.ElapsedMilliseconds);
            Console.WriteLine("来个循环，{0}", Watch.ElapsedMilliseconds);


            for (var i = 0; i < 3; i++)
            {
                ExtraOperation(i + 1);
            }
  
            Console.Read();
        }

        /// <summary>
        /// 统计字符个数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private static async Task<int> CountCharactersAsync(int id, string address)
        {
            Console.WriteLine("开始调用 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
            for (var i = 0; i < 3; i++)
            {
                ExtraOperation(i + 10);
            }
            var result = await Task.Run(()=>gosleep15());

            Console.WriteLine("调用完成 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
            
            return result;
        }


        private static async Task<int> CountCharactersAsync1(int id, string address)
        {
            Console.WriteLine("开始调用 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
            for (var i = 0; i < 3; i++)
            {
                ExtraOperation(i + 10);
            }
            var result = await Task.Run(() => gosleep20());

            Console.WriteLine("调用完成 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);

            return result;
        }


        public static int gosleep15()
        {
            Thread.Sleep(15000);
            return 1;
        }

        public static int gosleep20()
        {
            Thread.Sleep(20000);
            return 1;
        }


        /// <summary>
        /// 额外操作
        /// </summary>
        /// <param name="id"></param>
        private static void ExtraOperation(int id)
        {
            var s = "";
  
            for (var i = 0; i < 6000; i++)
            {
                s += i;
            }
  
            Console.WriteLine("id = {0}  循环测试 方法完成：{1} ms",id,Watch.ElapsedMilliseconds);
        }


    }
}
