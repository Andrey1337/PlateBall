using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using PlateBall.Server.Utility;

namespace PlateBall.Server
{
    public class Client
    {
        public string ClientName { get; }
        public IPEndPoint IpEndPoint { get; }
        public Client(string clientName, IPEndPoint ipEndPoint)
        {
            ClientName = clientName;
            IpEndPoint = ipEndPoint;
        }

        public void Connect()
        {
            var client = new UdpClient();
            IPEndPoint ep = IpEndPoint;
            client.Connect(ep);

            // send data
            var package = new Package(3, "SHALOM");
            client.Send(package.Serialize(), package.Serialize().Length);
            //client.Send(new byte[] { 1, 2 }, 2);
            //client.Send(new byte[] { 1, 2 }, 2);

            // then receive data
            var receivedData = client.Receive(ref ep);

            Console.Write("receive data from " + ep);
        }
    }
}
