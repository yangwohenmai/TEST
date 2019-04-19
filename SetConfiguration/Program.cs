using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            NameValueCollection nc = (NameValueCollection)ConfigurationSettings.GetConfig("TestGroup/Test");
            NameValueCollection nc1 = (NameValueCollection)ConfigurationSettings.GetConfig("TestGroup/Test1");
            NameValueCollection nc0 = (NameValueCollection)ConfigurationSettings.GetConfig("TestGroup");
            for (int i = 0; i < nc.AllKeys.Length; i++ )
                Console.WriteLine(nc.AllKeys[i].ToString() + " " + nc[nc.AllKeys[i].ToString()]);

            for (int i = 0; i < nc1.AllKeys.Length; i++)
                Console.WriteLine(nc1.AllKeys[i].ToString() + " " + nc1[nc1.AllKeys[i].ToString()]);




            NameValueCollection neww = (NameValueCollection)ConfigurationSettings.GetConfig("newGroup/new");
            NameValueCollection neww1 = (NameValueCollection)ConfigurationSettings.GetConfig("newGroup/new1");
            for (int i = 0; i < neww.AllKeys.Length; i++)
                Console.WriteLine(neww.AllKeys[i].ToString() + " " + neww[neww.AllKeys[i].ToString()]);

            for (int i = 0; i < neww1.AllKeys.Length; i++)
                Console.WriteLine(neww1.AllKeys[i].ToString() + " " + neww1[neww1.AllKeys[i].ToString()]);

            Console.ReadLine();
        }
    }
}
