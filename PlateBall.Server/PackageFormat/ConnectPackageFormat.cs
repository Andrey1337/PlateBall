namespace PlateBall.Server.PackageFormat
{
    public class ConnectPackageFormat
    {
        public int Port { get; }
        public string Name { get; }
        public ConnectPackageFormat(string name, int port)
        {
            Name = name;
            Port = port;
        }
    }
}
