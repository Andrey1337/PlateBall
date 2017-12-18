using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PlateBall.Server;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(11000);
            server.StartListen();

            Client client1 = new Client("Andrey", 64064, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));
            client1.Connect();

            Client client2 = new Client("Vanya", 64070, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));
            client2.Connect();

            Thread.Sleep(TimeSpan.FromSeconds(1));
            client1.StartGame();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            client2.StartGame();
            Console.ReadLine();
        }
    }
}
