using System;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Tester
{
    public class Server
    {
        public struct ConnectedClient
        {
            public string Name { get; set; }
            public string Ip { get; set; }
        }

        public ConnectedClient Client1 { get; private set; }
        public ConnectedClient Client2 { get; private set; }

        public void Start()
        {
            Thread serverThread = new Thread(() =>
            {
                EventBasedNetListener listener = new EventBasedNetListener();
                NetManager server = new NetManager(listener, 2, "1234");
                server.Start(9050);

                listener.NetworkReceiveEvent += (fromPeer, dataReader) =>
                {
                    if (Client1.Name == "")
                        Client1 = new ConnectedClient() { Name = dataReader.GetString(100), Ip = fromPeer.EndPoint.ToString() };
                    else if (Client2.Name == "")
                        Client2 = new ConnectedClient() { Name = dataReader.GetString(100), Ip = fromPeer.EndPoint.ToString() };

                    Console.WriteLine($"{Client1.Name} {Client2.Name}");
                };                

                while (!Console.KeyAvailable)
                {
                    server.PollEvents();
                    Thread.Sleep(15);
                }

                server.Stop();
            });
            serverThread.Start();
        }
    }
}