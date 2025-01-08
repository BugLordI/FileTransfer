using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static FileTransfer.i18n.LanuageMananger;

namespace FileTransfer.Core.SocketTool
{
    internal class SocketClient
    {
        public const String PING = "PING";
        public const String PONG = "PONG";
        public delegate void FTConsole(String text);

        public static void Ping(String ip, int port, FTConsole console)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes(PING);
            var socket = Connect(ip, port, console);
            if (socket != null)
            {
                socket.Send(sendBuffer);
                console($"{GetString("SendStr")}{PING}");
                byte[] receiveBuffer = new byte[1024];
                int receivedBytes = socket.Receive(receiveBuffer);
                string response = Encoding.UTF8.GetString(receiveBuffer, 0, receivedBytes);
                console($"{GetString("ReceiveStr")}{response}");
                if (response == PONG)
                {
                    console($"{GetString("CheckSuccessStr")}");
                }
                else
                {
                    console($"{GetString("CheckFailedStr")}");
                }
                console($"{GetString("DisConnectStr")}");
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                console($"{GetString("DisconnectedStr")}");
            }
            else
            {
                console($"{GetString("ConnectFailedStr")}");
            }
        }

        public static Socket? Connect(String ip, int port, FTConsole console)
        {
            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.ReceiveTimeout = 10000;
                console($"{GetString("TryConnectStr")}");
                clientSocket.Connect(ipEndPoint);
                return clientSocket;
            }
            catch (Exception ex)
            {
                console($"{GetString("ConnectFailedStr")}");
                return null;
            }
        }
    }
}
