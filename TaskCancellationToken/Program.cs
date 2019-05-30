using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCancellationToken
{
    class Program
    {
        //声明CancellationTokenSource对象
        static CancellationTokenSource c1 = new CancellationTokenSource();
        static CancellationTokenSource c2 = new CancellationTokenSource();
        static CancellationTokenSource c3 = new CancellationTokenSource();

        //使用多个CancellationTokenSource进行复合管理
        static CancellationTokenSource compositeCancel = CancellationTokenSource.CreateLinkedTokenSource(c1.Token, c2.Token, c3.Token);

        static void Main(string[] args)
        {
            // 取消任意一个任务，则所有任务都取消
            //CancellationTokenAllTest();
            // 取消单个任务
            //CancellationTokenTest();
        }


        #region 取消任意一个任务，所有的任务都取消
        /// <summary>
        /// 取消任意一个任务，则所有任务都取消
        /// </summary>
        public static void CancellationTokenAllTest()
        {
            var task = Task.Factory.StartNew(() =>
            {
                for (var i = 1; i < 1000; i++)
                {
                    Console.WriteLine("Hello World!");
                    Thread.Sleep(1000);
                    //判断是否取消任务，取消为true；不取消为false
                    if (compositeCancel.IsCancellationRequested)
                    {
                        Console.Write("取消任务成功！");
                        return;
                    }
                }
            }, compositeCancel.Token);


            //等待用户输入
            var input = Console.ReadLine();

            //如果输入了0，则取消c1这个任务，c1取消后，符合管理compositeCancel的状态都取消
            if (Convert.ToInt32(input) == 0)
            {
                //任意一个 CancellationTokenSource 取消任务，那么所有任务都会被取消
                c1.Cancel();
            }
        }
        #endregion




        #region 取消单个任务
        /// <summary>
        /// 
        /// </summary>
        public static void CancellationTokenTest()
        {
            //通知 System.Threading.CancellationToken，告知其应被取消。
            CancellationTokenSource TokenSource = new CancellationTokenSource();

            var task = Task.Factory.StartNew(() => DoWork(TokenSource.Token), TokenSource.Token);

            //在这里还可以给token注册了一个方法，输出一条信息  用户输入0后，执行tokenSource.Cancel方法取消任务。这个取消任务执行后，会执行这个代码.
            TokenSource.Token.Register(() => 
            {
                Console.WriteLine("取消");
                Console.WriteLine("我点击了取消");
                return;
            });

            //等待用户输入
            var input = Console.ReadLine();

            if (Convert.ToInt32(input) == 0)
            {
                //如果输入了0，则取消这个任务;
                //一旦cancel被调用。task将会抛出OperationCanceledException来中断此任务的执行，最后将当前task的Status的IsCanceled属性设为true
                TokenSource.Cancel();
            }

            Thread.Sleep(10000);
            Console.WriteLine("任务是否完成：" + task.IsCompleted);
            Console.WriteLine("任务是否成功：" + task.IsCompletedSuccessfully);
            //当线程是因为异常而取消时，IsCanceled字段为true，正常取消时显示为false
            Console.WriteLine("任务是否是被手动取消：" + task.IsCanceled);
            Console.ReadLine();
        }


        /// <summary>
        /// 使用IsCancellationRequested终止一个线程
        /// </summary>
        /// <param name="Token"></param>
        public static void DoWork(CancellationToken Token)
        {
            int count = 0;
            for (var i = 1; i < 10; i++)
            {
                Console.WriteLine("i:" + i);
                Thread.Sleep(1000);
                //判断是否取消任务，取消为true；不取消为false
                if (Token.IsCancellationRequested)
                {
                    count++;
                    Console.WriteLine("取消任务成功！" + count);
                    
                    if (count == 5)
                    {
                        return;
                    }

                }
            }
        }
        #endregion
    }
}
