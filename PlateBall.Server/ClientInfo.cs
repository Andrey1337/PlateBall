using System.Net;

namespace PlateBall.Server
{
    public class ClientInfo
    {
        public IPEndPoint IpAddress { get; }
        public string Name { get; }
        public int Key { get; }
        public bool StartGame { get; set; }
        public ClientInfo(string name, IPEndPoint ipAddress, int key)
        {
            Name = name;
            IpAddress = ipAddress;
            Key = key;
        }
    }
}