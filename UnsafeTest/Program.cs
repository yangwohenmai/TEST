using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnsafeTest
{
    class Program
    {

        unsafe static void Main(string[] args)
        {
            #region 类调用方法1
            CaryData cd = new CaryData();
            Console.WriteLine("改变前: {0}", cd.data);

            fixed (int* p = &cd.data)
            {
                ChangeValue(p);
            }
            Console.WriteLine("改变后: {0}", cd.data);

            Point pt = new Point();
            pt.x = 5;
            pt.y = 6;
            // Pin pt in place:
            fixed (int* p = &pt.x)
            {
                SquarePtrParam(p);
            }
            Console.WriteLine("{0} {1}", pt.x, pt.y);
            Console.ReadLine();
            #endregion

            #region 托管传递非托管
            string s11 = "Hello ";
            string s22 = s11;
            IntPtr p1 = Marshal.StringToHGlobalAnsi(s11);
            IntPtr p12 = Marshal.StringToHGlobalAnsi(s11);
            IntPtr p11 = Marshal.StringToHGlobalAnsi(s22);
            #endregion


            unsafe
            {
                Console.Read();

                string str = "Hello World";
                string str1 = str;
                #region 示例1
                //fixed (char* pStr = str, pStr1 = str)//fixed可以在非托管中定义多个指针，指针离开{}之后就被销毁
                //{
                //    Console.WriteLine("{0:D}", (int)pStr);////输出str的地址，指针pStr指向变量str的地址
                //    char* pStr2;//创建一个新指针
                //    pStr2 = pStr;//把新指针指向变量的地址
                //    Console.WriteLine("{0}", *pStr2);//输出新指针所指向的第一个地址存储内容
                //    for (int i = 0; i < str.Length; i++)//依次打印出指针所指向的地址存储的后续内容
                //    {
                //        Console.Write("{0}", *(++pStr2));
                //    }
                //}
                #endregion


                #region 示例2
                fixed (char* pStr1 = str1)
                {
                    Console.WriteLine("{0:x}", (int)pStr1);
                }
                str += "!!!";
                fixed (char* pStr = str)
                {
                    Console.WriteLine("{0:x}", (int)pStr);
                }
                fixed (char* pStr1 = str1)
                {
                    Console.WriteLine("{0:x}", (int)pStr1);
                }
                #endregion

            }
            Console.ReadLine();

        }
        #region 类调用方法1
        unsafe static void SquarePtrParam(int* p)
        {
            *p += *p;
        }
        unsafe static void ChangeValue(int* pInt)
        {
            *pInt = 23111111;
        }
        #endregion 
    }


    #region 类调用方法1
    class CaryData
    {
        public int data;
    }

    class Point
    {
        public int x, y;
    }
    #endregion

}
