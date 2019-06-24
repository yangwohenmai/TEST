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
            RedisClientHelper.MainHelper();

            RedisClient client = new RedisClient("127.0.0.1", 6379);
            var timeOut = new TimeSpan(0, 0, 30);
            AddString(client);
            AddPerson(client);
            Addhash(client);
            AddQueue(client);
            AddStack(client);
            AddSet(client);
            AddSortSet(client);
            Union(client);
            JJ(client);
            CJ(client);

            RedisClientHelper.MainHelper();
            Console.ReadLine();

            IDatabase db = redisClient.GetDatabase();
            // 测试 key value
            #region 存取一个key value到Redis中
            string value = "你好";
            db.StringSet("mykey", value);
            value = db.StringGet("111");
            //db.SetAdd("sdaf", new Student { id = "q", name = "df" });
            Console.WriteLine(value);
            
            #endregion

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


            #region 消息队列方法2，在客户端通过SUBSCRIBE <通道名称>来监听
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

        //差集
        private static void CJ(RedisClient client)
        {
            var hashD = client.GetDifferencesFromSet("Set8001", new string[] { "Set8002" });  //[返回存在于第一个集合，但是不存在于其他集合的数据。差集]
            foreach (var item in hashD)
            {
                Console.WriteLine(item);
            }
        }

        //交集
        private static void JJ(RedisClient client)
        {
            var hashG = client.GetIntersectFromSets(new string[] { "Set8001", "Set8002" });
            foreach (var item in hashG)
            {
                Console.WriteLine(item);
            }
        }

        //合集
        private static void Union(RedisClient client)
        {
            client.AddItemToSet("Set8001", "A");
            client.AddItemToSet("Set8001", "B");
            client.AddItemToSet("Set8001", "C");
            client.AddItemToSet("Set8001", "D");


            client.AddItemToSet("Set8002", "E");
            client.AddItemToSet("Set8002", "F");
            client.AddItemToSet("Set8002", "G");
            client.AddItemToSet("Set8002", "D");
            var setunion = client.GetUnionFromSets("Set8001", "Set8002");
            foreach (var item in setunion)
            {
                Console.WriteLine(item);
            }
        }


        //加入有序集合
        private static void AddSortSet(RedisClient client)
        {
            client.AddItemToSortedSet("SetSorted1001", "1.qhh");
            client.AddItemToSortedSet("SetSorted1001", "2.qihaohao");
            client.AddItemToSortedSet("SetSorted1001", "3.qihh");
            var sortset = client.GetAllItemsFromSortedSet("SetSorted1001");
            foreach (var item in sortset)
            {
                Console.WriteLine(item);
            }

            var sortset1 = client.GetRangeFromSortedSet("SetSorted1001", 0, 0);

            foreach (var item in sortset1)
            {
                Console.WriteLine(item);
            }

            var list = client.GetRangeFromSortedSetDesc("SetSorted1001", 0, 0);

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

        }

        //加入Set
        private static void AddSet(RedisClient client)
        {
            client.AddItemToSet("Set1001", "qhh");
            client.AddItemToSet("Set1001", "qihaohao");
            client.AddItemToSet("Set1001", "qihh");
            var set = client.GetAllItemsFromSet("Set1001");
            foreach (var item in set)
            {
                Console.WriteLine(item);
            }
        }

        //加入栈
        private static void AddStack(RedisClient client)
        {
            client.PushItemToList("StackListId", "1.qhh");  //入栈
            client.PushItemToList("StackListId", "2.qihaohao");
            client.PushItemToList("StackListId", "3.qihh");
            var stackCount = client.GetListCount("StackListId");
            for (int i = 0; i < stackCount; i++)
            {
                Console.WriteLine(client.PopItemFromList("StackListId"));
            }
        }

        //加入队列
        private static void AddQueue(RedisClient client)
        {
            client.EnqueueItemOnList("QueueListId", "1.qhh");
            client.EnqueueItemOnList("QueueListId", "2.qihaohao");
            client.EnqueueItemOnList("QueueListId", "3.qihh");
            var queue = client.GetListCount("QueueListId");
            for (int i = 0; i < queue; i++)
            {
                Console.WriteLine(client.DequeueItemFromList("QueueListId"));
            }
        }

        //加入哈希
        private static void Addhash(RedisClient client)
        {
            client.SetEntryInHash("HashId", "Name", "QHH");
            client.SetEntryInHash("HashId", "Age", "26");
            var hash = client.GetHashValues("HashId");
            foreach (var item in hash)
            {
                Console.WriteLine(item);
            }

        }

        //添加对象
        private static void AddPerson(RedisClient client)
        {
            var person = new Person() { Name = "qhh", Age = 26 };
            client.Add("person", person);
            var cachePerson = client.Get<Person>("person");
            Console.WriteLine("name=" + cachePerson.Name + "----age=" + cachePerson.Age);
        }

        //添加字符串
        private static void AddString(RedisClient client)
        {
            client.Add("qhh", "qihaohao");
            Console.WriteLine(client.Get<string>("qhh"));
        }

    }
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
