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
            return stocklist;
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
