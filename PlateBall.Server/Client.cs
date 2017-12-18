using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Newtonsoft.Json;
using PlateBall.Server.PackageFormat;

namespace PlateBall.Server
{
    public class Client
    {
        public string ClientName { get; }
        public IPEndPoint IpEndPoint { get; }
        public int ConnectionKey { get; set; }
        public int Port { get; set; }
        public bool IsConnected { get; set; }
        public Client(string clientName, int recievePort, IPEndPoint ipEndPoint)
        {
            ClientName = clientName;
            IpEndPoint = ipEndPoint;
            Port = recievePort;
            RecieveListener();
        }

        private DateTime starTime;

        public void Connect()
        {

            var client = new UdpClient();
            IPEndPoint ep = IpEndPoint;
            client.Connect(ep);
            var package = new Package(1, JsonConvert.SerializeObject(new ConnectPackageFormat(ClientName, Port)));
            client.Send(package.Serialize(), package.Serialize().Length);
            starTime = DateTime.Now;
            client.Close();
        }

        public void StartGame()
        {
            var client = new UdpClient();
            IPEndPoint ep = IpEndPoint;
            client.Connect(ep);
            var package = new Package(2, JsonConvert.SerializeObject(new SessionConnectionFormat(ConnectionKey, "HELOU")));
            client.Send(package.Serialize(), package.Serialize().Length);

            client.Close();
        }

        public void RecieveListener()
        {
            Thread listener = new Thread(() =>
                {
                    UdpClient udpServer = new UdpClient(Port);
                    while (true)
                    {
                        var remoteEP = new IPEndPoint(IPAddress.Any, Port);
                        var data = udpServer.Receive(ref remoteEP);
                        Package package = Package.Desserialize(data);
                        switch (package.Command)
                        {
                            case 95:
                                ConnectionKey = int.Parse(package.Data);
                                Console.WriteLine($"Connection key => {ConnectionKey}");
                                break;
                        }
                        Console.WriteLine($"Recieved data => {package.Data}");
                    }
                }
            );

            listener.Start();
        }
    }
}
