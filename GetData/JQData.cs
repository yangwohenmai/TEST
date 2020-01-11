using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GetData
{
    public class JQData
    {
        public static void get()
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
                    code = "300083.XSHG",
                    unit = "1d",
                    date= "2019-11-10",    
                    end_date= "2020-01-06",
                    fq_ref_date = "2020-01-10"
                }
                //{
                //    method= "get_price",
                //    token= token1,
                //    code= "600000.XSHG",
                //    count= 10,
                //    unit= "1d",
                //    //end_date= "2018-07-21",
                //    fq_ref_date= "2018-07-21"
                //}
                );
                var bodyContent = new StringContent(body, Encoding.UTF8, "application/json");
                //POST请求并等待结果
                result = client.PostAsync(url, bodyContent).Result;
                //code,display_name,name,start_date,end_date,type,parent
                //502050.XSHG,上证50B,SZ50B,2015-04-27,2200-01-01,fjb,502048.XSHG
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
                    Console.WriteLine(Convert.ToDateTime(i[0]).ToString("yyyyMMdd"));
                    stock.open = Convert.ToDecimal(i[1]);
                    stock.close = Convert.ToDecimal(i[2]);
                    stock.hight = Convert.ToDecimal(i[3]);
                    stock.low = Convert.ToDecimal(i[4]);
                    stocklist.Add(Convert.ToDateTime(i[0]).ToString("yyyyMMdd"),stock);
                }
                
            }
            var r1 = (2 * stocklist.Last().Value.close + stocklist.Last().Value.hight + stocklist.Last().Value.low) / 3;

            //VAR1:=(2*CLOSE+HIGH+LOW)/3;
            //VAR2:= EMA(EMA(EMA(VAR1, 3), 3), 3);
            //J: (VAR2 - REF(VAR2, 1)) / REF(VAR2, 1) * 100
            //D: MA(J, 2);
            //K: MA(J, 1);

            Console.WriteLine(securityInfo);
            Console.ReadLine();
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
