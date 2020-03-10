using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketServer
{
    public class SocketServer
    {
        private const int headLength = 4;
        protected TcpListener listener;


        public void StartBackGroundListener(string ipaddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipaddress);
            listener = new TcpListener(ip, port);
            listener.Start();
            Console.WriteLine("服务端开始监听消息");
            while (true)
            {
                try
                {
                    // A blocking operation was interrupted by a call to WSACancelBlockingCall
                    Socket client = listener.AcceptSocket();
                    
                    ThreadPool.QueueUserWorkItem(HandleClientComm, client);
                }
                catch (SocketException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void HandleClientComm(object client)
        {
            Socket socketClient = (Socket)client;
            socketClient.NoDelay = true;

            LingerOption lingerOption = new LingerOption(true, 3);
            socketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, lingerOption);

            string request = string.Empty;

            try
            {
                byte[] headBuffer = new byte[4];
                // 读取头信息 接受的长度
                socketClient.Receive(headBuffer, 4, SocketFlags.None);
                // 需要接受的长度
                int needRecvLength = BitConverter.ToInt32(headBuffer, 0);

                if (needRecvLength != 0)
                {
                    // 未接受的长度
                    int notRecvLength = needRecvLength;

                    // 分配空间
                    byte[] readBuffer = new byte[needRecvLength + headLength];

                    // 接受信息
                    do
                    {
                        // 已经接受的长度
                        int hasRecv = socketClient.Receive(readBuffer, headLength + needRecvLength - notRecvLength,
                                                           notRecvLength, SocketFlags.None);
                        notRecvLength -= hasRecv;

                    } while (notRecvLength != 0);

                    request = Encoding.UTF8.GetString(readBuffer, headLength, needRecvLength);
                }

                
                StringBuilder response = new StringBuilder(request);
                Console.WriteLine("收到客户端消息：" + response);
                if (socketClient.Connected)
                {
                    string sendString = string.Format("还你_{0}456", response.ToString());

                    // 发送字符串
                    byte[] contentByte = Encoding.UTF8.GetBytes(sendString);
                    byte[] headBytes = BitConverter.GetBytes(contentByte.Length);

                    byte[] sendByte = new byte[headBytes.Length + contentByte.Length];

                    headBytes.CopyTo(sendByte, 0);
                    contentByte.CopyTo(sendByte, headLength);

                    int needSendLength = sendByte.Length;

                    do
                    {
                        int nSend = socketClient.Send(sendByte, sendByte.Length - needSendLength, needSendLength,
                                                      SocketFlags.None);
                        needSendLength -= nSend;

                    } while (needSendLength != 0);

                    Console.WriteLine("消息回复完成：" + sendString);
                }
            }
            #region 全局错误处理
            catch (SocketException ex)
            {

                if (socketClient.Connected)
                {
                    socketClient.Shutdown(SocketShutdown.Both);
                    socketClient.Close();
                }
            }
            catch (Exception ex)
            {

                // 写错误日志
                //EventLogger.LogError(ex.Message);

                // 发错误邮件
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                string mailBody = string.Format("用户输入：{0}{1}{0}错误信息：{0}{2}{0}堆栈信息：{0}{3}{0}", "\r\n", request, ex.Message, ex.StackTrace);


            }
            #endregion
        }
    }
}
