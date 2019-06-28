using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisSomeHelper
{
    public static class SampleRedisHelper
    {
        //差集
        public static void CJ(RedisClient client)
        {
            var hashD = client.GetDifferencesFromSet("Set8001", new string[] { "Set8002" });  //[返回存在于第一个集合，但是不存在于其他集合的数据。差集]
            foreach (var item in hashD)
            {
                Console.WriteLine(item);
            }
        }

        //交集
        public static void JJ(RedisClient client)
        {
            var hashG = client.GetIntersectFromSets(new string[] { "Set8001", "Set8002" });
            foreach (var item in hashG)
            {
                Console.WriteLine(item);
            }
        }

        //合集
        public static void Union(RedisClient client)
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
        public static void AddSortSet(RedisClient client)
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
        public static void AddSet(RedisClient client)
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
        public static void AddStack(RedisClient client)
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
        public static void AddQueue(RedisClient client)
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
        public static void Addhash(RedisClient client)
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
        public static void AddPerson(RedisClient client)
        {
            var person = new Person() { Name = "qhh", Age = 26 };
            client.Add("person", person);
            var cachePerson = client.Get<Person>("person");
            Console.WriteLine("name=" + cachePerson.Name + "----age=" + cachePerson.Age);
        }

        //添加字符串
        public static void AddString(RedisClient client)
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
