using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Calculate
{
    public class JQData
    {
        /// <summary>
        /// 获取单只股票数据
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static SortedList<string, stock> get_price_period(Dictionary<string,string> dic)
        {
            var url = "https://dataapi.joinquant.com/apis";
            string securityInfo = string.Empty;
            using (var client = new HttpClient())
            {
                //需要添加System.Web.Extensions
                //生成JSON请求信息
                string json = new JavaScriptSerializer().Serialize(new
                {
                    method = "get_token",
                    mob = "13052089963",
                    pwd = "yangyanan"
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //POST请求并等待结果
                var result = client.PostAsync(url, content).Result;
                //读取返回的TOKEN
                string token1 = result.Content.ReadAsStringAsync().Result;
                string body = new JavaScriptSerializer().Serialize(new
                {
                    method = "get_price_period",
                    token = token1,
                    code = dic["code"],
                    unit = dic["unit"],
                    date= dic["date"],
                    end_date= dic["end_date"],
                    fq_ref_date = dic["fq_ref_date"]
                }
                );
                var bodyContent = new StringContent(body, Encoding.UTF8, "application/json");
                //POST请求并等待结果
                result = client.PostAsync(url, bodyContent).Result;
                securityInfo = result.Content.ReadAsStringAsync().Result;
            }
            var s = securityInfo.Split('\n');
            SortedList<string, stock> stocklist = new SortedList<string, stock>();
            foreach (var item in s)
            {
                var i = item.Split(',');
                if (i[0].ToString() != "date")
                {
                    stock stock = new stock();
                    stock.open = Convert.ToDecimal(i[1]);
                    stock.close = Convert.ToDecimal(i[2]);
                    stock.hight = Convert.ToDecimal(i[3]);
                    stock.low = Convert.ToDecimal(i[4]);
                    stocklist.Add(Convert.ToDateTime(i[0]).ToString("yyyyMMdd"),stock);
                }
            }

            //List<decimal> testValues = new List<decimal>();
            //int i1 = 0;
            //foreach (var item in stocklist)
            //{
            //    i1++;
            //    if (i1 != stocklist.Count)
            //        testValues.Add(item.Value.per_ema);
            //}
            //List<decimal> pre_ema_list = EMA(EMA(EMA(testValues, 3),3),3);
            //decimal J1 = (REF(pre_ema_list, 0) - REF(pre_ema_list, 1)) / REF(pre_ema_list, 2) * 100;
            //decimal J2 = (REF(pre_ema_list, 1) - REF(pre_ema_list, 2)) / REF(pre_ema_list, 2) * 100;
            //decimal D = (J1 + J2) / 2;
            //decimal K = J1;

            //VAR1:=(2*CLOSE+HIGH+LOW)/3;
            //VAR2:= EMA(EMA(EMA(VAR1, 3), 3), 3);
            //J: (VAR2 - REF(VAR2, 1)) / REF(VAR2, 1) * 100
            //D: MA(J, 2);
            //K: MA(J, 1);

            //Console.WriteLine(securityInfo);
            return stocklist;
            //Console.ReadLine();
        }


        /// <summary>
        /// Calculates Exponential Moving Average (EMA) indicator
        /// </summary>
        /// <param name="input">Input signal</param>
        /// <param name="period">Number of periods</param>
        /// <returns>Object containing operation results</returns>
        public static List<decimal> EMA(IEnumerable<decimal> input, int period)
        {
            var returnValues = new List<decimal>();

            decimal multiplier = (decimal)(2.0 / (period + 1));
            decimal initialSMA = input.Take(period).Average();

            returnValues.Add(initialSMA);

            var copyInputValues = input.ToList();

            for (int i = period; i < copyInputValues.Count; i++)
            {
                var resultValue = (copyInputValues[i] - returnValues.Last()) * multiplier + returnValues.Last();

                returnValues.Add(resultValue);
            }

            return returnValues;
        }

        /// <summary>
        /// REF取前N天数据
        /// </summary>
        /// <param name="input"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static decimal REF(List<decimal> input, int period)
        {
            return input[input.Count - period - 1];
        }
    }
    public struct stock
    {
        public decimal open;
        public decimal close;
        public decimal hight;
        public decimal low;
    }
}
