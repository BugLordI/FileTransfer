using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Core.SocketTool
{
    internal class SocketServer
    {
        public const int PORT = 7138;
        public const int DEFAULT_BUFFER_SIZE = 1024;
        private static Dictionary<String, Socket> connections = new Dictionary<String, Socket>();

        public static async void Start()
        {
            IPAddress iPAddress = IPAddress.Any;
            IPEndPoint ipEndPoint = new IPEndPoint(iPAddress, PORT);
            using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    serverSocket.Bind(ipEndPoint);
                    serverSocket.Listen(10);
                    while (true)
                    {
                        Socket clientSocket = await serverSocket.AcceptAsync();
                        //IPEndPoint remoteEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
                        String host = GetIpHost(clientSocket);
                        if (!String.IsNullOrEmpty(host))
                        {
                            if (connections.ContainsKey(host))
                            {
                                connections.Remove(host);
                            }
                            connections.Add(host, clientSocket);
                        }
                        _ = HandleClientAsync(clientSocket);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"服务器启动失败: {ex.Message}");
                }
            }
        }

        private static async Task HandleClientAsync(Socket clientSocket)
        {
            try
            {
                byte[] buffer = new byte[DEFAULT_BUFFER_SIZE];
                int receivedBytes;
                while (true)
                {
                    receivedBytes = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                    int type = MessageProtocol.GetMessageType(buffer);
                    int length = MessageProtocol.GetMessageLength(buffer);
                    if (type == (int)MessageType.PING)
                    {
                        byte[] msgByte = new byte[length];
                        Array.Copy(buffer, 13, msgByte, 0, length);
                        String msg = Encoding.UTF8.GetString(msgByte);
                        if (msg == Message.PING.ToString())
                        {
                            byte[] responseBuffer = Encoding.UTF8.GetBytes(Message.PONG);
                            _ = clientSocket.SendAsync(new ArraySegment<byte>(responseBuffer), SocketFlags.None);
                        }
                    }
                    else if (type == (int)MessageType.FILE)
                    {
                        String fileName = MessageProtocol.GetFileName(buffer);
                        ReceiveFile(clientSocket, length, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理客户端时发生错误: {ex.Message}");
            }
            finally
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }

        public static void ReceiveFile(Socket clientSocket,int fileSize,String fileName)
        {
            String basePath = Path.Combine(AppContext.BaseDirectory, "Received");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            String filePath = Path.Combine(basePath, fileName);
            int totalReceived = 0;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[DEFAULT_BUFFER_SIZE];
                int bytesReceived;
                while (totalReceived < fileSize && (bytesReceived = clientSocket.Receive(buffer)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesReceived);
                    totalReceived += bytesReceived;
                }
            }
        }

        public static String GetIpHost(Socket socket)
        {
            if (socket == null || !socket.Connected)
            {
                return "";
            }
            IPEndPoint remoteEndPoint = socket.RemoteEndPoint as IPEndPoint;
            if (remoteEndPoint != null)
            {
                IPAddress clientIpAddress = remoteEndPoint.Address;
                int clientPort = remoteEndPoint.Port;
                return clientIpAddress.ToString() + ":" + clientPort;
            }
            else
            {
                return "";
            }
        }
    }
}
