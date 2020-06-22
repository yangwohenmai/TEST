using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace DoXML
{
    class xmllog
    {
        static void Main1(string[] args)
        {
            createXml();//创建文件和添加基本数据，便于之后操作实验
            //addItem();//追加数据
            readtext();//读取数据
            //updatexml();//更新数据
            //delnode();//删除数据
        }

        #region Xml
        private static void createXml()
        {
            XmlTextWriter writer = new XmlTextWriter("titles.xml", null);
            //使用自动缩进便于阅读
            writer.Formatting = Formatting.Indented;
            //写入根元素
            writer.WriteStartElement("items");
            writer.WriteStartElement("item");
            //写入属性及属性的名字
            writer.WriteAttributeString("类别", "文学");
            writer.WriteAttributeString("品质", "优");
            //加入子元素
            writer.WriteElementString("title", "毛著");
            writer.WriteElementString("author", "毛泽东");
            writer.WriteElementString("price", "10.0");
            //关闭根元素，并书写结束标签
            writer.WriteEndElement();
            writer.WriteEndElement();
            //将XML写入文件并且关闭XmlTextWriter
            writer.Close();
        }
        private static void addItem()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("titles.xml");
            XmlNode root = xmlDoc.SelectSingleNode("items");
            XmlElement xe1 = xmlDoc.CreateElement("item");
            xe1.SetAttribute("类别", "张三");
            xe1.SetAttribute("品质", "老师");
            XmlElement xmlsub1 = xmlDoc.CreateElement("title");
            xmlsub1.InnerText = "wahahha";
            xe1.AppendChild(xmlsub1);
            XmlElement xmlsub2 = xmlDoc.CreateElement("author");
            xmlsub2.InnerText = "三毛";
            xe1.AppendChild(xmlsub2);
            XmlElement xmlsub3 = xmlDoc.CreateElement("Price");
            xmlsub3.InnerText = "15.00";
            xe1.AppendChild(xmlsub3);
            root.AppendChild(xe1);
            xmlDoc.Save("titles.xml");
        }
        private static void readtext()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("titles.xml");
            XmlNode xn = xmlDoc.SelectSingleNode("items");

            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xnf in xnl)
            {
                XmlElement xe = (XmlElement)xnf;
                Console.Write("类别  " + xe.GetAttribute("类别") + "     品质  ");//显示属性值
                Console.WriteLine(xe.GetAttribute("品质"));
                XmlNodeList xnf1 = xe.ChildNodes;
                foreach (XmlNode xn2 in xnf1)
                {
                    Console.WriteLine(xn2.InnerText);//显示子节点点文本
                }
                Console.WriteLine();
            }
        }
        private static void updatexml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("titles.xml");
            XmlNode xns = xmlDoc.SelectSingleNode("items");
            XmlNodeList xnl = xns.ChildNodes;
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.GetAttribute("类别") == "文学")
                {
                    xe.SetAttribute("类别", "娱乐");

                }
                else
                {
                    xe.SetAttribute("类别", "文学");
                }
                XmlNodeList xnl2 = xe.ChildNodes;
                foreach (XmlNode xn2 in xnl2)
                {
                    XmlElement xe2 = (XmlElement)xn2;
                    if (xe2.Name == "price")
                    {
                        if (xe2.InnerText == "10.00")
                            xe2.InnerText = "15.00";
                        else
                            xe2.InnerText = "10.00";
                    }
                    //break;
                }
                //break;
            }
            xmlDoc.Save("titles.xml");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            readtext();
        }
        private static void delnode()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("titles.xml");
            XmlNodeList xnl = xmlDoc.SelectSingleNode("items").ChildNodes;
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.GetAttribute("类别") == "文学")
                {
                    xe.RemoveAttribute("类别");//删除genre属性
                }
                else if (xe.GetAttribute("类别") == "娱乐")
                {
                    xe.RemoveAll();//删除该节点的全部内容
                }
            }
            xmlDoc.Save("titles.xml");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            readtext();
        }
        #endregion


    }
}



