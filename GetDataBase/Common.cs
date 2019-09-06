using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataBase
{
    class Common
    {
        public static void AddLog(string filename, string message)
        {
            //StreamWriter sw = null;
            string logFile = "D:\\HKHDQUOTE\\" + filename + ".txt";
            //FileStream fs = new FileStream(logFile, FileMode.Create, FileAccess.ReadWrite);
            //StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
            try
            {
                File.AppendAllText(logFile, message + "\r\n", Encoding.GetEncoding("GB2312"));
                //if (File.Exists(logFile))
                //{
                //    File.AppendAllText(logFile, message + "\r\n", Encoding.GetEncoding("GB2312"));
                //    //File.AppendAllLines
                //}
                //else
                //{
                //    //File.CreateText(logFile);
                //    File.AppendAllText(logFile, message + "\r\n", Encoding.GetEncoding("GB2312"));
                //}
                //sw.WriteLine(message);
                //sw.WriteLine(message + "\r\n");
            }
            catch (Exception ex)
            {
                string a = ex.ToString();

            }
            finally
            {
                //if (sw != null)
                //{
                //sw.Close();
                //}
            }
        }
    }
}
