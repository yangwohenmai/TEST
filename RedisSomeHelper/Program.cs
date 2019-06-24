using ServiceStack.Redis;
using System;

namespace RedisSomeHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            #region RedisClientHelper
            RedisClientHelper.MainHelper();
            Console.ReadLine();
            #endregion

            #region SampleRedisHelper
            RedisClient client = new RedisClient("127.0.0.1", 6379);
            var timeOut = new TimeSpan(0, 0, 30);
            SampleRedisHelper.AddString(client);
            SampleRedisHelper.AddPerson(client);
            SampleRedisHelper.Addhash(client);
            SampleRedisHelper.AddQueue(client);
            SampleRedisHelper.AddStack(client);
            SampleRedisHelper.AddSet(client);
            SampleRedisHelper.AddSortSet(client);
            SampleRedisHelper.Union(client);
            SampleRedisHelper.JJ(client);
            SampleRedisHelper.CJ(client);
            Console.ReadLine();
            #endregion
        }
    }
}
