using ServiceStack.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedisMessageQueen
{
    [Serializable]
    public class Student
    {
        public string id;
        public string name;
    }
    class Program
    {
        //static ConnectionMultiplexer redisClient = ConnectionMultiplexer.Connect("localhost");
        static ConnectionMultiplexer redisClient = ConnectionMultiplexer.Connect("127.0.0.1:6379");

        static void Main(string[] args)
        {
            ISubscriber sub = redisClient.GetSubscriber();

            #region 消息队列方法1，自己发消息自己取数据
            //建立一个监听对象，监听通道名为messages
            sub.Subscribe("messages", (channel, message) =>
            {
                //收到消息后将消息显示出来
                Console.WriteLine((string)message);
            });
            Thread.Sleep(1000);

            //通过Publish向messages通道发送消息，发送通道名称和监听通道名称要相同
            sub.Publish("messages", "hello");
            sub.Publish("messages", "hello1");
            sub.Publish("messages", "hello2");
            #endregion


            #region 消息队列方法2，在cmd客户端通过 SUBSCRIBE <通道名称> 命令来监听
            Thread.Sleep(10000);
            sub.Publish("redisMessages", "This message from C# program123");
            string pub = Console.ReadLine();
            //连发十条消息
            for (int i = 0; i < 10; i++)
            {
                sub.Publish("redisMessages", pub);
            }
            #endregion


            Console.ReadLine();
        }
    }
}
