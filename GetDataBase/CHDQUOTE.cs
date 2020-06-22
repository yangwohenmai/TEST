using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataBase
{
    public class CHDQUOTE
    {
        /// <summary>
        /// 把股票数据组装成字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, SortedList<string, List<string>>> GetCHDQUOTE()
        {
            #region 读取股票数据
            // [-,20050101],
            // (20050101,20100101]
            // (20100101,20150101]
            // (20150101,20150101]
            // (20170101,+]
            Dictionary<string, SortedList<string, List<string>>> listnew = getdatago1(@"select TDATE ,EXCHANGE , SYMBOL ,SNAME ,LCLOSE,TOPEN ,TCLOSE ,HIGH ,LOW ,VOTURNOVER , VATURNOVER ,NDEALS ,
        AVGPRICE , AVGVOLPD , AVGVAPD ,CHG ,PCHG ,PRANGE,MCAP, TCAP,TURNOVER from chdquote with(nolock) 
        where  tdate >20170101 AND tdate <=20170201 ",
                @"data source=192.168.100.123;initial catalog =FCDB;user id=read;password=read;connect timeout=120;pooling=true;max pool size=512;min pool size=1");
            //插入
            insertDB(listnew);
            #endregion
            return listnew;
        }




        /// <summary>
        /// 从数据库读取股票数据
        /// txt使用
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sql"></param>
        /// <param name="dbcon"></param>
        /// <returns></returns>
        public static Dictionary<string, SortedList<string, string>> getdatago(string sql, string dbcon)
        {
            SqlConnection conn = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr = null;

            try
            {
                conn = new SqlConnection(dbcon);
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 1200;
                conn.Open();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //conn.Close();
            }

            Dictionary<string, SortedList<string, string>> ListDic = new Dictionary<string, SortedList<string, string>>();
            while (dr.Read())
            {
                string str = "";
                //股票一共23个字段，组成一行数据
                for (int i = 0; i < 21; i++)
                {
                    str += dr[i] + ",";
                }

                if (!ListDic.ContainsKey(dr[2].ToString()))
                {
                    SortedList<string, string> sort = new SortedList<string, string>();
                    //用日期作为排序Key
                    sort.Add(dr[0].ToString(), str);
                    //用股票代码作为Key
                    ListDic[dr[2].ToString()] = sort;
                }
                else
                {
                    ListDic[dr[2].ToString()][dr[0].ToString()] = str;
                }
            }
            return ListDic;
        }



        /// <summary>
        /// 从数据库读取股票数据
        /// SqlLite使用
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbcon"></param>
        /// <returns></returns>
        public static Dictionary<string, SortedList<string, List<string>>> getdatago1(string sql, string dbcon)
        {
            SqlConnection conn = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr = null;

            try
            {
                conn = new SqlConnection(dbcon);
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 1200;
                conn.Open();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //conn.Close();
            }
            List<string> liststr = new List<string>();
            Dictionary<string, SortedList<string, List<string>>> ListDic = new Dictionary<string, SortedList<string, List<string>>>();
            while (dr.Read())
            {
                //string str = "";
                liststr = new List<string>();
                //股票一共23个字段，组成一行数据
                for (int i = 0; i < 21; i++)
                {
                    liststr.Add(Convert.ToString(dr[i]));
                }

                if (!ListDic.ContainsKey(dr[2].ToString()))
                {
                    SortedList<string, List<string>> sort = new SortedList<string, List<string>>();
                    //用日期作为排序Key
                    sort.Add(dr[0].ToString(), liststr);
                    //用股票代码作为Key
                    ListDic[dr[2].ToString()] = sort;
                }
                else
                {
                    ListDic[dr[2].ToString()][dr[0].ToString()] = liststr;
                }
            }
            return ListDic;
        }


        public static void insertDB(Dictionary<string, SortedList<string, List<string>>> listnew)
        {
            SqliteAccess sqlite = new SqliteAccess("E:\\临时测试程序\\GetDBData\\DataBase3.db3");
            //SqliteAccess sqlite = new SqliteAccess("C:\\Users\\ysjsfw\\Desktop\\DataBase3.db3");
            //string DBLINK = @"Data Source=E:\\临时测试程序\\GetDBData\\DataBase3.db3;Pooling=true;FailIfMissing=false";
            string DBLINK = @"Data Source=C:\\Users\\ysjsfw\\Desktop\\DataBase.db3;Pooling=true;FailIfMissing=false";
            StringBuilder stblist = new StringBuilder();
            int count = 0;//当前批次数量
            int countFinish = 0;//已完成数量
            List<string> list = new List<string>();
            string strinsert = @"INSERT INTO CHDQUOTE
                            (TDATE,
                              EXCHANGE,
                              SYMBOL,
                              SNAME,
                              LCLOSE,
                              TOPEN,
                              TCLOSE,
                              HIGH,
                              LOW,
                              VOTURNOVER,
                              VATURNOVER,
                              NDEALS,
                              AVGPRICE,
                              AVGVOLPD,
                              AVGVAPD,
                              CHG,
                              PCHG,
                              PRANGE,
                              MCAP,
                              TCAP,
                              TURNOVER
                            )
                            VALUES({0})";
            string temp = "";
            //代码索引
            foreach (var sort in listnew)
            {
                //日期索引
                foreach (var item in sort.Value)
                {
                    //拼接字段
                    foreach (var i in item.Value)
                    {
                        temp += "'" + Convert.ToString(i) + "',";
                    }
                    temp = temp.Substring(0, temp.Length - 1);
                    list.Add(string.Format(strinsert, temp));
                    temp = "";
                    count++;
                    //5000个一批次插入
                    if (count == 5000)
                    {
                        sqlite.insertQuick(DBLINK, list);
                        list = new List<string>();
                        count = 0;
                        countFinish += 5000;
                        Console.WriteLine(countFinish.ToString());
                    }
                }
            }
            Console.WriteLine("countFinish:" + countFinish.ToString());
            Console.WriteLine("count:" + count.ToString());

            //不足5000批次插入
            if (list.Count > 0)
            {
                sqlite.insertQuick(DBLINK, list);
            }
        }
    }
}
