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
                    mob = "13052089963", //mob是申请JQData时所填写的手机号
                    pwd = "yangyanan" //Password为聚宽官网登录密码，新申请用户默认为手机号后6位
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //POST请求并等待结果
                var result = client.PostAsync(url, content).Result;
                //读取返回的TOKEN
                string token1 = result.Content.ReadAsStringAsync().Result;
                string body = new JavaScriptSerializer().Serialize(new
                {
                    method= "get_price",
                    token= token1,
                    code= "600000.XSHG",
                    count= 5000,
                    unit= "1d",
                    //end_date= "2018-07-21",
                    fq_ref_date= "2018-07-21"
                });
                var bodyContent = new StringContent(body, Encoding.UTF8, "application/json");
                //POST请求并等待结果
                result = client.PostAsync(url, bodyContent).Result;
                //code,display_name,name,start_date,end_date,type,parent
                //502050.XSHG,上证50B,SZ50B,2015-04-27,2200-01-01,fjb,502048.XSHG
                securityInfo = result.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine(securityInfo);
            Console.ReadLine();
        }
    }
}
