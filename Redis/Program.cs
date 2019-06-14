using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedisMessageQueen
{
    class Program
    {
        static ConnectionMultiplexer redisClient = ConnectionMultiplexer.Connect("localhost");

        static void Main(string[] args)
        {
            IDatabase db = redisClient.GetDatabase();
            // 测试 key value
            string value = "abcdefg";
            db.StringSet("mykey", value);
            value = db.StringGet("mykey");
            Console.WriteLine(value);

            ISubscriber sub = redisClient.GetSubscriber();

            #region 消息队列方法1
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


            #region 消息队列方法2
            Thread.Sleep(1000);
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
