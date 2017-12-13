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
            Server server = new Server();
            server.Start();

            Client client1 = new Client("Andrey");
            client1.Connect("localhost");

            Client client2 = new Client("Vanya");
            client2.Connect("localhost");
                                     
        }

    }
}

