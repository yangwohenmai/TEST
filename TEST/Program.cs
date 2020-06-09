using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            string Begin = "Begin";
            Console.WriteLine(Begin);
            #region 构造BulkCopy的DataTable
            DataTable BulkCopyData = new DataTable();
            DataRow BulkCopyRow = BulkCopyData.NewRow();
            BulkCopyData.Columns.Add("ITCode2", typeof(string));
            BulkCopyData.Columns.Add("SITCode2", typeof(string));
            BulkCopyData.Columns.Add("NLevel", typeof(Int32));
            BulkCopyData.Columns.Add("EntryDT", typeof(DateTime));
            BulkCopyData.Columns.Add("Flag", typeof(Int32));
            #endregion
            #region 构造失败队列的DataTable
            DataTable ErrorDt = new DataTable();
            DataRow ErrorDtRow = ErrorDt.NewRow();
            ErrorDt.Columns.Add("ITCode2", typeof(string));
            ErrorDt.Columns.Add("SITCode2", typeof(string));
            ErrorDt.Columns.Add("NLevel", typeof(Int32));
            ErrorDt.Columns.Add("EntryDT", typeof(DateTime));
            ErrorDt.Columns.Add("Flag", typeof(Int32));
            #endregion

            DataRow sqlRow = BulkCopyData.NewRow();
            sqlRow["ITCode2"] = "1";
            sqlRow["SITCode2"] = "1";
            sqlRow["NLevel"] = 1;
            sqlRow["EntryDT"] = DateTime.Now;
            sqlRow["Flag"] = 1;
            BulkCopyData.Rows.Add(sqlRow);

            DataRow sqlRow1 = ErrorDt.NewRow();
            sqlRow1["ITCode2"] = "2";
            sqlRow1["SITCode2"] = "2";
            sqlRow1["NLevel"] = 2;
            sqlRow1["EntryDT"] = DateTime.Now;
            sqlRow1["Flag"] = 2;
            ErrorDt.Rows.Add(sqlRow1);

            ErrorDt.Merge(BulkCopyData);


            Dictionary<string, Dictionary<string, string>> dictest = new Dictionary<string, Dictionary<string, string>>();
            dictest["1"] = new Dictionary<string, string>();
            dictest["1"]["3"] = "4";
            dictest["1"]["3"] = "5";
            dictest["1"]["5"] = "6";
            dictest["1"]["6"] = "7";
            dictest["2"] = new Dictionary<string, string>();
            dictest["2"]["1"] = "1";
            dictest["2"]["2"] = "2";
            dictest["2"]["3"] = "3";
            dictest["2"]["4"] = "4";
            dictest["2"]["5"] = "5";
            dictest["2"].Remove(null);





            SortedList<int, string> list = new SortedList<int, string>();
            list.Add(4, "sdf");
            list.Add(2, "sfd");
            list.Add(6, "sdfa");
            list.Add(3, "sfaf");
            int a11 = list.Keys.Min();
            string a12 = list[3];



            try
            {
                ex();
            }
            catch (Exception ex)
            {
                string c = ex.Message;
            }


            #region 时间
            //b1();
            //Console.WriteLine(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString());
            //DateTime a = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            //Console.WriteLine();
            //Console.WriteLine(a.AddMonths(1));
            //DateTime b = a.AddMonths(1);
            //Console.WriteLine(new DateTime(b.AddMonths(1).Year, b.AddMonths(1).Month, DateTime.DaysInMonth(b.AddMonths(1).Year, b.AddMonths(1).Month)).ToString());
            //b = new DateTime(b.AddMonths(1).Year, b.AddMonths(1).Month, DateTime.DaysInMonth(b.AddMonths(1).Year, b.AddMonths(1).Month));
            //b = new DateTime(b.AddMonths(1).Year, b.AddMonths(1).Month, DateTime.DaysInMonth(b.AddMonths(1).Year, b.AddMonths(1).Month));



            //b = new DateTime(b.AddMonths(1).Year, b.AddMonths(1).Month, DateTime.DaysInMonth(b.AddMonths(1).Year, b.AddMonths(1).Month));



            //Console.WriteLine(b);
            //    DateTime LMday = DateTime.Now;
            //    for (int i = 0; i < 10; i++)
            //    {
            //        Console.WriteLine(new DateTime(LMday.AddMonths(1).Year, LMday.AddMonths(1).Month, DateTime.DaysInMonth(LMday.AddMonths(1).Year, LMday.AddMonths(1).Month)).ToString());
            //        LMday = new DateTime(LMday.AddMonths(1).Year, LMday.AddMonths(1).Month, DateTime.DaysInMonth(LMday.AddMonths(1).Year, LMday.AddMonths(1).Month));
            //    }

            //Console.ReadLine();
            //int aaa = 1;
            //int c = 0;
            //Console.WriteLine(aaa);
            //a1(ref aaa);
            //Console.WriteLine(aaa);
            //a1(ref aaa);
            //Console.WriteLine(aaa);
            //a1(ref aaa);
            //Console.WriteLine(aaa);
            //a1(ref aaa);
            //Console.WriteLine(aaa);
            //Console.ReadLine();
            #endregion

            #region 字典赋值
            //Dictionary<string, string> a = new Dictionary<string, string>();
            //Dictionary<string, Dictionary<string, string>> b = new Dictionary<string, Dictionary<string, string>>();
            //a["aa"] = "aa";
            //a["bb"] = "bb";
            //a["cc"] = "cc";
            //a["aa"] = "dd";
            //b["aa"] = a;
            //b["bb"] = a;
            //b = new Dictionary<string, Dictionary<string, string>>();
            //b["aa"] = a;



            //b["aa"]["dd"] = "dd";
            //b["bb"]["ee"] = "ee";
            //b["aa"]["dd"] = "ff";
            //a.Add("aa", "aa");
            #endregion

            #region 时间转换
            //Console.WriteLine(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime("17:00:00").ToString("HH:mm:ss")));
            //Convert.ToDateTime("17:00:00").ToString("HH:mm:ss");
            //MessageBoxs.Show(Convert.ToDateTime("17:00:00").ToString("HH:mm:ss"));
            //Console.WriteLine(Convert.ToDateTime("2010-02-03 14:00:00").ToString());
            //Console.WriteLine(DateTime.Now.ToString());
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime("15:00:00").ToString("HH:mm:ss"));
            //Console.WriteLine(DateTime.Now);

            //if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime("14:00:00").ToString("HH:mm:ss")) > DateTime.Now)
            //    Console.WriteLine("S");
            //else
            //    Console.WriteLine("B");
            //Console.WriteLine(DateTime.Now.Hour);
            //Console.ReadLine();
            #endregion

            #region try catch
            try
            {
                for (int a1 = 1; a1 < 10; a1++)
                {
                    try
                    {
                        string a = null;
                        a.ToString();

                    }
                    catch
                    {
                        try
                        {
                            string b1 = null;
                            b1.ToString();
                        }
                        catch
                        {
                            throw;
                        }
                        throw;
                    }
                }
                    
            }
            catch
            {
                int aa = 1;
            }
            #endregion






            Console.ReadLine();

            




        }




        public static void ex()
        {
            int a = 0;
            int b = 10 / a;
        }



    }

   

}
