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
            GetCommonConfig();
            writeXML("123");
        }



        /// <summary>
        /// 读文件
        /// </summary>
        /// <returns></returns>
        public static bool GetCommonConfig()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlNodeList = null;
            XmlNode xmlNode = null;
            string str_Temp = string.Empty;

            //XML配置文件路径
            string str_ConfigFilePath = "E:\\MyGit\\TEST\\DoXML\\bin\\Debug\\config.xml";
            if (!File.Exists(str_ConfigFilePath))
            {
                return false;
            }

            //XML配置部分
            try
            {
                xmlDoc.Load(str_ConfigFilePath);

                #region 加载配置
                xmlNode = xmlDoc.SelectSingleNode("/Root/CommConfig/Sftp");
                if (xmlNode != null)
                {
                    string Name = xmlNode.SelectSingleNode("Name").InnerText.Trim();
                    string FtpUri = xmlNode.SelectSingleNode("FtpUri").InnerText.Trim();
                    string FtpPort = xmlNode.SelectSingleNode("FtpPort").InnerText.Trim();
                    string FtpUserID = xmlNode.SelectSingleNode("FtpUserID").InnerText.Trim();
                    string FtpPassword = xmlNode.SelectSingleNode("FtpPassword").InnerText.Trim();
                    bool IsEncrypt = xmlNode.SelectSingleNode("IsEncrypt").InnerText.Trim().ToLower() == "true" ? true : false;
                }
                else
                {
                    return false;
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                xmlDoc = null;
                xmlNodeList = null;
                str_Temp = null;
            }
        }





        /// <summary>
        /// 写XML文件
        /// </summary>
        /// <param name="strTimeStamp"></param>
        /// <returns></returns>
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
