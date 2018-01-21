using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlateBall.Client.Screens;
using PlateBall.Server.PackageFormat;

namespace PlateBall.Client
{
    public class Client
    {
        public string ClientName { get; }
        private int _connectionKey;

        public IPEndPoint IpEndPoint { get; }

        public int Port { get; set; }

        private bool _isConnected;
        public bool DoneListen { get; set; }

        private Task _listener;

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

        public void ConnectRequest()
        {
            Task serverConnectTask = new Task(() =>
            {
                var client = new UdpClient();
                IPEndPoint ep = IpEndPoint;

                client.Connect(ep);
                var package = new Package(1, JsonConvert.SerializeObject(new ConnectPackageFormat(ClientName, Port)));
                client.Send(package.Serialize(), package.Serialize().Length);

                client.Close();
            });
            serverConnectTask.Start();
        }

        public void StartGameRequest()
        {
            Task startGameThread = new Task(() =>
            {
                var client = new UdpClient();
                IPEndPoint ep = IpEndPoint;
                client.Connect(ep);
                var package = new Package(2, JsonConvert.SerializeObject(new SessionConnectionFormat(_connectionKey, "HELOU")));
                client.Send(package.Serialize(), package.Serialize().Length);
                client.Close();
            });
            startGameThread.Start();
        }

        public void Exit()
        {
            DoneListen = true;
        }

        public void RecieveListener()
        {
            _listener = new Task(() =>
                {
                    UdpClient udpServer = new UdpClient(Port);
                    while (!DoneListen)
                    {
                        try
                        {
                            var remoteEp = new IPEndPoint(IPAddress.Any, Port);
                            var data = udpServer.Receive(ref remoteEp);
                            HandleRequest(Package.Desserialize(data));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("[{0}]\t-->\tError\n{1}\n", DateTime.Now, e.Message);
                        }
                    }
                }
            );

            _listener.Start();
        }

        private void HandleRequest(Package package)
        {
            switch (package.Command)
            {
                case 95:
                    _connectionKey = int.Parse(package.Data);
                    _isConnected = true;
                    StartGameRequest();
                    Debug.WriteLine($"Client: {ClientName}, ConnectionKey: {_connectionKey}");
                    break;
                case 66:
                    Debug.WriteLine("Got game package");
                    _gameScreen.GameWorld.GameInfo =
                        JsonConvert.DeserializeObject<GameStatePackage>(package.Data);
                    break;
            }
        }
    }
}
