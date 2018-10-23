using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetData
{
    class Program
    {
        static void Main(string[] args)
        {
            cal();
        }


        public static DataTable getdata()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "data source=192.168.100.123;initial catalog =FCDB;user id=ysnew;password=ysnew;connect timeout=120;pooling=true;max pool size=512;min pool size=1";
                conn.Open(); // 打开数据库连接
                String sql = "select tdate,chg from cihdquote with (nolock) where symbol='000001' order by tdate desc"; // 查询语句
                SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器
                myda.Fill(dt); // 保存数据 
                conn.Close(); // 关闭数据库连接
            }
            return dt;
        }


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
            dt = getdata();
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
