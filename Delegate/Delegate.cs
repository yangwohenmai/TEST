using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegate
{
    class Delegate
    {
        static void Main(string[] args)
        {
            #region 委托示例1 （事件）
            ///* 项目场景描述：
            //* 我从明天早上开始要早起晨读，室友每天都七点起床晨跑，我告诉他明天起来的时候把我叫醒，并让我晨读
            //* PS：还有个王八犊子非要跟我赌五毛我起不来，还让明天室友也叫他，见证我到底有没有起来
            //*/

            ////首先你们三个得存在吧
            //var 中国好室友 = new 室友();
            //var 我 = new 要晨读的我();
            //var 王八犊子 = new 王八犊子();

            ////我先告诉室友：我要早起晨读
            //中国好室友.叫别人起床该干嘛干嘛 += 我.起床晨读;
            ////王八犊子来搅屎了
            //中国好室友.叫别人起床该干嘛干嘛 += 王八犊子.赌五毛起不来;
            ////于是第二天到了
            //Console.WriteLine("闹铃：早上七点钟...");
            //Console.WriteLine("系统提示：室友起床了！");
            //Console.WriteLine();
            //中国好室友.起床晨跑去();
            //Console.WriteLine();
            //Console.WriteLine("==========全剧终==========");
            #endregion
            Console.ReadLine();
            #region 委托示例2
            //GreetPeople("Jimmy Zhang", EnglishGreeting);
            //GreetPeople("张子阳", ChineseGreeting);//ChineseGreeting是实参
            //Console.ReadKey();
            #endregion
            #region 委托示例3
            //string name1, name2;
            //name1 = "Jimmy Zhang";
            //name2 = "张子阳";

            //GreetPeople(name1, EnglishGreeting);
            //GreetPeople(name2, ChineseGreeting);
            //Console.ReadKey();
            #endregion
            #region 委托示例4
            //GreetingDelegate delegate1, delegate2;
            //delegate1 = EnglishGreeting;
            //delegate2 = ChineseGreeting;

            //GreetPeople("Jimmy Zhang", delegate1);
            //GreetPeople("张子阳", delegate2);
            //Console.ReadKey();
            #endregion
            #region 委托示例5 用+=方式的委托，一次会执行所有+=过的方法
            //GreetingDelegate delegate1;
            //delegate1 = EnglishGreeting; // 先给委托类型的变量赋值
            //delegate1 += ChineseGreeting;   // 给此委托变量再绑定一个方法

            //// 将先后调用 EnglishGreeting 与 ChineseGreeting 方法
            //GreetPeople("Jimmy Zhang", delegate1);
            //Console.ReadKey();
            #endregion
            #region 委托示例6 直接调用委托
            //GreetingDelegate delegate1;//事件的定义其实和这里类似，是对委托的一种实例化，进而实现+=,-=的注册方法
            #region 事件和委托的关系eg
            ////public delegate void delegate2(int param);   //把delegate2声明成委托类型
            ////public event delegate2 delegate3;            //把delegate3声明成一个delegate2类型的事件，这里的delegate3可以和delegate1具有相同的功能了（+=和-=）
            #endregion
            //delegate1 = EnglishGreeting; // 先给委托类型的变量赋值
            //delegate1 += ChineseGreeting;   // 给此委托变量再绑定一个方法

            //// 将先后调用 EnglishGreeting 与 ChineseGreeting 方法
            //delegate1("Jimmy Zhang");
            //Console.ReadKey();
            #endregion
            
            #region 观察者模式 委托 热水器示例
            Heater heater = new Heater();
            Alarm alarm = new Alarm();

            heater.BoilEvent += alarm.MakeAlert;    //注册方法
            heater.BoilEvent += (new Alarm()).MakeAlert;   //给匿名对象注册方法(效果同上)
            heater.BoilEvent += Display.ShowMsg;       //注册静态方法

            heater.BoilWater();   //烧水，会自动调用注册过对象的方法
            Console.ReadLine();
            #endregion
        }

        #region 委托示例2,3,4,5,6通用
        //private static void EnglishGreeting(string name)
        //{
        //    Console.WriteLine("Morning, " + name);
        //}

        //private static void ChineseGreeting(string name)
        //{
        //    Console.WriteLine("早上好, " + name);
        //}

        ////注意此方法，它接受一个GreetingDelegate类型的方法作为参数
        //private static void GreetPeople(string name, GreetingDelegate MakeGreeting)//GreetingDelegate是参数类型，MakeGreeting是形参
        //{
        //    MakeGreeting(name);//通过形参接收到的实参
        //}
        #endregion


    }

    #region 观察者模式 委托 热水器示例
    // 显示器
    public class Display
    {
        public static void ShowMsg(int param)
        { //静态方法
            Console.WriteLine("Display：水快烧开了，当前温度：{0}度。", param);
        }
    }
    // 警报器
    public class Alarm
    {
        public void MakeAlert(int param)
        {
            Console.WriteLine("Alarm：嘀嘀嘀，水已经 {0} 度了：", param);
        }
    }

    public class Heater
    {
        private int temperature;
        public delegate void BoilHandler(int param);   //声明委托
        public event BoilHandler BoilEvent;        //声明事件

        // 烧水
        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                temperature = i;

                if (temperature > 95)
                {
                    if (BoilEvent != null)
                    { //如果有对象注册
                        BoilEvent(temperature);  //调用所有注册对象的方法
                    }
                }
            }
        }
    }
    #endregion



    #region 委托示例2,3,4,5,6通用
    //public delegate void GreetingDelegate(string name);//这里作用是：把GreetingDelegate这个字符串变为一种参数类型（GreetingDelegate变成了类似int或string的东西）
    #endregion

    #region 委托示例1 （事件）
    //public delegate void 这是一个委托();

    //public class 室友
    //{
    //    public void 起床晨跑去()
    //    {
    //        Console.WriteLine("室友：喂，起床啦！");
    //        Console.WriteLine("");
    //        if (叫别人起床该干嘛干嘛 != null) //如果有人委托我什么
    //        {
    //            叫别人起床该干嘛干嘛();
    //        }
    //        Console.WriteLine("室友：完事走人，晨跑去！");
    //    }

    //    public event 这是一个委托 叫别人起床该干嘛干嘛;//将委托封装成一个事件

    //}

    //public class 要晨读的我
    //{
    //    public void 起床晨读()
    //    {
    //        Console.WriteLine("我：哦，起来了！！！");
    //        Console.WriteLine("我：一二三四五，上网看知乎，刷完知乎再晨读！");
    //        Console.WriteLine();
    //    }
    //}

    //public class 王八犊子
    //{
    //    public void 赌五毛起不来()
    //    {
    //        Console.WriteLine("王八犊子：五毛拿走，劳资要碎觉！");
    //        Console.WriteLine();
    //    }
    //}
    #endregion
}
