using System;
using System.Net;
using System.Net.Sockets;
using NetServer.Action;
using NetServer.Session;

namespace NetServer
{
    /// <summary>
    /// 服务器类
    /// </summary>
    public class Server
    {
        /// <summary>
        /// 监听套接字
        /// </summary>
        public Socket listen;

        public int maxClient = 50;

        /// <summary>
        /// 启动服务器
        /// </summary>
        public void StartServer(string host, int port)
        {
            listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(host);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            listen.Bind(ipEndPoint);
            listen.Listen(maxClient);
            listen.BeginAccept(AcceptCallBack, null);
            Console.WriteLine("服务器启动成功！");
            SessionClientPool.SetMaxSessionClient(maxClient);
        }

        /// <summary>
        /// 异步建立客户端连接回调
        /// </summary>
        private void AcceptCallBack(IAsyncResult ar)
        {
            try
            {
                Socket _socket = listen.EndAccept(ar);
                SessionClient _client = SessionClientPool.GetSessionClient();
                if (_client != null)
                {
                    _client.Initialize(_socket);
                    Console.WriteLine("客户端连接 [{0}]", _socket.RemoteEndPoint.ToString());
                }
                else
                {
                    _socket.Close();
                    Console.WriteLine("警告：连接已满！");
                }

                listen.BeginAccept(AcceptCallBack, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("异步建立客户端连接失败：" + e.Message);
            }
        }
    }
}
