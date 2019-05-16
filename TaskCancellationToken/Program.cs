using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCancellationToken
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            tasktest2();
        }


        /// <summary>
        /// 
        /// </summary>
        public static void tasktest2()
        {
            //通知 System.Threading.CancellationToken，告知其应被取消。
            var TokenSource = new CancellationTokenSource();

            //获取与此 System.Threading.CancellationTokenSource 关联的 System.Threading.CancellationToken。
            var Token = TokenSource.Token;

            var task = Task.Factory.StartNew(() => DoWork(Token), Token);
            
            //在这里还可以给token注册了一个方法，输出一条信息  用户输入0后，执行tokenSource.Cancel方法取消任务。这个取消任务执行后，会执行这个代码.
            Token.Register(() => { Console.WriteLine("取消");Console.WriteLine("我点击了取消");return; });

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
    }
}
