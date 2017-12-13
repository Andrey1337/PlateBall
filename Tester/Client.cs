using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Tester
{
    class Client
    {
        public string ClientName { get; }
        public Client(string clientName)
        {
            ClientName = clientName;
        }

        public void Connect(string ip)
        {
            Thread clientThread = new Thread(() =>
            {
                EventBasedNetListener listener = new EventBasedNetListener();
                NetManager client = new NetManager(listener, "1234");
                client.Start();
                client.Connect(ip, 9050);

                listener.PeerConnectedEvent += peer =>
                {
                    NetDataWriter writer = new NetDataWriter();
                    writer.Put(ClientName);
                    peer.Send(writer, SendOptions.ReliableOrdered);                    
                };

                //listener.NetworkReceiveEvent += (fromPeer, dataReader) =>
                //{
                //    Console.WriteLine("We got: {0}", dataReader.GetString(100));
                //    Console.WriteLine(fromPeer.EndPoint);
                //};

                while (!Console.KeyAvailable)
                {
                    client.PollEvents();
                    Thread.Sleep(15);
                }

                client.Stop();

            });

            clientThread.Start();
        }
    }
}
