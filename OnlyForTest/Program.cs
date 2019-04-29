using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlyForTest
{
    class Program
    {
        static void Main(string[] args)
        {

            tasktest2();

            

        }

        #region Task测试2
        public static void tasktest2()
        {
            //通知 System.Threading.CancellationToken，告知其应被取消。
            var TokenSource = new CancellationTokenSource();

            //获取与此 System.Threading.CancellationTokenSource 关联的 System.Threading.CancellationToken。
            var Token = TokenSource.Token;

            var task = Task.Factory.StartNew(() => test2(Token), Token);

            //在这里还可以给token注册了一个方法，输出一条信息  用户输入0后，执行tokenSource.Cancel方法取消任务。这个取消任务执行后，会执行这个代码.
            Token.Register(() => Console.WriteLine("取消"));

            //等待用户输入
            var input = Console.ReadLine();

            if (Convert.ToInt32(input) == 0)
            {
                //如果输入了0，则取消这个任务;
                //一旦cancel被调用。task将会抛出OperationCanceledException来中断此任务的执行，最后将当前task的Status的IsCanceled属性设为true
                TokenSource.Cancel();
            }
            Console.ReadLine();
        }

        public static void test2(CancellationToken Token)
        {
            int count = 0;
            for (var i = 1; i < 1000; i++)
            {
                Console.WriteLine("i:" + i);
                Thread.Sleep(1000);
                //判断是否取消任务，取消为true；不取消为false
                if (Token.IsCancellationRequested)
                {
                    count++;
                    Console.WriteLine("取消任务成功！"+count);
                    if (count == 5)
                    {
                        return;
                    }

                }
            }
        }
        #endregion


        public static void tasktest1()
        {
            //特别注意，Task是接收不到返回值的，只有Task<TResult>才能接收到返回值（即：t1是没有Result属性的）
            //Task t1 = new Task(() => Console.WriteLine("启动了"));


            //启动一个任务的方法一：（需要调用Start方法启动任务）
            Task t1 = new Task(() => Test.ExecuteFun());
            //t1.RunSynchronously();//运行在主线程中，等同于直接运行:Test.ExecuteFun()因此，该Task不是运行在线程池中，而是运行在主线程中
            t1.Start(); //启动 Task，并将它安排到当前的 TaskScheduler 中执行。

            //启动一个任务的方法二：(就会立即启动任务)
            Task<string> t2 = Task.Factory.StartNew<string>(() => Test.GetUserName());
            //这里不能再次Start()了，因为StartNew就已经Start()了
            var userNameA = t2.Result; //得到“刘德华”
            Console.WriteLine(userNameA);

            //Task.Run的跟Task.Factory.StarNew和new Task相差不多，不同的是前两种是放进线程池立即执行，而Task.Run则是等线程池空闲后再执行。

            //启动一个任务的方法三：（就会立即启动任务）
            Task<string> t3 = Task.Run<string>(() => Test.GetUserName());
            //Task.Run方法是Task类中的静态方法，接受的参数是委托。返回值是为该Task对象。
            //这种方式是直接运行了Task,不像第一种方法那样还需要Start();			
            var userNameB = t3.Result; //得到“刘德华”
            Console.WriteLine(userNameB);


            //----------------------------------------------------//



            //Task类型公开了多个ContinueWith的重载。当另外一个task完成的时候，该方法会创建新的将被调度的task。该重载接受CancellationToken，TaskCreationOptions,和TaskScheduler,这些都对task的调度和执行提供了细粒度的控制。
            //TaskFactory类提供了ContinueWhenAll 和ContinueWhenAny方法。当提供的一系列的tasks中的所有或任何一个完成时，这些方法会创建一个即将被调度的新的task。有了ContinueWith,就有了对于调度的控制和任务的执行的支持。

            //一下表达式：表示在执行完t3这个任务后在执行Task.Run<int>(()=>Test.GetAge())这个任务【ContinueWith其实就相当于回调】
            var userAge = t3.ContinueWith(r => Task.Run<int>(() => Test.GetAge())).Result.Result; //得到“23”

            //return View();
        }



        #region Task测试1
        public static void tasktest()
        {
            //Created：表示默认初始化任务，但是我们发现“工厂创建的”实例直接跳过。
            //WaitingToRun: 这种状态表示等待任务调度器分配线程给任务执行。
            //RanToCompletion：任务执行完毕。

            Task t1 = new Task(() => dowork1());
            Console.WriteLine(t1.Status);//输出：Created
            t1.Start();
            Console.WriteLine(t1.Status);//输出：WaitingToRun

            Task t2 = Task.Factory.StartNew(() => dowork2());
            Console.WriteLine(t2.Status); //输出：WaitingToRun

            Task t3 = Task.Run(() => dowork3());
            Console.WriteLine(t3.Status);//输出：WaitingToRun

            t3.ContinueWith(r => Console.Write(t3.Status));//输出：RanToCompletion


            Console.ReadLine();
        }

        public static void dowork1()
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine("111");
            }
                
        }

        public static void dowork2()
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine("222");
            }

        }

        public static void dowork3()
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine("333");
            }

        }

        #endregion



    }

    
    public class Test
    {
        public static void ExecuteFun()
        {
            Console.WriteLine("哈哈");
        }
        public static int GetAge()
        {
            return 23;
        }
        public static string GetUserName()
        {
            return "刘德华";
        }
    }
}
