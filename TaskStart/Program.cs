using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskStart
{
    class Program
    {
        static void Main(string[] args)
        {
            //有参数，没有返回值
            (new Task(() => Test.ExecuteFun())).Start();

            Task t1_2 = new Task(() => Test.ExecuteFun());
            t1_2.Start();
            Task t1_3 = new Task(() => Test.taskPara("1","2"));
            t1_3.Start();


            //有返回值
            Task<string> t0_1 = new Task<string>(() => Test.taskPara("1", "2"));
            t0_1.Start();


            //TaskFactory的调用方式
            TaskFactory tf = new TaskFactory();
            Task t2_1 = tf.StartNew(Test.ExecuteFun);
            Task t2_2 = new TaskFactory().StartNew(() => Test.ExecuteFun());
            Task t2_3 = new TaskFactory().StartNew(() => Test.taskPara("1", "2"));


            //Task.Factory.StartNew的调用方式
            Task t3_1 = Task.Factory.StartNew(Test.ExecuteFun);
            Task t3_2 = Task.Factory.StartNew(() => Test.ExecuteFun());
            Task t3_3 = Task.Factory.StartNew(() => Test.taskPara("1", "2"));
            Task t3_4 = Task.Factory.StartNew(() => { Test.taskPara("1", "2"); });
            Task t3_5 = Task.Factory.StartNew(() => { Console.WriteLine("hehe");return "3|4"; });


            //t4_1的Task.Run方法有重载，加上Action指定调用函数，否则报错有二义性
            Task t4_1 = Task.Run((Action)Test.ExecuteFun);
            Task t4_2 = Task.Run(()=>Test.ExecuteFun());
            Task t4_3 = Task.Run(() => Test.taskPara("1", "2"));
            Task t4_4 = Task.Run(() => { Test.taskPara("1", "2"); });
            Task t4_5 = Task.Run(() => { Console.WriteLine("hehe"); });


            //Task是接收不到返回值的，只有Task<TResult>才能接收到返回值（即：t1是没有Result属性的）
            Task<string> t5_1 = Task.Factory.StartNew<string>(() => Test.taskPara("1", "2"));
            Console.WriteLine(t5_1.Result); 
            Task<string> t5_2 = Task.Factory.StartNew<string>(() => { Console.WriteLine("1|2"); Thread.Sleep(2000); return "3|4"; });
            Console.WriteLine(t5_2.Result);

            
            //有返回值
            Task<string> t6_1 = Task.Run<string>(() => Test.taskPara("1", "2"));
            Console.WriteLine(t6_1.Result);
            Task<string> t6_2 = Task.Run<string>(() => { Console.WriteLine("1|2"); Thread.Sleep(2000); return "3|4"; });
            Console.WriteLine(t6_2.Result);

            
            //表示在执行完t6_1这个任务后在执行Task.Run<int>(()=>Test.taskPara()),[ContinueWith其实就相当于回调]
            Task<Task<string>> t6_3 = t6_1.ContinueWith(r => Task.Run<string>(() => Test.taskPara("5", "6")));
            //t6_4和t6_5两种获取的线程是一样的，但是t6_5这种方法获取返回值速度更快
            Task<string> t6_4 = t6_1.ContinueWith(r => Task.Run<string>(() => Test.taskPara("5", "6"))).Result;
            Task t6_5 = t6_1.ContinueWith(r => Task.Run<string>(() => Test.taskPara("5", "6"))).Result;
            string t6_6 = t6_1.ContinueWith(r => Task.Run<string>(() => Test.taskPara("5", "6"))).Result.Result; //得到“23”
            Console.WriteLine(t6_6);


            //RunSynchronously以主进程的方式运行线程的方法
            Task<string> t7 = new Task<string>(() => Test.taskPara("1", "2"));
            t7.Start();
            //运行在主线程中，等同于直接运行TaskMethod("Task 2"),不再是异步线程
            t7.RunSynchronously();
            string result = t7.Result;


            //定义线程调度的策略
            Task t8 = new Task(Test.ExecuteFun, TaskCreationOptions.PreferFairness);
            t8.Start();

            ////Task.Run的跟Task.Factory.StarNew和new Task相差不多，不同的是前两种是放进线程池立即执行，而Task.Run则是等线程池空闲后再执行。



            System.Console.ReadLine();
            tasktest1();


            

        }


        public static void tasktest1()
        {
            //特别注意，Task是接收不到返回值的，只有Task<TResult>才能接收到返回值（即：t1是没有Result属性的）
            //Task t1 = new Task(() => Console.WriteLine("启动了"));


            //启动一个任务的方法一：（需要调用Start方法启动任务）
            Task t1 = new Task(() => Test.ExecuteFun());
            //t1.RunSynchronously();//运行在主线程中，等同于直接运行:Test.ExecuteFun()因此，该Task不是运行在线程池中，而是运行在主线程中
            t1.Start(); //启动 Task，并将它安排到当前的 TaskScheduler 中执行。

            //有返回值的Task
            Task<string> t2 = Task.Factory.StartNew<string>(() => Test.GetUserName());
            //这里不能再次Start()了，因为StartNew就已经Start()了
            var userNameA = t2.Result; //得到“刘德华”
            Console.WriteLine(userNameA);


            //启动一个任务的方法二：(就会立即启动任务)
            Task t21 = Task.Factory.StartNew(() => Test.GetUserName());
            //这里不能再次Start()了，因为StartNew就已经Start()了
            var userNameA1 = t21; //得到“刘德华”
            Console.WriteLine(userNameA1);

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


    /// <summary>
    /// 测试类
    /// </summary>
    public class Test
    {
        public static void ExecuteFun()
        {
            Console.WriteLine("哈哈");
            //return "A";
        }
        public static int GetAge()
        {
            return 23;
        }
        public static string GetUserName()
        {
            return "刘德华";
        }


        public static string taskPara(string par1, string par2)
        {
            string a = par1 + "|" + par2;
            Console.WriteLine(a);
            return a;
        }

    }
}
