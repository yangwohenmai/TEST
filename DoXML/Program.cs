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
            CreateXML("");
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
            }

            return bool_Result;
        }

        public static void CreateXML(string xmlName)
        {
            //通过代码创建XML文档
            //1、引用命名空间   System.Xml
            //2、创建一个 xml 文档
            XmlDocument xml = new XmlDocument();
            //3、创建一行声明信息，并添加到 xml 文档顶部
            XmlDeclaration decl = xml.CreateXmlDeclaration("1.0", "utf-8", null);
            xml.AppendChild(decl);

            //4、创建根节点
            XmlElement rootEle = xml.CreateElement("Sftp");
            xml.AppendChild(rootEle);
            //5、创建子结点|属性：信息
            XmlElement childEle = xml.CreateElement("info");
            rootEle.AppendChild(childEle);

            XmlElement c2Ele = xml.CreateElement("password");
            c2Ele.InnerText = "1";
            childEle.AppendChild(c2Ele);
            c2Ele = xml.CreateElement("Name");
            c2Ele.InnerText = "0";
            childEle.AppendChild(c2Ele);


            //6、创建子节点|属性：FTP信息
            childEle = xml.CreateElement("FtpInfo");
            rootEle.AppendChild(childEle);

            c2Ele = xml.CreateElement("FtpUri");
            c2Ele.InnerText = "123";
            childEle.AppendChild(c2Ele);

            c2Ele = xml.CreateElement("FtpPort");
            c2Ele.InnerText = "234";
            childEle.AppendChild(c2Ele);

            c2Ele = xml.CreateElement("FtpUserID");
            c2Ele.InnerText = "345";
            childEle.AppendChild(c2Ele);

            c2Ele = xml.CreateElement("FtpPassword");
            c2Ele.InnerText = "456";
            childEle.AppendChild(c2Ele);

            xml.Save("E:\\MyGit\\TEST\\DoXML\\bin\\Debug\\config1.xml");

        }


    }
}
