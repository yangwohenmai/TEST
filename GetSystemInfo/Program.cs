using System;
using System.Runtime.InteropServices;

namespace GetSystemInfo
{
    class Program
    {
        const int STD_INPUT_HANDLE = -10;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const uint ENABLE_INSERT_MODE = 0x0020;
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int hConsoleHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint mode);

        public static void DisbleQuickEditMode()
        {
            IntPtr hStdin = GetStdHandle(STD_INPUT_HANDLE);
            uint mode;
            GetConsoleMode(hStdin, out mode);
            mode &= ~ENABLE_QUICK_EDIT_MODE;//移除快速编辑模式
            mode &= ~ENABLE_INSERT_MODE;      //移除插入模式
            SetConsoleMode(hStdin, mode);
        }

        public static void ShowSystem()
        {
            var os = Environment.OSVersion;
            Console.WriteLine("Current OS Information:\n");
            Console.WriteLine("Platform: {0:G}", os.Platform);
            Console.WriteLine("Version String: {0}", os.VersionString);
            Console.WriteLine("Version Information:");
            Console.WriteLine("   Major: {0}", os.Version.Major);
            Console.WriteLine("   Minor: {0}", os.Version.Minor);
            Console.WriteLine("Service Pack: '{0}'", os.ServicePack);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ShowSystem();
            CloseEdit();
            Console.ReadLine();

        }


        static void CloseEdit()
        {
            //获取操作系统平台的PlatformID枚举值
            PlatformID _PlatformID = Environment.OSVersion.Platform;
            Console.WriteLine(_PlatformID);
            if (!(_PlatformID != PlatformID.Win32NT))
            {
                DisbleQuickEditMode();
            }
            //Console.WriteLine(res); //打印当前系统判断结果
        }
    }
}
