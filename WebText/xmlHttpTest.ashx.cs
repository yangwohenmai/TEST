using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebText
{
    public class xmlHttpTest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            string methodName1 = System.Web.HttpContext.Current.Request.QueryString["methodName"];
            string methodName = context.Request["methodName"];
            string methodNameForm = System.Web.HttpContext.Current.Request.Form["methodName"];
            string str1 = System.Web.HttpContext.Current.Request.Form["str1"];
            string str2 = context.Request["str2"];
            Uri Project_Id = request.UrlReferrer;
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}