using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
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
  
              //两次调用 CountCharactersAsync 方法（异步下载某网站内容，并统计字符的个数）
              Task<int> t1 = CountCharactersAsync(1, url1);
              Task<int> t2 = CountCharactersAsync(2, url2);
  
              //三次调用 ExtraOperation 方法（主要是通过拼接字符串达到耗时操作）
              for (var i = 0; i < 3; i++)
              {
                  ExtraOperation(i + 1);
              }
  
              //控制台输出
              Console.WriteLine("{0} 的字符个数：{1}", url1, t1.Result);
              Console.WriteLine("{0} 的字符个数：{1}", url2, t2.Result);
  
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
              var wc = new WebClient();
              Console.WriteLine("开始调用 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
  
              var result = await wc.DownloadStringTaskAsync(address);
              Console.WriteLine("调用完成 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
  
              return result.Length;
          }
  
          /// <summary>
          /// 额外操作
          /// </summary>
          /// <param name="id"></param>
          private static void ExtraOperation(int id)
          {
              //这里是通过拼接字符串进行一些相对耗时的操作
              var s = "";
  
              for (var i = 0; i < 6000; i++)
              {
                  s += i;
              }
  
              Console.WriteLine("id = {0} 的 ExtraOperation 方法完成：{1} ms",id,Watch.ElapsedMilliseconds);
          }
         //创建计时器
         //private static readonly Stopwatch Watch = new Stopwatch();
 
         //private static void Main(string[] args)
         //{
         //    //启动计时器
         //    Watch.Start();
 
         //    const string url1 = "http://www.cnblogs.com/";
         //    const string url2 = "http://www.cnblogs.com/liqingwen/";
 
         //    //两次调用 CountCharacters 方法（下载某网站内容，并统计字符的个数）
         //    var result1 = CountCharacters(1, url1);
         //    var result2 = CountCharacters(2, url2);
 
         //    //三次调用 ExtraOperation 方法（主要是通过拼接字符串达到耗时操作）
         //    for (var i = 0; i < 3; i++)
         //    {
         //        ExtraOperation(i + 1);
         //    }
 
         //    //控制台输出
         //    Console.WriteLine("{0} 的字符个数：{1}", url1, result1);
         //    Console.WriteLine("{0} 的字符个数：{1}", url2, result2);
 
         //    Console.Read();
         //}
 
         ///// <summary>
         ///// 统计字符个数
         ///// </summary>
         ///// <param name="id"></param>
         ///// <param name="address"></param>
         ///// <returns></returns>
         //private static int CountCharacters(int id, string address)
         //{
         //    var wc = new WebClient();
         //    Console.WriteLine("开始调用 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
 
         //    var result = wc.DownloadString(address);
         //    Console.WriteLine("调用完成 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds);
 
         //    return result.Length;
         //}
 
         ///// <summary>
         ///// 额外操作
         ///// </summary>
         ///// <param name="id"></param>
         //private static void ExtraOperation(int id)
         //{
         //    //这里是通过拼接字符串进行一些相对耗时的操作
         //    var s = "";
 
         //    for (var i = 0; i < 6000; i++)
         //    {
         //        s += i;
         //    }
 
         //    Console.WriteLine("id = {0} 的 ExtraOperation 方法完成：{1} ms",id,Watch.ElapsedMilliseconds);
         //}
    }
}
