using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadText
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dt = new DataTable();
            OpenCSVFile(ref dt, "C:\\Users\\admin\\Desktop\\eq_dispara.csv");
            //string str = System.IO.File.ReadAllText("C:\\Users\\admin\\Desktop\\基金实时估值\\CurrentNav.txt");
            FileInfo fi = new FileInfo("C:\\Users\\admin\\Desktop\\基金实时估值\\CurrentNav.txt");
            string a = fi.CreationTime.ToString("yyyyMMdd");
            Console.WriteLine(fi.CreationTime.ToString());
            Console.ReadKey();
        }

        public void read()
        {
            FileStream fs = new FileStream("C:\\Users\\yangzo\\Desktop\\C5S6\\CID.csv", FileMode.Open, FileAccess.Read, FileShare.None);
            //StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("GB2312"));

            DataTable dt = new DataTable();
            dt.Columns.Add("f");
            dt.Columns.Add("s");
            dt.Columns.Add("d");
            string str = "";
            string s = Console.ReadLine();
            while (str != null)
            {

                str = sr.ReadLine();
                string[] xu = new String[3];
                if (str != null)
                {
                    xu = str.Split(',');
                    string ser = xu[0];
                    string dse = xu[1];
                    dt.Rows.Add(xu[0], xu[1], xu[2]);
                }
                //if (ser == s)
                //{
                //    Console.WriteLine(dse); 
                //    break;
                //}
            }
            sr.Close();
            DataTable dtt = new DataTable();
            dtt = dt;
        }


        static private bool OpenCSVFile(ref DataTable mycsvdt, string filepath)
        {
            string strpath = filepath; //csv文件的路径
            try
            {
                int intColCount = 0;
                bool blnFlag = true;

                DataColumn mydc;
                DataRow mydr;

                string strline;
                string[] aryline;
                StreamReader mysr = new StreamReader(strpath, System.Text.Encoding.Default);

                while ((strline = mysr.ReadLine()) != null)
                {
                    aryline = strline.Split(new char[] { ',' });

                    //给datatable加上列名
                    if (blnFlag)
                    {
                        blnFlag = false;
                        intColCount = aryline.Length;
                        int col = 0;
                        for (int i = 0; i < aryline.Length; i++)
                        {
                            col = i;
                            mydc = new DataColumn(aryline[col].ToString());
                            mycsvdt.Columns.Add(mydc);
                        }
                    }

                    //填充数据并加入到datatable中
                    mydr = mycsvdt.NewRow();
                    for (int i = 0; i < intColCount; i++)
                    {
                        mydr[i] = aryline[i];
                    }
                    mycsvdt.Rows.Add(mydr);
                }
                return true;

            }
            catch (Exception e)
            {


                //throw (Stack.GetErrorStack(strpath + "读取CSV文件中的数据出错." + e.Message, "OpenCSVFile("));
                return false;
            }
        }
    }
}
