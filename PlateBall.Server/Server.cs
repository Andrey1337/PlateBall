using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using PlateBall.Server.PackageFormat;

namespace PlateBall.Server
{
    public class Server
    {
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public int Port { get; }
        public Random Random;
        public Server(int port)
        {
            Port = port;
            Random = new Random();
        }

        public void StartListen()
        {
            Thread serverThread = new Thread(() =>
            {
                UdpClient udpServer = new UdpClient(Port);

                while (true)
                {
                    var remoteEP = new IPEndPoint(IPAddress.Any, Port);
                    var data = udpServer.Receive(ref remoteEP);
                    Package package = Package.Desserialize(data);

                    Console.WriteLine(remoteEP);
                    switch (package.Command)
                    {
                        case 1:
                            EnterPlayer(remoteEP, package);
                            break;
                        case 2:

                            break;
                        default:
                            byte[] sendBackPackage = new Package(99, "Incorect Command").Serialize();
                            udpServer.Send(sendBackPackage, sendBackPackage.Length, remoteEP);
                            break;
                    }
                }
            });
            serverThread.Start();
        }

        private void EnterPlayer(IPEndPoint ipAdress, Package package)
        {
            var connectPackage = JsonConvert.DeserializeObject<ConnectPackageFormat>(package.Data);
            var recieveIpAdress = new IPEndPoint(ipAdress.Address, connectPackage.Port);
            int key = Random.Next(30000);

            if (Player1 != null && Player2 != null)
            {
                byte[] sendBackPackage = new Package(99, "Server full").Serialize();
                SendRecieve(recieveIpAdress, sendBackPackage);
                return;
            }

            if (Player1 == null)
            {
                Player1 = new Player(connectPackage.Name, recieveIpAdress, key.GetHashCode());
                SendRecieve(recieveIpAdress, new Package(95, key.ToString()).Serialize());
                Console.WriteLine(Player1.Name);
                return;
            }

            if (Player2 == null)
            {
                Player2 = new Player(connectPackage.Name, recieveIpAdress, key.GetHashCode());
                SendRecieve(recieveIpAdress, new Package(95, key.ToString()).Serialize());
                Console.WriteLine(Player2.Name);
                return;
            }
        }

        private void SendRecieve(IPEndPoint ipAndress, byte[] data)
        {
            UdpClient udpReciever = new UdpClient();
            udpReciever.Connect(ipAndress);
            udpReciever.Send(data, data.Length);
            udpReciever.Close();
        }
    }
}