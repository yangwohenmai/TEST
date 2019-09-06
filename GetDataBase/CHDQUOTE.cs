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
            Dictionary<string, SortedList<string, List<string>>> list = new Dictionary<string, SortedList<string, List<string>>>();
            // [-,20050101],
            // (20050101,20100101]
            // (20100101,20150101]
            // (20150101,20150101]
            // (20170101,+]
            Dictionary<string, SortedList<string, List<string>>> listnew = getdatago1(list,
                @"select TDATE ,EXCHANGE , SYMBOL ,SNAME ,LCLOSE,TOPEN ,TCLOSE ,HIGH ,LOW ,VOTURNOVER , VATURNOVER ,NDEALS ,
        AVGPRICE , AVGVOLPD , AVGVAPD ,CHG ,PCHG ,PRANGE,MCAP, TCAP,TURNOVER from chdquote with(nolock) 
        where  tdate >20170101 --AND tdate <=20170101 ", 
                @"data source=192.168.100.123;initial catalog =FCDB;user id=ysnew;password=ysnew;connect timeout=120;pooling=true;max pool size=512;min pool size=1");
            #endregion
            return listnew;
        }

        


        /// <summary>
        /// 从数据库读取股票数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sql"></param>
        /// <param name="dbcon"></param>
        /// <returns></returns>
        public static Dictionary<string, SortedList<string, string>> getdatago(Dictionary<string, SortedList<string, string>> list, string sql, string dbcon)
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

            Dictionary<string, SortedList<string, string>> ListDic = list;
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
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sql"></param>
        /// <param name="dbcon"></param>
        /// <returns></returns>
        public static Dictionary<string, SortedList<string, List<string>>> getdatago1(Dictionary<string, SortedList<string, List<string>>> list, string sql, string dbcon)
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
            Dictionary<string, SortedList<string, List<string>>> ListDic = list;
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
    }
}
