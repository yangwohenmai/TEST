﻿using System;
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

            Console.WriteLine(DateTime.Now.AddDays(-Convert.ToInt32(DateTime.Now.DayOfWeek)));
            


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
    }
}
