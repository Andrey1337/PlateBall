using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            EventBasedNetListener listener = new EventBasedNetListener();
            NetManager server = new NetManager(listener, 2 /* maximum clients */, "SomeConnectionKey");
            server.Start(9050 /* port */);

            listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
                NetDataWriter writer = new NetDataWriter();                 // Create writer class
                writer.Put("Hello client!");                                // Put some string
                peer.Send(writer, SendOptions.ReliableOrdered);             // Send with reliability
            };

            while (!Console.KeyAvailable)
            {
                server.PollEvents();
                Thread.Sleep(15);
            }

            server.Stop();



        }
    }
}
