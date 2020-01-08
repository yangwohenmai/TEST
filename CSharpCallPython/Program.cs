using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCallPython
{
    class Program
    {
        static void Main(string[] args)
        {
            CallCmd.RunPythonScript("",new List<string>());

            #region python2 部分
            #region 调用python脚本、传参
            ScriptEngine pyEngine = Python.CreateEngine();//创建Python解释器对象
            dynamic py = pyEngine.ExecuteFile(@"test1.py");//读取脚本文件
            int[] array = new int[4] { 1, 2, 3, 4 };
            string dd = py.mainF(array);
            //dd = py.mainF("12345");//调用脚本文件中对应的函数名称
            Console.WriteLine(dd);
            Console.ReadLine();
            #endregion

            #region 动态执行输入的python语句
            ScriptEngine pyEngine1 = Python.CreateEngine();//创建一个Python引擎
            string str = "import sys;print(sys.path);";
            dynamic da = pyEngine1.CreateScriptSourceFromString(str);//读取脚本源码字符串
            da.Execute();//执行脚本;winForm程序中执行结果会在输出中显示;控制台程序中执行结果会显示在控制台中
            string a = Console.ReadLine();
            #endregion
            #endregion


            #region Python3部分

            Python3_With_Para();
            Python3_Without_Para();
            
            #endregion

        }

        /// <summary>
        /// 执行拼接的python3脚本
        /// </summary>
        public static void Python3_With_Para()
        {
            string Path = "E:\\MyGit\\TEST\\CSharpCallPython\\bin\\Debug";//脚本文件路径
            string Filename = "test2";//执行脚本文件名称
            string functionname = "add(2,4)";//脚本中要调用的方法
            //1.调用现有的python脚本
            string cmd = string.Format("-c \"import sys;sys.path.append('{0}');import {1};print({1}.{2})\"", Path, Filename, functionname);
            //2.直接动态传入要执行的sql脚本
            cmd = string.Format("-c \"import sys;print(sys.path);a=2;b=3;c=a+b;print('result='+str(c));\"");

            //Console.WriteLine("请输入");
            //string input = Console.ReadLine();
            //cmd = string.Format("-c \""+input+"\"");

            string result = CallCmd.run_cmd("python.exe", cmd);
            Console.WriteLine(result);
            Console.ReadKey();

            #region 上述python脚本内容
            //import sys
            //def add(a, b):
            //    return a + b
            #endregion
            
        }

        /// <summary>
        /// 使用cmd 执行python3脚本 可传参
        /// </summary>
        public static void Python3_Without_Para()
        {
            string path = "E:\\MyGit\\TEST\\CSharpCallPython\\bin\\Debug\\test3.py";
            string para1 = "\"Form C#:\"";
            string para2 = "\"Form C++++:\"";
            string strcmd = string.Format("{0} {1} {2}", path, para1, para2);
            string cmdresult = CallCmd.run_cmd("python.exe", strcmd);
            Console.WriteLine(cmdresult);
            Console.ReadKey();

            #region 上述python脚本内容
            //import sys
            //def add(a, b):
            //    return a + b
            //if __name__ == "__main__":
            //    print(sys.path)
            //    print(sys.argv[1])
            //    print(sys.argv[2])
            //    print(add(len(sys.argv[1]), len(sys.argv[2])))
            //    print("hello python")
            #endregion
            
        }

        

        

    }
}
