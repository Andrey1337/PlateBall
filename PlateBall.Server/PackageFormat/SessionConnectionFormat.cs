namespace PlateBall.Server.PackageFormat
{
    public class SessionConnectionFormat
    {
        public int Key { get; }

        public string Data { get; }

        public SessionConnectionFormat(int key, string data)
        {
            Key = key;
            Data = data;
        }
    }
}