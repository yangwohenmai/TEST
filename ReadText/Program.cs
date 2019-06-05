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
