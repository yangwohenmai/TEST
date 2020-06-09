using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //短连接
            //shotlink("");
            //长连接
            longlink();

            //衍生2.0 Socket交互方法,只能调用衍生客户端方法
            //SocketUserClient client = new SocketUserClient("127.0.0.1", 5002);
            //string responseMsg = client.SentMsgToServer("我是客户端请求123");
            Console.ReadLine();
        }



        /// <summary>
        /// 短连接，最后调用Close释放资源
        /// </summary>
        /// <param name="input"></param>
        public static void shotlink(string input)
        {
            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 5002)); //配置服务器IP与端口  
                Console.WriteLine("连接服务器成功");
            }
            catch
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
                Console.ReadLine();
                return;
            }

            string sendMessage = "你好";//发送到服务端的内容
            clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage));//向服务器发送数据，需要发送中文则需要使用Encoding.UTF8.GetBytes()，否则会乱码
            Console.WriteLine("向服务器发送消息：" + sendMessage);

            //接受从服务器返回的信息
            string recvStr = "";
            byte[] recvBytes = new byte[1024];
            int bytes;
            bytes = clientSocket.Receive(recvBytes, recvBytes.Length, 0);    //从服务器端接受返回信息 
            recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
            Console.WriteLine("服务端发来消息：{0}", recvStr);//回显服务器的返回信息
            //每次完成通信后，关闭连接并释放资源
            clientSocket.Close();
            Console.ReadLine();
        }


        /// <summary>
        /// 长连接不调用Close释放资源
        /// </summary>
        public static void longlink()
        {
            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 5002)); //配置服务器IP与端口  
                Console.WriteLine("连接服务器成功");
            }
            catch
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
                Console.ReadLine();
                return;
            }

            //循环读取输入数据
            while (true)
            {
                Console.WriteLine("请输入");
                string sentstr = Console.ReadLine();
                SentMsg(sentstr, clientSocket);
            }
        }


        /// <summary>
        /// 长连接，不释放资源
        /// </summary>
        /// <param name="sentstr"></param>
        /// <param name="clientSocket"></param>
        public static void SentMsg(string sentstr, Socket clientSocket)
        {
            string sendMessage = "你好";//发送到服务端的内容
            sendMessage = sentstr;//发送到服务端的内容
            //向服务器发送数据，需要发送中文则需要使用Encoding.UTF8.GetBytes()，否则会乱码
            clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage));
            Console.WriteLine("向服务器发送消息：" + sendMessage);

            //接受从服务器返回的信息
            string recvStr = "";
            byte[] recvBytes = new byte[1024];
            int bytes;
            bytes = clientSocket.Receive(recvBytes, recvBytes.Length, 0);    //从服务器端接受返回信息 
            recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
            Console.WriteLine("服务端发来消息：{0}", recvStr);    //回显服务器的返回信息
            //clientSocket.Close();//关闭连接并释放资源//如果是长连接，注释掉close
        }
    }
}
