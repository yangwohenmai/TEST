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

        static SemaphoreSlim semLim = new SemaphoreSlim(3); //3表示最多只能有三个线程同时访问



        public static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(SemaphoreTest).Start();
            }
            Console.Read();

            
         
        }


        static void SemaphoreTest()
        {
            semLim.Wait();
            Console.WriteLine("线程" + Thread.CurrentThread.ManagedThreadId.ToString() + "开始执行:"+DateTime.Now.ToString());
            Random rd = new Random();
            rd.Next(1, 3);
            Thread.Sleep(rd.Next(3, 55) *1000);
            Console.WriteLine("线程" + Thread.CurrentThread.ManagedThreadId.ToString() + "执行完毕:" + DateTime.Now.ToString());
            semLim.Release();
        }





    }





}
