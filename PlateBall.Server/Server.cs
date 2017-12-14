using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PlateBall.Server
{
    public class Server
    {
        public void Start()
        {
            Thread serverThread = new Thread(() =>
            {
                UdpClient udpServer = new UdpClient(11000);

                while (true)
                {
                    var remoteEP = new IPEndPoint(IPAddress.Any, 11000);
                    var data = udpServer.Receive(ref remoteEP); // listen on port 11000
                    Console.Write("receive data from " + remoteEP.ToString());
                    udpServer.Send(new byte[] { 1 }, 1, remoteEP); // reply back
                }
            });
            serverThread.Start();
        }
    }
}