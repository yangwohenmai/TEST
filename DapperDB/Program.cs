using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.IO;
using System.Data.SQLite;

namespace DapperDB
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TaskDriverStru> list = GetTask();
            foreach(var item in list)
            {
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Init);
                Console.WriteLine(item.Tables);
                Console.WriteLine(item.taskId);
                Console.WriteLine(item.TaskName);
                Console.WriteLine(item.taskStatus);
                Console.WriteLine(item.userName);
            }
            Console.ReadLine();
        }



        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public static List<TaskDriverStru> GetTask()
        {
            List<TaskDriverStru> tempDataList = new List<TaskDriverStru>();
            string errorMsg = string.Empty;

            string sql = @"SELECT * FROM TaskStatus;";

            string str_ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "finchina.db3");
            string SqlitePath = string.Format("Data Source={0};Pooling=true;FailIfMissing=false", str_ConfigFilePath);

            var list = Select<TaskDriverStru>(sql, SqlitePath);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return null;
            }

            foreach (TaskDriverStru item in list)
            {
                tempDataList.Add(item);
            }

            return tempDataList;
        }





        /// <summary>
        /// ORM数据映射
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="connConfig"></param>
        /// <returns></returns>
        public static IEnumerable<T> Select<T>(string sql, string connConfig)
        {
            IEnumerable<T> list = null;

            using (IDbConnection conn = GetConnection(connConfig))
            {
                try
                {
                    //传统sql in (1, 2, 3)写法
                    //conn.Query<TestTable>("SELECT * FROM TestTable  WHERE id IN (@ids) ",new { ids = IDs.ToArray()})
                    list = conn.Query<T>(sql);
                    //list = conn.Query<DataInfoStru>(sql, new { id = ID });
                }
                catch (Exception ex)
                {
                }
            }
            return list;
        }

        /// <summary>
        /// 链接数据库
        /// </summary>
        /// <param name="connConfig"></param>
        /// <returns></returns>
        public static IDbConnection GetConnection(string connConfig)
        {
            IDbConnection conn = null;

            if (string.IsNullOrEmpty(connConfig))
            {
                return null;
            }

            try
            {
                conn = new SQLiteConnection(connConfig);
                //打开数据库链接
                conn.Open();
            }
            catch (Exception ex)
            {
            }
            return conn;
        }

    }


    public class TaskDriverStru
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string taskId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string Tables { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public string taskStatus { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TMSP1 { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TMSP2 { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TMSP3 { get; set; }
        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool Init { get; set; }
    }


}
