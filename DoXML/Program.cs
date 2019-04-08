using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DoXML
{
    class Program
    {
        static void Main(string[] args)
        {

            writeXML("123");
        }

         private static  bool writeXML(string strTimeStamp)
        {
            bool bool_Result = false;

            if (string.IsNullOrEmpty(strTimeStamp))
            {
                return bool_Result;
            }
            //设置时间戳
            XmlDocument xmlDoc = new XmlDocument();

            if (!File.Exists("E:\\MyGit\\TEST\\DoXML\\bin\\Debug\\config.xml"))
            {
                return bool_Result;
            }

            XmlNode envNode = null;

            try
            {
                xmlDoc.Load("E:\\MyGit\\TEST\\DoXML\\bin\\Debug\\config.xml");

                //时间戳交替

                envNode = xmlDoc.SelectSingleNode("/Root/UserList/User/UserName");
                

                if (envNode != null)
                {
                    envNode.InnerText = strTimeStamp.Trim();
                }

                //保存配置
                xmlDoc.Save("E:\\MyGit\\TEST\\DoXML\\bin\\Debug\\config.xml");

                bool_Result = true;
            }
            catch (Exception ex)
            {
                //记录日志
                //Log4NetUtil.Error(this, "SetRsTimeStamp->设置资源时间戳异常:" + ex.ToString());
            }

            return bool_Result;
        }

    }
}
