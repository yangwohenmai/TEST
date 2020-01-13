using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    class Program
    {
        static void Main(string[] args)
        {
            BLJJ("");
            //Function.MA(new List<decimal> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 10);
        }

        /// <summary>
        /// VAR1:=(2*CLOSE+HIGH+LOW)/3;
        /// VAR2:= EMA(EMA(EMA(VAR1, 3), 3), 3);
        /// J: (VAR2 - REF(VAR2, 1)) / REF(VAR2, 1) * 100
        /// D: MA(J, 2);
        /// K: MA(J, 1);
        /// </summary>
        public static void BLJJ(string code, string type = "1d", string date = null)
        {
            string EndDate = date == null ? DateTime.Now.ToString("yyyy-MM-dd"): Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            string BeginDate = Convert.ToDateTime(EndDate).AddDays(-1450).ToString("yyyy-MM-dd");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("code", "300083.XSHG");
            dic.Add("unit", type);
            dic.Add("date", BeginDate);
            dic.Add("end_date", EndDate);
            dic.Add("fq_ref_date", EndDate);
            SortedList<string, stock> stocklist = JQData.get_price_period(dic);
            List<decimal> testValues = new List<decimal>();
            foreach (var item in stocklist)
            {
                decimal per_ema = (2 * item.Value.close + item.Value.hight + item.Value.low) / 3;
                testValues.Add(per_ema);
            }
            List<decimal> pre_ema_list = Function.EMA(Function.EMA(Function.EMA(testValues, 3), 3), 3);
            decimal J1 = (Function.REF(pre_ema_list, 0) - Function.REF(pre_ema_list, 1)) / Function.REF(pre_ema_list, 2) * 100;
            decimal J2 = (Function.REF(pre_ema_list, 1) - Function.REF(pre_ema_list, 2)) / Function.REF(pre_ema_list, 2) * 100;
            decimal D = (J1 + J2) / 2;
            decimal K = J1;

        }
    }
}
