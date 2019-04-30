using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ResetEvent
{
    class Program
    {
        static EventWaitHandle _tollStation = new ManualResetEvent(false);//改为ManualResetEvent,车闸默认关闭
        static EventWaitHandle _tollStationAuto = new AutoResetEvent(true);//车闸默认关闭
        static void Main(string[] args)
        {
            AutoResetEventTest();
            //ManualResetEventTest();
        }


        #region AutoResetEventTest
        public static void AutoResetEventTest()
        {
            new Thread(Car3).Start();//车辆3
            Thread.Sleep(1000);
            new Thread(Car4).Start();//车辆4
            //_tollStationAuto.Set();
            Console.ReadKey();
        }


        static void Car3()
        {
            _tollStationAuto.WaitOne();//等待开启车闸，即_tollStation.Set();
            Console.WriteLine("车辆3，顺利通过。");
        }

        static void Car4()
        {
            _tollStationAuto.WaitOne();
            Console.WriteLine("车辆4，顺利通过。！");
        }
        #endregion



        #region ManualResetEventTest
        public static void ManualResetEventTest()
        {
            new Thread(Car1).Start();//车辆1
            new Thread(Car2).Start();//车辆2

            _tollStation.Set();//开启车闸，放行
            Timer timer = new Timer(CloseDoor, null, 0, 2000);//2秒后关闭车闸

            Console.ReadLine();
            //再次打开车闸
            _tollStation.Set();
            Console.ReadLine();
        }

        static void Car1()
        {
            _tollStation.WaitOne();//等待开启车闸，即_tollStation.Set();
            Console.WriteLine("车辆1，顺利通过。");
        }

        static void Car2()
        {
            Thread.Sleep(3000);//睡眠3秒
            _tollStation.WaitOne();//当醒来后车闸已经被关闭
            Console.WriteLine("车辆2，顺利通过。");//所以车辆2不会被通过
        }

        /// <summary>
        /// 2秒后关闭车闸
        /// </summary>
        static void CloseDoor(object o)
        {
            _tollStation.Reset();//关闭车闸
        }
        #endregion
    }
}
