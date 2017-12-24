using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using FarseerPhysics;
using Newtonsoft.Json;
using PlateBall.Server.PackageFormat;

namespace PlateBall.Server
{
    public class Server
    {
        private enum ServerState
        {
            WaitForPlayers,
            Run
        };

        private Task _task;

        public ClientInfo ClientInfo1 { get; private set; }
        public ClientInfo ClientInfo2 { get; private set; }
        public bool IsReady { get; private set; }
        private ServerState _serverState;
        private PlateBallWorld _world;

        public int Port { get; }
        public Random Random;
        public Server(int port)
        {
            _serverState = ServerState.WaitForPlayers;
            Update();
            Port = port;
            Random = new Random();
        }

        public void Update()
        {
            Thread updateThread = new Thread(() =>
           {
               while (true)
               {
                   if (_serverState == ServerState.Run)
                   {
                       Debug.WriteLine("Server start to play");
                       var lastIterationTime = DateTime.Now;
                       var stepSize = TimeSpan.FromSeconds(0.01);
                       while (true)
                       {
                           while (lastIterationTime + stepSize < DateTime.Now)
                           {
                               _world.Update(stepSize.Milliseconds);
                               byte[] sendPackage = new Package(66,
                                       JsonConvert.SerializeObject(
                                           new GameStatePackage(ConvertUnits.ToDisplayUnits(_world.Ball.Position))))
                                   .Serialize();
                               if (ClientInfo1 != null)
                                   SendData(ClientInfo1.IpAddress, sendPackage);
                               if (ClientInfo2 != null)
                                   SendData(ClientInfo2.IpAddress, sendPackage);
                           }

                           lastIterationTime += stepSize;
                       }
                   }
               }
           });
            updateThread.Start();
        }

        public UdpClient UdpServer { get; set; }
        public void StartListen()
        {
            Thread serverThread = new Thread(() =>
            {
                UdpServer = new UdpClient(Port);

                while (true)
                {
                    var remoteEP = new IPEndPoint(IPAddress.Any, Port);
                    var data = UdpServer.Receive(ref remoteEP);
                    Package package = Package.Desserialize(data);
                    switch (package.Command)
                    {
                        case 1:
                            EnterPlayer(remoteEP, package);

                            break;
                        case 2:
                            StartGameRequest(remoteEP, package);
                            break;
                        default:
                            byte[] sendBackPackage = new Package(99, "Incorect Command").Serialize();
                            UdpServer.Send(sendBackPackage, sendBackPackage.Length, remoteEP);
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
            int key = Random.Next(20000);

            if (ClientInfo1 != null && ClientInfo2 != null)
            {
                byte[] sendBackPackage = new Package(99, "Server full").Serialize();
                SendData(recieveIpAdress, sendBackPackage);
                return;
            }

            if (ClientInfo1 == null)
            {
                ClientInfo1 = new ClientInfo(connectPackage.Name, recieveIpAdress, key.GetHashCode());
                SendData(recieveIpAdress, new Package(95, key.ToString()).Serialize());
                Console.WriteLine(ClientInfo1.Name);
                return;
            }

            if (ClientInfo2 == null)
            {
                ClientInfo2 = new ClientInfo(connectPackage.Name, recieveIpAdress, key.GetHashCode());
                SendData(recieveIpAdress, new Package(95, key.ToString()).Serialize());
                Console.WriteLine(ClientInfo2.Name);
                return;
            }
        }

        private void StartGameRequest(IPEndPoint ipAdress, Package package)
        {
            var sessionPackage = JsonConvert.DeserializeObject<SessionConnectionFormat>(package.Data);

            if (ClientInfo1 != null && sessionPackage.Key.GetHashCode() == ClientInfo1.Key)
            {
                ClientInfo1.StartGame = true;
                _serverState = ServerState.Run;
            }

            if (ClientInfo2 != null && sessionPackage.Key.GetHashCode() == ClientInfo2.Key)
            {
                ClientInfo2.StartGame = true;
                _serverState = ServerState.Run;
            }

            if (ClientInfo1 != null && ClientInfo2 != null && ClientInfo1.StartGame && ClientInfo2.StartGame)
            {
                _serverState = ServerState.Run;
            }
            _serverState = ServerState.Run;
            Debug.WriteLine($"{ClientInfo1 != null && ClientInfo1.StartGame}, { ClientInfo2 != null && ClientInfo2.StartGame}");
        }

        private void SendData(IPEndPoint ipAndress, byte[] data)
        {
            UdpClient udpReciever = new UdpClient();
            udpReciever.Connect(ipAndress);
            udpReciever.Send(data, data.Length);
            udpReciever.Close();
        }
    }
}