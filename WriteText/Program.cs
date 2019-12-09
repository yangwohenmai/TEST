using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteText
{
    class Program
    {
        static void Main(string[] args)
        {
            Write_txt("444");
            AddLog("test");
        }

        public static void AddLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                string logFile = "D:\\logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                if (File.Exists(logFile))
                {
                    sw = File.AppendText(logFile);
                }
                else
                {
                    sw = File.CreateText(logFile);
                }

                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + message + "\r\n");
            }
            catch
            {

            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }


        public static void creatTxt(DataTable dt)
        {
            string fn = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + "PO014" + ".txt";
            FileStream fs = new FileStream("D:\\" + fn, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter strmWriter = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));//存入到文本文件中 

            string str = ",";

            foreach (DataRow dr in dt.Rows)
            {
                strmWriter.Write("1");
                strmWriter.Write(str);
                strmWriter.Write("2");
                strmWriter.Write(str);
                strmWriter.WriteLine(); //换行
            }
            strmWriter.Flush();
            strmWriter.Close();
        }

        public static void Write_txt(string log)
        {
            string logFileName = Path.Combine("c:\\", DateTime.Now.ToString("yyyyMMdd") + ".log");

            File.AppendAllText(logFileName, DateTime.Now.ToString() + " ");
            File.AppendAllText(logFileName, log);
            File.AppendAllText(logFileName, Convert.ToChar(13).ToString());
            File.AppendAllText(logFileName, Convert.ToChar(10).ToString());

        }

    }
}
