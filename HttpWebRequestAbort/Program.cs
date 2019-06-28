using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpWebRequestAbort
{
    class Program
    {
        static void Main(string[] args)
        {
            WebRequest();
            //IoThread();
            Console.ReadLine();
        }


        public static void WebRequest()
        {
            WebRequest request1 = HttpWebRequest.Create("https://blog.csdn.net/");
            request1.BeginGetResponse(ar =>
            {
                var response1 = request1.EndGetResponse(ar);
                Console.WriteLine(DateTime.Now + ": Response Get1");

            }, null);

            WebRequest request2 = HttpWebRequest.Create("https://blog.csdn.net/");
            request2.BeginGetResponse(ar =>
            {
                var response2 = request2.EndGetResponse(ar);
                Console.WriteLine(DateTime.Now + ": Response Get2");

            }, null);

            //不能立刻终止WebRequest，要等待WebRequest执行完成之后终止
            Thread.Sleep(2000);
            request1.Abort();

            WebRequest request3 = HttpWebRequest.Create("https://blog.csdn.net/");
            request3.BeginGetResponse(ar =>
            {
                var response3 = request3.EndGetResponse(ar);
                Console.WriteLine(DateTime.Now + ": Response Get3");

            }, null);

        }



        /// <summary>
        /// WebRequest请求会有线程数量限制，测试时发现对同一个网站发送http请求
        /// 只有两次有返回值，其余的请求都不再返回数据
        /// 只有使用Abort释放这个request后，才能继续向这个网站发送WebRequest请求
        /// </summary>
        public static void IoThread()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            WebRequest request22 = HttpWebRequest.Create("https://blog.csdn.net/");
            request22.BeginGetResponse(ar =>
            {
                var response22 = request22.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get22");

            }, null);

            WebRequest request11 = HttpWebRequest.Create("https://blog.csdn.net/");
            request11.BeginGetResponse(ar =>
            {
                var response11 = request11.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get11");

            }, null);

            WebRequest request00 = HttpWebRequest.Create("https://blog.csdn.net/");
            request00.BeginGetResponse(ar =>
            {
                var response00 = request00.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get00");

            }, null);

            WebRequest request44 = HttpWebRequest.Create("https://blog.csdn.net/");
            request44.BeginGetResponse(ar =>
            {
                var response44 = request44.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get00");

            }, null);

            WebRequest request = HttpWebRequest.Create("http://www.cnblogs.com/");
            request.BeginGetResponse(ar =>
            {
                var response = request.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get0");

            }, null);

            WebRequest request1 = HttpWebRequest.Create("https://www.baidu.com/");
            request1.BeginGetResponse(ar =>
            {
                var response1 = request1.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get1");

            }, null);


            WebRequest request2 = HttpWebRequest.Create("http://www.cnblogs.com/");
            request2.BeginGetResponse(ar =>
            {
                var response2 = request2.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get2");

            }, null);


            WebRequest request3 = HttpWebRequest.Create("https://www.baidu.com/");
            request3.BeginGetResponse(ar =>
            {
                var response3 = request3.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get3");

            }, null);
            //释放WebRequest
            Thread.Sleep(2000);
            request3.Abort();


            WebRequest request4 = HttpWebRequest.Create("http://www.cnblogs.com/");
            request4.BeginGetResponse(ar =>
            {
                var response4 = request4.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get4");

            }, null);
            //释放WebRequest
            Thread.Sleep(2000);
            request2.Abort();

            WebRequest request5 = HttpWebRequest.Create("https://www.baidu.com/");
            request5.BeginGetResponse(ar =>
            {
                var response5 = request5.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get5");

            }, null);
            //释放WebRequest
            Thread.Sleep(2000);
            request1.Abort();

            WebRequest request6 = HttpWebRequest.Create("https://www.baidu.com/");
            request6.BeginGetResponse(ar =>
            {
                var response6 = request6.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get6");

            }, null);

            WebRequest request7 = HttpWebRequest.Create("https://www.baidu.com/");
            request7.BeginGetResponse(ar =>
            {
                var response7 = request7.EndGetResponse(ar);
                Console.WriteLine(watch.Elapsed + ": Response Get7");

            }, null);
        }
    }
}
