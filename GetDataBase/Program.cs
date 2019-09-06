using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            string title = "TDate,Exchange,Symbol,Sname,LClose,TOpen,TClose,High,Low,VaTurnover,VoTurnover,Chg,PChg,PRange";
            Console.WriteLine("BEGIN");
            Dictionary<string, SortedList<string, List<string>>> listnew = CHDQUOTE.GetCHDQUOTE();
            //Dictionary<string, SortedList<string, string>> listnew = HKHDQUOTE.GetHKHDQUOTE();

            Console.WriteLine("GET");
            //insert(listnew);
            newinsert(listnew);

            Console.ReadLine();


            //foreach (var sort in listnew)
            //{
            //    //添加抬头
            //    //Common.AddLog(sort.Key, title);
            //    foreach (var item in sort.Value)
            //    {
            //        //Common.AddLog(sort.Key, item.Value);
            //    }
            //}
        }


        static void insertlast(Dictionary<string, SortedList<string, List<string>>> listnew)
        {
            SqliteAccess sqlite = new SqliteAccess("E:\\临时测试程序\\GetDBData\\DataBase3.db3");
            //SqliteAccess sqlite = new SqliteAccess("C:\\Users\\ysjsfw\\Desktop\\DataBase3.db3");
            string strlist = "";
            StringBuilder stblist = new StringBuilder();
            string str = "";
            int count = 0;
            int countall = 0;
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
            foreach (var sort in listnew)
            {
                foreach (var item in sort.Value)
                {
                    count++;
                    if (count > 1236000)
                    {
                        foreach (var i in item.Value)
                        {
                            temp += "'" + Convert.ToString(i) + "',";
                        }
                        temp = temp.Substring(0, temp.Length - 1);
                        stblist.Append(string.Format(strinsert, temp) + ";");
                        temp = "";
                        sqlite.ExecuteTran(stblist.ToString(), out str);
                        stblist = new StringBuilder();
                        Console.WriteLine(count.ToString());
                    }

                }
            }

        }




        static void insert(Dictionary<string, SortedList<string, List<string>>> listnew)
        {
            SqliteAccess sqlite = new SqliteAccess("E:\\临时测试程序\\GetDBData\\DataBase3.db3");
            //SqliteAccess sqlite = new SqliteAccess("C:\\Users\\ysjsfw\\Desktop\\DataBase3.db3");
            string strlist = "";
            StringBuilder stblist = new StringBuilder();
            string str = "";
            int count = 0;
            int countall = 0;
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
            foreach (var sort in listnew)
            {
                foreach (var item in sort.Value)
                {
                    foreach (var i in item.Value)
                    {
                        temp += "'" + Convert.ToString(i) + "',";
                    }
                    temp = temp.Substring(0, temp.Length - 1);
                    stblist.Append(string.Format(strinsert, temp) + ";");
                    temp = "";
                    count++;

                    if (count == 3000)
                    {
                        sqlite.ExecuteTran(stblist.ToString(), out str);
                        stblist = new StringBuilder();
                        count = 0;
                        countall += 3000;
                        Console.WriteLine(countall.ToString());
                    }
                }
            }
            Console.WriteLine("countall:" + countall.ToString());
            Console.WriteLine("count:" + count.ToString());
            if (stblist.Length != 0)
            {
                sqlite.ExecuteTran(stblist.ToString(), out str);
            }
        }


        static void newinsert(Dictionary<string, SortedList<string, List<string>>> listnew)
        {
            SqliteAccess sqlite = new SqliteAccess("E:\\临时测试程序\\GetDBData\\DataBase3.db3");
            //SqliteAccess sqlite = new SqliteAccess("C:\\Users\\ysjsfw\\Desktop\\DataBase3.db3");
            //string DBLINK = @"Data Source=E:\\临时测试程序\\GetDBData\\DataBase3.db3;Pooling=true;FailIfMissing=false";
            string DBLINK = @"Data Source=C:\\Users\\ysjsfw\\Desktop\\DataBase.db3;Pooling=true;FailIfMissing=false";
            StringBuilder stblist = new StringBuilder();
            int count = 0;
            int countall = 0;
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
            foreach (var sort in listnew)
            {
                foreach (var item in sort.Value)
                {
                    foreach (var i in item.Value)
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
                        countall += 5000;
                        Console.WriteLine(countall.ToString());
                    }
                }
            }
            Console.WriteLine("countall:" + countall.ToString());
            Console.WriteLine("count:" + count.ToString());
            if (list.Count > 0)
            {
                sqlite.insertQuick(DBLINK, list);
            }
        }
    }
}
