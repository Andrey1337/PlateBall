using System;
using System.Net;
using System.Net.Sockets;

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
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000); // endpoint where server is listening
            client.Connect(ep);

            // send data
            client.Send(new byte[] { 1, 2, 3, 4, 5 }, 5);

            // then receive data
            var receivedData = client.Receive(ref ep);
 


            Console.Write("receive data from " + ep.ToString());            
        }            

    }
}
