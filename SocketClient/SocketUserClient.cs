using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class SocketUserClient
    {
        private Socket client;
        private readonly string hostName;
        private readonly int serverPort;
        private readonly IPEndPoint ipep;

        public SocketUserClient(string hostName, int serverPort)
        {
            this.hostName = hostName;
            this.serverPort = serverPort;

            ipep = new IPEndPoint(IPAddress.Parse(hostName), serverPort);
        }


        public string SentMsgToServer(string msg)
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(hostName, serverPort);

                // 向服务器发送内容
                byte[] contentByte = Encoding.UTF8.GetBytes(msg);
                byte[] headBytes = BitConverter.GetBytes(contentByte.Length);

                byte[] sendByte = new byte[headBytes.Length + contentByte.Length];

                headBytes.CopyTo(sendByte, 0);
                contentByte.CopyTo(sendByte, 4);
                client.SendTo(sendByte, sendByte.Length, SocketFlags.None, ipep);
                Console.WriteLine("客户端信息已发送：" + msg);
                // 接受响应
                string response = string.Empty;

                byte[] headBuffer = new byte[4];
                // 读取头信息 接受的长度
                client.Receive(headBuffer, 4, SocketFlags.None);
                // 需要接受的长度
                int needRecvLength = BitConverter.ToInt32(headBuffer, 0);

                if (needRecvLength != 0)
                {
                    // 未接受的长度
                    int notRecvLength = needRecvLength;

                    // 分配空间
                    byte[] readBuffer = new byte[needRecvLength + 4];

                    // 接受信息
                    do
                    {
                        // 已经接受的长度
                        int hasRecv = client.Receive(readBuffer, 4 + needRecvLength - notRecvLength, notRecvLength, SocketFlags.None);
                        notRecvLength -= hasRecv;

                    } while (notRecvLength != 0);

                    response = Encoding.UTF8.GetString(readBuffer, 4, needRecvLength);
                    Console.WriteLine("收到服务端回复："+ response);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (client.Connected)
                {
                    client.Close();
                }
            }
        }
    }
}
