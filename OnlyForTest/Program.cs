using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyForTest
{
    class Program
    {
        static void Main(string[] args)
        {
        	
        	DateTime a1,a2;
            DateTime a = IntToDate(20180430);
        	a1 = a.AddMonths(-1);
        	//a1 = IntToDate(GetLastDayOfThisMonth(20180205)).AddMonths(-1) ;
            a2 = IntToDate(GetLastDayOfThisMonth(DateToInt(a1)));


            string dfg = DateTime.Now.ToString("yyyyMMdd");


            DateTime d1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            DateTime d2 = d1.AddMonths(1).AddDays(-1);

            d2 = IntToDate(20180323).AddDays(1 - IntToDate(20180123).Day).AddDays(-1);


        }
        
        
        
        public static int GetLastDayOfThisMonth(int date)
        {
            DateTime lastDayOfThisMonth = IntToDate(date);

            int year = lastDayOfThisMonth.Year;
            int month = lastDayOfThisMonth.Month;

            lastDayOfThisMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            return DateToInt(lastDayOfThisMonth);

        }
        
        public static int DateToInt(DateTime date)
        {
            int dt = 19000101;
            try
            {
                dt = int.Parse(date.ToString("yyyyMMdd"));
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        
        
        
        
        public static DateTime IntToDate(int date)
        {
            return StringToDateTime(date.ToString(CultureInfo.InvariantCulture));
        }

        public static DateTime StringToDateTime(string date)
        {
            DateTime dt = new DateTime(1900, 01, 01);
            try
            {
                dt = DateTime.ParseExact(date.Trim(), "yyyyMMdd", null);
            }
            catch (Exception ex)
            {
            }
            return dt;
        }


    }
}
