using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GetData
{
    class Program
    {
        static string title = "";
        static void Main(string[] args)
        {
            Console.WriteLine("BEGIN");
            //JQData.get();
            //aa();
            Dictionary<string, SortedList<string, string>> listnew = CHDQUOTE.GetCHDQUOTE();
            #region 读取股票数据
            //Dictionary<string, SortedList<string, string>> list = new Dictionary<string, SortedList<string, string>>();
            //Dictionary<string, SortedList<string, string>> listnew = getdatago(list, "select * from chdquote with(nolock) where tdate >=20130101 ", "data source=192.168.100.123;initial catalog =FCDB;user id=ysnew;password=ysnew;connect timeout=120;pooling=true;max pool size=512;min pool size=1");
            #endregion

            Console.WriteLine("GET");
            //Dictionary<string, SortedList<string, string>> listnew1 = getdatago(listnew, "select * from chdquote with(nolock) where tdate >=20130101 ", "data source=192.168.100.123;initial catalog =FCDB;user id=ysnew;password=ysnew;connect timeout=120;pooling=true;max pool size=512;min pool size=1");
            //DataTable dt = GetChdquote();
            //Console.WriteLine("GET1");
            foreach (var sort in listnew)
            {
                foreach (var item in sort.Value)
                {
                    AddLog(sort.Key, item.Value);
                }
            }


            //list = new Dictionary<string, SortedList<string, string>>();
            //Dictionary<string, SortedList<string, string>> listnew1 = getdatago(listnew, "select * from chdquote with(nolock) where tdate >=20130101 ", "data source=192.168.100.123;initial catalog =FCDB;user id=ysnew;password=ysnew;connect timeout=120;pooling=true;max pool size=512;min pool size=1");
            //foreach (var sort in listnew1)
            //{
            //    foreach (var item in sort.Value)
            //    {
            //        AddLog(sort.Key, item.Value);
            //    }
            //}


            //creatTxt(dt);
            Console.WriteLine("FINISH");
            //string ColumnName = "";
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    ColumnName += dt.Columns[i].ColumnName.ToString()+",";
            //}
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    string a = dt.Rows[i]["TDATE"].ToString();
            //    string a = dt.Rows[i]["EXCHANGE"].ToString();
            //    string a = dt.Rows[i]["SYMBOL"].ToString();
            //    string a = dt.Rows[i]["SNAME"].ToString();
            //    string a = dt.Rows[i]["LCLOSE"].ToString();
            //    string a = dt.Rows[i]["TOPEN"].ToString();
            //    string a = dt.Rows[i]["TCLOSE"].ToString();
            //    string a = dt.Rows[i]["HIGH"].ToString();
            //    string a = dt.Rows[i]["LOW"].ToString();
            //    string a = dt.Rows[i]["VOTURNOVER"].ToString();
            //    string a = dt.Rows[i]["VATURNOVER"].ToString();
            //    string a = dt.Rows[i]["NDEALS"].ToString();
            //    string a = dt.Rows[i]["AVGPRICE"].ToString();
            //    string a = dt.Rows[i]["AVGVOLPD"].ToString();
            //    string a = dt.Rows[i]["AVGVAPD"].ToString();
            //    string a = dt.Rows[i]["CHG"].ToString();
            //    string a = dt.Rows[i]["PCHG"].ToString();
            //    string a = dt.Rows[i]["PRANGE"].ToString();
            //    string a = dt.Rows[i]["MCAP"].ToString();
            //    string a = dt.Rows[i]["TCAP"].ToString();
            //    string a = dt.Rows[i]["TURNOVER"].ToString();
            //    string a = dt.Rows[i]["ENTRYDATE"].ToString();
            //    string a = dt.Rows[i]["ENTRYTIME"].ToString();

            //}
            //cal();
        }

        public static void AddLog(string filename, string message)
        {
            //StreamWriter sw = null;
            string logFile = "D:\\logs\\" + filename + ".txt";
            //FileStream fs = new FileStream(logFile, FileMode.Create, FileAccess.ReadWrite);
            //StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
            try
            {
                File.AppendAllText(logFile, message + "\r\n", Encoding.GetEncoding("GB2312"));
                //if (File.Exists(logFile))
                //{
                //    File.AppendAllText(logFile, message + "\r\n", Encoding.GetEncoding("GB2312"));
                //    //File.AppendAllLines
                //}
                //else
                //{
                //    //File.CreateText(logFile);
                //    File.AppendAllText(logFile, message + "\r\n", Encoding.GetEncoding("GB2312"));
                //}
                //sw.WriteLine(message);
                //sw.WriteLine(message + "\r\n");
            }
            catch (Exception ex)
            {
                string a = ex.ToString();

            }
            finally
            {
                //if (sw != null)
                //{
                    //sw.Close();
                //}
            }
        }


        public static DataTable GetChdquote()
        {
            DataTable dt = getdata("select * from chdquote with(nolock) WHERE TDATE>20160101");
            return dt;
        }

        public static void creatTxt(DataTable dt)
        {
            string fn = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + "PO014" + ".txt";
            //String conStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=C:\\Users\\yangzo\\Desktop\\C5S6\\CID.xls;" + "Extended Properties=Excel 8.0;";
            //OleDbConnection con = new OleDbConnection(conStr);
            //con.Open();
            //string sql = "select code,name,type from [Sheet2$]";
            ////OleDbCommand mycom = new OleDbCommand("select * from TSD_PO014", mycon);
            ////OleDbDataReader myreader = mycom.ExecuteReader(); //也可以用Reader读取数据
            //DataSet ds = new DataSet();
            //OleDbDataAdapter oda = new OleDbDataAdapter(sql, con);
            //oda.Fill(ds, "PO014");



            //DataTable dt = ds.Tables[0];
            FileStream fs = new FileStream("D:\\" + fn, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter strmWriter = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));    //存入到文本文件中 
            //把标题写入.txt文件中
            string str = ",";
            string ColumnName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ColumnName += dt.Columns[i].ColumnName.ToString() + ",";
            }
            strmWriter.Write(ColumnName);
            strmWriter.WriteLine(); //换行

            //数据用"|"分隔开
            foreach (DataRow dr in dt.Rows)
            {
                strmWriter.Write(dr[0].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[1].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[2].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[3].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[4].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[5].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[6].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[7].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[8].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[9].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[10].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[11].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[12].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[13].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[14].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[15].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[16].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[17].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[18].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[19].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[20].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[21].ToString());
                strmWriter.Write(str);
                strmWriter.Write(dr[22].ToString());
                strmWriter.Write(str);
                strmWriter.WriteLine(); //换行
            }
            strmWriter.Flush();
            strmWriter.Close();
            //if (con.State == ConnectionState.Open)
            //{
            //    con.Close();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sql"></param>
        /// <param name="dbcon"></param>
        /// <returns></returns>
        public static Dictionary<string, SortedList<string, string>> getdatago(Dictionary<string, SortedList<string, string>> list, string sql,string dbcon)
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

            Dictionary<string, SortedList<string,string>> ListDic = list;
            while (dr.Read())
            {
                string str = "";
                for (int i = 0; i < 23; i++)
                {
                    str += dr[i] + ",";
                }
                if (!ListDic.ContainsKey(dr[2].ToString()))
                {
                    SortedList<string, string> sort = new SortedList<string, string>();
                    sort.Add(dr[0].ToString(), str);
                    ListDic[dr[2].ToString()] = sort;
                }
                else
                {
                    ListDic[dr[2].ToString()][dr[0].ToString()] = str;
                }
            }
            return ListDic;
        }

        #region SqlDataReader
        public static void aa()
        {
            SqlDataReader dr = QueryDr("SELECT id,filelist as name,rs0001_002 as picture FROM rs0001 WITH (NOLOCK) WHERE rscode = '7786585327'", 300, "");
            if (dr != null)
            {
                while (dr.Read())
                {
                    Console.WriteLine(DateTime.Now+" begin");
                    string b = Convert.ToString(dr["id"]);
                    Console.WriteLine(DateTime.Now + " id");
                    string a = Convert.ToString(dr["name"]);
                    Console.WriteLine(DateTime.Now + " name");
                    Console.WriteLine(DateTime.Now + " end");
                }
           }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeOut"></param>
        /// <param name="strConnStr"></param>
        /// <returns></returns>
        public static SqlDataReader QueryDr(string sql, int timeOut, string strConnStr = "")
        {
            SqlConnection conn = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr = null;
            
            try
            {
                //取得数据库连接
                conn = new SqlConnection("server=10.10.13.97;database=BASICDB;uid=read;pwd=read;connect Timeout=60;pooling = true;max pool size=10;min pool size=2;Application Name=newsPlatform");
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                //等待命令执行的时间（以秒为单位）,默认值为 30 秒。 
                if (timeOut <= 30)
                {
                    cmd.CommandTimeout = 30;
                }
                else
                {
                    cmd.CommandTimeout = timeOut;
                }

                conn.Open();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                cmd.Dispose();
            }
            

            return dr;
        }
        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable getdata(string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "data source=192.168.100.123;initial catalog =FCDB;user id=ysnew;password=ysnew;connect timeout=120;pooling=true;max pool size=512;min pool size=1";
                conn.Open(); // 打开数据库连接
                
                //String sql = "select tdate,chg from cihdquote with (nolock) where symbol='000001' order by tdate desc"; // 查询语句

                SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器
                myda.Fill(dt); // 保存数据 
                conn.Close(); // 关闭数据库连接
            }
            return dt;
        }

        /// <summary>
        /// 大盘统计
        /// </summary>
        /// <returns></returns>
        public static DataTable cal()
        {
            DataTable dtWeek = new DataTable();
            dtWeek.Columns.Add("日期范围");
            dtWeek.Columns.Add("周一");
            dtWeek.Columns.Add("周二");
            dtWeek.Columns.Add("周三");
            dtWeek.Columns.Add("周四");
            dtWeek.Columns.Add("周五");
            dtWeek.Columns.Add("周六");
            dtWeek.Columns.Add("周日");


            DataTable dt = new DataTable();
            dt = getdata("");
            int T = 0;
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            
            foreach (DataRow row in dt.Rows)
            {
                dicData.Add(row["tdate"].ToString(), row["chg"].ToString());
            }


            string begin = "";
            string end = "";
            Dictionary<string, string> dicEveryWeek = new Dictionary<string, string>();
            for (DateTime dtEvery = new DateTime(1990, 12, 24); dtEvery <= new DateTime(2018, 9, 6); dtEvery = dtEvery.AddDays(1))
            {
                string strdt = dtEvery.ToString("yyyyMMdd");
                
                if (dicData.ContainsKey(strdt))
                {
                    DateTime dtime = DateTime.ParseExact(strdt, "yyyyMMdd", null);
                    if (dtime.DayOfWeek.ToString() == "Monday")
                    {
                        dicEveryWeek["周一"] = dicData[strdt].ToString();
                        begin = strdt;
                    }
                    else if (dtime.DayOfWeek.ToString() == "Tuesday")
                    {
                        dicEveryWeek["周二"] = dicData[strdt].ToString();
                    }
                    else if (dtime.DayOfWeek.ToString() == "Wednesday")
                    {
                        dicEveryWeek["周三"] = dicData[strdt].ToString();
                    }
                    else if (dtime.DayOfWeek.ToString() == "Thursday")
                    {
                        dicEveryWeek["周四"] = dicData[strdt].ToString();
                    }
                    else if (dtime.DayOfWeek.ToString() == "Friday")
                    {
                        dicEveryWeek["周五"] = dicData[strdt].ToString();
                    }
                    else if (dtime.DayOfWeek.ToString() == "Saturday")
                    {
                        dicEveryWeek["周六"] = dicData[strdt].ToString();
                    }
                    else
                    {
                        dicEveryWeek["周日"] = dicData[strdt].ToString();
                        end = strdt;
                    }
                }
                else
                {
                    DateTime dtime = DateTime.ParseExact(strdt, "yyyyMMdd", null);
                    if (dtime.DayOfWeek.ToString() == "Monday")
                    {
                        dicEveryWeek["周一"] = "NULL";
                        begin = strdt;
                    }
                    else if (dtime.DayOfWeek.ToString() == "Tuesday")
                    {
                        dicEveryWeek["周二"] = "NULL";
                    }
                    else if (dtime.DayOfWeek.ToString() == "Wednesday")
                    {
                        dicEveryWeek["周三"] = "NULL";
                    }
                    else if (dtime.DayOfWeek.ToString() == "Thursday")
                    {
                        dicEveryWeek["周四"] = "NULL";
                    }
                    else if (dtime.DayOfWeek.ToString() == "Friday")
                    {
                        dicEveryWeek["周五"] = "NULL";
                    }
                    else if (dtime.DayOfWeek.ToString() == "Saturday")
                    {
                        dicEveryWeek["周六"] = "NULL";
                    }
                    else
                    {
                        dicEveryWeek["周日"] = "NULL";
                        end = strdt;
                    }
                }
                T++;
                if (T == 7)
                {
                    DataRow drNew = dtWeek.NewRow();
                    drNew["日期范围"] = begin + "~" + end;
                    drNew["周一"] = dicEveryWeek["周一"];
                    drNew["周二"] = dicEveryWeek["周二"];
                    drNew["周三"] = dicEveryWeek["周三"];
                    drNew["周四"] = dicEveryWeek["周四"];
                    drNew["周五"] = dicEveryWeek["周五"];
                    drNew["周六"] = dicEveryWeek["周六"];
                    drNew["周日"] = dicEveryWeek["周日"];
                    dtWeek.Rows.Add(drNew);
                    dicEveryWeek = new Dictionary<string, string>();
                    T = 0;
                }
            }
            return dtWeek;
        }
        

    }
}