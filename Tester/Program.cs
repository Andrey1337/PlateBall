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
using PlateBall.Server;


namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();

            Client client = new Client("Andrey", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));
            client.Connect();

            Console.ReadLine();
        }

    }
}

