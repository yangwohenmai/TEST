using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized; 

namespace GetConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConfigurationManager.GetSection


            //访问配置节sampleSection2  


            NameValueCollection nc =(NameValueCollection)ConfigurationSettings.GetConfig("TestGroup/Test");
            string aa = nc["Hello"].ToString();
            string aa1 = nc["Hello1"].ToString();
            string aa2 = nc["Hello2"].ToString();
            string aa3 = nc["Hello3"].ToString();
            string aa4 = nc["Hello4"].ToString();

            //NameValueCollection nc1 = (NameValueCollection)ConfigurationSettings.GetConfig("TestGroup1/Test1");
            //string aaa = nc1["Hello"].ToString();
            //string aaa1 = nc1["Hello11"].ToString();
            //string aaa2 = nc1["Hello22"].ToString();
            //string aaa3 = nc1["Hello33"].ToString();
            //string aaa4 = nc1["Hello44"].ToString();

            NameValueCollection nc1 = (NameValueCollection)ConfigurationSettings.GetConfig("Test2");
            string aaa = nc1["Hello"].ToString();
            string aaa1 = nc1["Hello11"].ToString();
            string aaa2 = nc1["Hello22"].ToString();
            string aaa3 = nc1["Hello33"].ToString();
            string aaa4 = nc1["Hello44"].ToString();




            IDictionary IDTest2 =(IDictionary)ConfigurationSettings.GetConfig("sampleSection2");  
  
            string[] keys=new string[IDTest2.Keys.Count];  
  
            string[] values=new string[IDTest2.Values.Count];
            //string kk = IDTest2.v.["key"].ToString();
            //string a = IDTest2.Keys.
  
            IDTest2.Keys.CopyTo(keys,0);  
  
            IDTest2.Values.CopyTo(values,0);  
  
            //MessageBox.Show(keys[0]+" "+values[0]); //输出 
        }
    }
}
