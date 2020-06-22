using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataBase
{
    public class EQ_Dispara
    {
        /// <summary>
        /// 把股票数据组装成字典
        /// </summary>
        /// <returns></returns>
        public static List<List<string>> GetEQ_Dispara()
        {
            List<List<string>> listnew = getdatago(@"select Tdate,Exchange,Symbol,DType,D_nt,F_nt,S_nt,K_nt,C_nt,TClose,LClose,R,SECode
                from eq_dispara with(nolock) ",
                @"data source=192.168.100.123;initial catalog =FCDB;user id=read;password=read;connect timeout=120;pooling=true;max pool size=512;min pool size=1");
            //插入
            insert(listnew);
            return listnew;
        }

        /// <summary>
        /// 从数据库读取股票数据
        /// Sqlit使用
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sql"></param>
        /// <param name="dbcon"></param>
        /// <returns></returns>
        public static List<List<string>> getdatago(string sql, string dbcon)
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
            List<List<string>> listall = new List<List<string>>();
            while (dr.Read())
            {
                //string str = "";
                liststr = new List<string>();
                //股票一共13个字段，组成一行数据
                for (int i = 0; i < 13; i++)
                {
                    liststr.Add(Convert.ToString(dr[i]));
                }

                listall.Add(liststr);
            }
            return listall;
        }

        /// <summary>
        /// 直接插入数据库
        /// </summary>
        /// <param name="listnew"></param>
        public static void insert(List<List<string>> listnew)
        {
            SqliteAccess sqlite = new SqliteAccess("E:\\临时测试程序\\GetDBData\\DataBase3.db3");
            //SqliteAccess sqlite = new SqliteAccess("C:\\Users\\ysjsfw\\Desktop\\DataBase3.db3");
            string DBLINK = @"Data Source=E:\\临时测试程序\\GetDBData\\DataBase.db3;Pooling=true;FailIfMissing=false";
            //string DBLINK = @"Data Source=C:\\Users\\ysjsfw\\Desktop\\DataBase.db3;Pooling=true;FailIfMissing=false";
            StringBuilder stblist = new StringBuilder();
            int count = 0;
            int countFinish = 0;
            List<string> list = new List<string>();
            string strinsert = @"INSERT INTO eq_dispara
                            (Tdate,Exchange,Symbol,DType,D_nt,F_nt,S_nt,K_nt,C_nt,TClose,LClose,R,SECode)
                            VALUES({0})";
            string temp = "";
            foreach (var sort in listnew)
            {
                foreach (var i in sort)
                {
                    temp += "'" + Convert.ToString(i) + "',";
                }
                temp = temp.Substring(0, temp.Length - 1);
                list.Add(string.Format(strinsert, temp));
                temp = "";
                count++;

                if (count == 5000)
                {
                    sqlite.insertQuick(DBLINK, list);
                    list = new List<string>();
                    count = 0;
                    countFinish += 5000;
                    Console.WriteLine(countFinish.ToString());
                }
            }
            Console.WriteLine("countFinish:" + countFinish.ToString());
            Console.WriteLine("count:" + count.ToString());
            if (list.Count > 0)
            {
                sqlite.insertQuick(DBLINK, list);
            }
        }
    }
}
