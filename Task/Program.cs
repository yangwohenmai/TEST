using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasktest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Task传参和获取返回值 1（在子函数中对task传参和获取返回值）
            Task<int> task = CreateTask("Task 2");
            task.RunSynchronously(); //运行在主线程中，等同于直接运行： TaskMethod("Task 2");  
            int result = task.Result;
            Console.WriteLine("Result is: {0}", result);
            #endregion

            #region Task传参和获取返回值 2
            //Task<int> task = new Task<int>(() => TaskMethod("Task 1"));
            //task.Start();
            //int result = task.Result;
            //Console.WriteLine("Result is: {0}", result);  
            #endregion

            #region 在Task中可以直接写函数方法
            //Task<string> t11 = Task.Factory.StartNew(() =>
            //{
            //    int i = 1;
            //    for (; i < 10; i++)
            //    {
            //        i += i;
            //    }
            //    //throw new Exception("Task异常测试");//程序的异常会在Wait()方法中被捕获
            //    return i.ToString();
            //});
            //t11.Wait();
            //Console.WriteLine(t11.Result);
            //Console.ReadLine();
            #endregion

            #region 一个Task中可以创建，调用另一个Task
            //Task parent = new Task(ParentTask);
            //parent.Start();
            //Thread.Sleep(1000);
            //Console.WriteLine("父任务的状态：{0}", parent.Status);
            //Thread.Sleep(4000);
            //Console.WriteLine("父任务的状态：{0}", parent.Status);
            //Console.ReadLine();
            #endregion

            #region 创建和启动一个Task的四种方法
            ////使用TaskFactory创建一个任务
            //TaskFactory tf = new TaskFactory();
            //Task t1 = tf.StartNew(NewTask);
            ////使用Task类de Factory创建一个任务
            //Task t2 = Task.Factory.StartNew(NewTask);

            //Task t3 = new Task(NewTask);
            //t3.Start();
            //Task t4 = new Task(NewTask, TaskCreationOptions.PreferFairness);
            //t4.Start();
            //Thread.Sleep(1000);//因为任务是后台线程，所以我们这里阻塞主线程一秒钟来等待任务全部执行完成
            //Console.ReadLine();
            #endregion

            #region 一个Task的取消
            //CancellationTokenSource cts = new CancellationTokenSource();
            //Task t = new Task(() => LongRunTask(cts.Token));
            //t.Start();
            //Thread.Sleep(2000);
            //cts.Cancel();
            //Console.Read();
            #endregion

            #region ContinueWith可以再一个线程执行完之后自动开始执行下一个
            //Task task2 = new Task(() => { Thread.Sleep(5000); Console.WriteLine("Hello，"); Thread.Sleep(1000); });
            //task2.Start();
            //Task newTask = task2.ContinueWith(t22 => Console.WriteLine("World!"));
            //Console.Read();
            #endregion

            #region Task中可以创建子Task，Task可以直接定义函数
            //Task parant = new Task(() =>
            //{
            //    new Task(() => Console.WriteLine("Hello")).Start();
            //    new Task(() => Console.WriteLine(",")).Start();
            //    new Task(() => Console.WriteLine("World")).Start();
            //    new Task(() => Console.WriteLine("!")).Start();
            //    new Task(() =>
            //    {
            //        for (int i = 0; i < 10; i++)
            //        {
            //            Console.Write("{0}", i);
            //        }
            //    }).Start();
            //});
            //parant.Start();
            //Console.ReadLine();
            #endregion

            #region 批量创建公用同一个状态的任务
            //Task parent = new Task(() =>
            //{
            //    CancellationTokenSource cts = new CancellationTokenSource();
            //    TaskFactory tf1 = new TaskFactory(cts.Token);
            //    var childTask = new[] 
            //    { 
            //     tf1.StartNew(()=>ConcreteTask(cts.Token)), 
            //     tf1.StartNew(()=>ConcreteTask(cts.Token)), 
            //     tf1.StartNew(()=>ConcreteTask(cts.Token)) 
            //    };

            //    Thread.Sleep(2000);//此处睡眠等任务开始一定时间后才取消任务 
            //    cts.Cancel();
            //}
            //);

            //parent.Start();//开始执行任务 
            //Console.Read();
            #endregion

        }

        #region 传参和获取返回值1，2
        static Task<int> CreateTask(string name)
        {
            Task<int> TT = new Task<int>(() => TaskMethod(name));
            TT.Start();
            return new Task<int>(() => TaskMethod(name));
        }

        static int TaskMethod(string name)
        {
            Console.WriteLine("Task {0} 运行在线程id为{1}的线程上。是否是线程池中线程？:{2}",
            name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(2000);
            return 42;
        }
        #endregion

        #region 一个Task中可以创建，调用另一个Task
        public static void ParentTask()
        {
            Console.WriteLine("父任务{0}正在运行......", Task.CurrentId);
            Task child = new Task(ChildTask);
            child.Start();
            Console.WriteLine("父任务启动了子任务");
            Thread.Sleep(1000);
        }
        static void ChildTask()
        {
            Console.WriteLine("子任务{0}正在运行......", Task.CurrentId);
            Thread.Sleep(3000);
            Console.WriteLine("子任务执行完成。");
        }
        #endregion

        #region 创建和启动一个Task的四种方法
        //static void NewTask()
        //{
        //    Console.WriteLine("开始一个任务");
        //    Console.WriteLine("Task id:{0}", Task.CurrentId);
        //    Console.WriteLine("任务执行完成");
        //}
        #endregion

        #region 一个Task的取消
        //static void LongRunTask(CancellationToken token)
        //{
        //    //此处方法模拟一个耗时的工作 
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        if (!token.IsCancellationRequested)
        //        {
        //            Thread.Sleep(500);
        //            Console.Write(".");
        //        }
        //        else
        //        {
        //            Console.WriteLine("任务取消");
        //            break;
        //        }
        //    }
        //}
        #endregion

        #region 批量创建公用同一个状态的任务
        //static void ConcreteTask(CancellationToken token)
        //{
        //    while (true)
        //    {
        //        if (!token.IsCancellationRequested)
        //        {
        //            Thread.Sleep(500);
        //            Console.Write(".");
        //        }
        //        else
        //        {
        //            Console.WriteLine("任务取消");
        //            break;
        //        }
        //    }
        //}
        #endregion



    }
}
