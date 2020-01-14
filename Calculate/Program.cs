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
            BLJJ();
        }

        /// <summary>
        /// 捕捞季节
        /// VAR1:=(2*CLOSE+HIGH+LOW)/3;
        /// VAR2:= EMA(EMA(EMA(VAR1, 3), 3), 3);
        /// J: (VAR2 - REF(VAR2, 1)) / REF(VAR2, 1) * 100;
        /// D: MA(J, 2);
        /// K: MA(J, 1);
        /// </summary>
        public static void BLJJ(string code = null, string period = null, string type = null, string date = null)
        {
            string EndDate = date == null ? DateTime.Now.ToString("yyyy-MM-dd"): Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            string BeginDate = Convert.ToDateTime(EndDate).AddDays(-1450).ToString("yyyy-MM-dd");
            code = code == null ? "300083" : code;
            type = type == null ? "1d" : type;
            period = period == null ? "30" : period;

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("code", code + ".XSHG");
            dic.Add("unit", type);
            dic.Add("date", BeginDate);
            dic.Add("end_date", EndDate);
            dic.Add("fq_ref_date", EndDate);
            SortedList<string, stock> stocklist = JQData.get_price_period(dic);
            List<decimal> testValues = new List<decimal>();
            //(2*CLOSE+HIGH+LOW)/3
            foreach (var item in stocklist)
            {
                testValues.Add((2 * item.Value.close + item.Value.hight + item.Value.low) / 3);
            }
            //J: (VAR2 - REF(VAR2, 1)) / REF(VAR2, 1) * 100
            List<decimal> pre_ema_list = Function.EMA(Function.EMA(Function.EMA(testValues, 3), 3), 3);
            List<decimal> DList = new List<decimal>();
            List<decimal> KList = new List<decimal>();
            if (pre_ema_list.Count > Convert.ToInt32(period))
            {
                for (int i = 0; i < Convert.ToInt32(period); i++)
                {
                    decimal J1 = (Function.REF(pre_ema_list, i) - Function.REF(pre_ema_list, i + 1)) / Function.REF(pre_ema_list, i + 1) * 100;
                    decimal J2 = (Function.REF(pre_ema_list, i + 1) - Function.REF(pre_ema_list, i + 2)) / Function.REF(pre_ema_list, i + 2) * 100;
                    DList.Add((J1 + J2) / 2);
                    KList.Add(J1);
                }
            }
            else
            {
                for (int i = 0; i < pre_ema_list.Count; i++)
                {
                    decimal J1 = (Function.REF(pre_ema_list, i) - Function.REF(pre_ema_list, i + 1)) / Function.REF(pre_ema_list, i + 1) * 100;
                    decimal J2 = (Function.REF(pre_ema_list, i + 1) - Function.REF(pre_ema_list, i + 2)) / Function.REF(pre_ema_list, i + 2) * 100;
                    DList.Add(J1 + J2);
                    KList.Add(J1);
                }
            }
        }

        //MID:=(3*CLOSE+LOW+OPEN+HIGH)/6;
        //AA:(20*MID+19*REF(MID,1)+18*REF(MID,2)+17*REF(MID,3)+16
        //*REF(MID,4)+15*REF(MID,5)+14*REF(MID,6)+13*REF(MID,7)+12*REF(MID,8)+11
        //*REF(MID,9)+10*REF(MID,10)+9*REF(MID,11)+8*REF(MID,12)+7*REF(MID,13)+6*REF(MID,14)+5
        //*REF(MID,15)+4*REF(MID,16)+3*REF(MID,17)+2*REF(MID,18)+REF(MID,20))/211;
        //BB:MA(AA,5);
        public void ZLZZ(string code = null, string period = null, string type = null, string date = null)
        {
            string EndDate = date == null ? DateTime.Now.ToString("yyyy-MM-dd") : Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            string BeginDate = Convert.ToDateTime(EndDate).AddDays(-1450).ToString("yyyy-MM-dd");
            code = code == null ? "300083" : code;
            type = type == null ? "1d" : type;
            period = period == null ? "30" : period;

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("code", code + ".XSHG");
            dic.Add("unit", type);
            dic.Add("date", BeginDate);
            dic.Add("end_date", EndDate);
            dic.Add("fq_ref_date", EndDate);
            SortedList<string, stock> stocklist = JQData.get_price_period(dic);
            List<decimal> Values = new List<decimal>();
            //(2*CLOSE+HIGH+LOW)/3
            foreach (var item in stocklist)
            {
                Values.Add((3 * item.Value.close + item.Value.hight + item.Value.low + item.Value.open) / 6);
            }
            //(20 * Function.REF(Values, 0) + 19 * Function.REF(Values, 1) + 18 * Function.REF(Values, 2) + 17 * Function.REF(Values, 3) +
            //    16 * Function.REF(Values, 4) + 15 * Function.REF(Values, 5) + 14 * Function.REF(Values, 6) + 13 * Function.REF(Values, 7) +
            //    12 * Function.REF(Values, 8) + 11 * Function.REF(Values, 9) + 10 * Function.REF(Values, 10) + 9 * Function.REF(Values, 11) +
            //    8 * Function.REF(Values, 12) + 7 * Function.REF(Values, 13) + 6 * Function.REF(Values, 14) + 5 * Function.REF(Values, 15) +
            //    4 * Function.REF(Values, 16) + 3 * Function.REF(Values, 17) + 2 * Function.REF(Values, 18) + Function.REF(Values, 20)) / 211 == 1
        }
    }
}
