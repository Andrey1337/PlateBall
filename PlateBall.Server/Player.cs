using System.Net;

namespace PlateBall.Server
{
    public class Player
    {
        public IPEndPoint IpAddress { get; }
        public string Name { get; }
        public int Key { get; }
        public Player(string name, IPEndPoint ipAddress, int key)
        {
            Name = name;
            IpAddress = ipAddress;
            Key = key;
        }
    }
}