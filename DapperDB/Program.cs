using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.IO;

namespace DapperDB
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TaskDriverStru> list = GetTaskDriver1();
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
        /// Sqlite加载TableStatusStru
        /// </summary>
        /// <param name="options"></param>
        /// <param name="pageClick"></param>
        /// <param name="sqlDic"></param>
        /// <returns></returns>
        public static List<TaskDriverStru> GetTaskDriver1()
        {
            // 查询数据总数
            List<TaskDriverStru> tempDataList = new List<TaskDriverStru>();
            string errorMsg = string.Empty;

            string sql = @"SELECT * FROM TaskStatus order by taskid desc;";

            DbConnStru SqlitePath = new DbConnStru();
            string str_ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "finchina.db3");
            SqlitePath.connStr = string.Format("Data Source={0};Pooling=true;FailIfMissing=false", str_ConfigFilePath);
            SqlitePath.dbType = "sqlite";
            SqlitePath.key = "SqliteConn_1";
            SqlitePath.isEncrypt = false;

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
        public static IEnumerable<T> Select<T>(string sql, DbConnStru connConfig)
        {
            IEnumerable<T> list = null;

            using (IDbConnection conn = DapperAcc.GetConnection(connConfig))
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
                    //Log4NetUtil.Error(this, "Select->" + ex.ToString());
                }
            }
            return list;
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

    /// <summary>
    /// 数据库连接配置结构
    /// </summary>
    public class DbConnStru
    {
        /// <summary>
        /// Key
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 数据库类型 sqlite,sqlserver,oracle,mysql
        /// </summary>
        public string dbType { get; set; }
        /// <summary>
        /// 链接字符串
        /// </summary>
        public string connStr { get; set; }
        /// <summary>
        /// 链接字符串是否被加密
        /// </summary>
        public bool isEncrypt { get; set; }
    }
}
