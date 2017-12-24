using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Newtonsoft.Json;
using PlateBall.Client.Screens;
using PlateBall.Server.PackageFormat;

namespace PlateBall.Client
{
    public class Client
    {
        public string ClientName { get; }
        public IPEndPoint IpEndPoint { get; }
        public int ConnectionKey { get; set; }
        public int Port { get; set; }
        public bool IsConnected { get; set; }

        private readonly PlateBallGameScreen _gameScreen;

        public Client(string clientName, int recievePort, IPEndPoint ipEndPoint, PlateBallGameScreen gameScreen)
        {
            _gameScreen = gameScreen;
            ClientName = clientName;
            IpEndPoint = ipEndPoint;
            Port = recievePort;
            RecieveListener();
        }

        public Client(string clientName, int recievePort, IPEndPoint ipEndPoint)
        {
            ClientName = clientName;
            IpEndPoint = ipEndPoint;
            Port = recievePort;
            RecieveListener();
        }

        public void Connect()
        {
            Thread serverConnectThread = new Thread(() =>
            {
                var client = new UdpClient();
                IPEndPoint ep = IpEndPoint;

                client.Connect(ep);
                var package = new Package(1, JsonConvert.SerializeObject(new ConnectPackageFormat(ClientName, Port)));
                client.Send(package.Serialize(), package.Serialize().Length);

                client.Close();
            });
            serverConnectThread.Start();
        }

        public void StartGame()
        {
            Thread startGameThread = new Thread(() =>
            {
                while (!IsConnected)
                {
                }
                var client = new UdpClient();
                IPEndPoint ep = IpEndPoint;
                client.Connect(ep);
                var package = new Package(2,
                    JsonConvert.SerializeObject(new SessionConnectionFormat(ConnectionKey, "HELOU")));
                client.Send(package.Serialize(), package.Serialize().Length);

                Debug.WriteLine("start request sent");
                client.Close();
            });
            startGameThread.Start();
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
                                IsConnected = true;
                                Debug.WriteLine($"Client: {ClientName}, ConnectionKey: {ConnectionKey}");
                                break;
                            case 66:
                                Debug.Write("GUT IT");

                                _gameScreen.GameWorld.GameInfo =
                                    JsonConvert.DeserializeObject<GameStatePackage>(package.Data);
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
