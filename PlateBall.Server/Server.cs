using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using FarseerPhysics;
using Microsoft.Xna.Framework;
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

        public Task GameUpdateTask { get; private set; }

        private ClientInfo _clientInfo1;
        private ClientInfo _clientInfo2;

        public int Port { get; }

        private readonly PlateBallWorld _world;

        private ServerState _serverState;


        private readonly Random _random;


        private bool _doneListen;
        private bool _doneSendPackets;
        private bool _doneWorldUpdate;

        private Task _serverUpdateTask;
        private Task _serverListener;


        public Server(int port)
        {
            StartListen();
            _serverState = ServerState.WaitForPlayers;
            _world = new PlateBallWorld(new Vector2(0, 0), this);
            Port = port;
            _random = new Random();
        }

        public void WorldUpdate()
        {
            GameUpdateTask = new Task(() =>
            {
                var lastIterationTime = DateTime.Now;
                var stepSize = TimeSpan.FromSeconds(0.01);
                while (!_doneWorldUpdate)
                {
                    while (lastIterationTime + stepSize < DateTime.Now)
                    {
                        _world.Update(stepSize.Milliseconds);
                        lastIterationTime += stepSize;
                    }
                }
            });

            GameUpdateTask.Start();
        }

        public bool IsRunning { get; set; }


        public void Exit()
        {

            _doneListen = true;
            _doneSendPackets = true;
            _doneWorldUpdate = true;

        }

        public void ServerSendPacketsUpdate()
        {
            _serverUpdateTask = new Task(() =>
           {
               var lastIterationTime = DateTime.Now;
               var stepSize = TimeSpan.FromSeconds(0.01);
               UdpClient udpSender = new UdpClient();
               udpSender.Connect(_clientInfo1.IpAddress);

               WorldUpdate();
               int count = 0;

               while (!_doneSendPackets)
               {
                   Debug.WriteLine(count++);
                   byte[] sendPackage = new Package(66, JsonConvert.SerializeObject(new GameStatePackage(ConvertUnits.ToDisplayUnits(_world.Ball.Position))))
                       .Serialize();
                   if (_clientInfo1 != null)
                       udpSender.Send(sendPackage, sendPackage.Length);

                   if (_clientInfo2 != null)
                       SendData(_clientInfo2.IpAddress, sendPackage);

                   lastIterationTime += stepSize;
               }
               udpSender.Close();

           });
            _serverUpdateTask.Start();
        }

        public void StartListen()
        {
            _serverListener = new Task(() =>
            {
                var udpServer = new UdpClient(Port);

                while (!_doneListen)
                {
                    var remoteEp = new IPEndPoint(IPAddress.Any, Port);
                    var data = udpServer.Receive(ref remoteEp);
                    Package package = Package.Desserialize(data);
                    HandleRequest(package, remoteEp);
                }
                udpServer.Close();
            });
            _serverListener.Start();
        }

        private void HandleRequest(Package package, IPEndPoint remoteEp)
        {
            switch (package.Command)
            {
                case 1:
                    EnterPlayer(remoteEp, package);
                    break;
                case 2:
                    StartGameRequest(remoteEp, package);
                    break;
                    //default:
                    //    byte[] sendBackPackage = new Package(99, "Incorect Command").Serialize();
                    //    udpServer.Send(sendBackPackage, sendBackPackage.Length, remoteEP);
                    //    break;
            }

        }


        private void EnterPlayer(IPEndPoint ipAdress, Package package)
        {
            var connectPackage = JsonConvert.DeserializeObject<ConnectPackageFormat>(package.Data);
            var recieveIpAdress = new IPEndPoint(ipAdress.Address, connectPackage.Port);
            int key = _random.Next(20000);

            if (_clientInfo1 != null && _clientInfo2 != null)
            {
                byte[] sendBackPackage = new Package(99, "Server full").Serialize();
                SendData(recieveIpAdress, sendBackPackage);
                return;
            }

            if (_clientInfo1 == null)
            {
                _clientInfo1 = new ClientInfo(connectPackage.Name, recieveIpAdress, key.GetHashCode());
                SendData(recieveIpAdress, new Package(95, key.ToString()).Serialize());
                return;
            }

            if (_clientInfo2 == null)
            {
                _clientInfo2 = new ClientInfo(connectPackage.Name, recieveIpAdress, key.GetHashCode());
                SendData(recieveIpAdress, new Package(95, key.ToString()).Serialize());
                return;
            }
        }

        private void StartGameRequest(IPEndPoint ipAdress, Package package)
        {
            var sessionPackage = JsonConvert.DeserializeObject<SessionConnectionFormat>(package.Data);

            if (_clientInfo1 != null && sessionPackage.Key.GetHashCode() == _clientInfo1.Key)
            {
                _clientInfo1.StartGame = true;
                ServerSendPacketsUpdate();
                //GameUpdate();
            }

            //if (ClientInfo2 != null && sessionPackage.Key.GetHashCode() == ClientInfo2.Key)
            //{
            //    ClientInfo2.StartGame = true;
            //    _serverState = ServerState.Run;
            //}

            //if (ClientInfo1 != null && ClientInfo2 != null && ClientInfo1.StartGame && ClientInfo2.StartGame)
            //{
            //    _serverState = ServerState.Run;
            //}
            _serverState = ServerState.Run;
            //Debug.WriteLine($"{_clientInfo1 != null && _clientInfo1.StartGame}, { _clientInfo2 != null && _clientInfo2.StartGame}");
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