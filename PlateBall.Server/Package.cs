namespace PlateBall.Server
{
    public class Package
    {
        public byte TypeOfPackage { get; }
        public Command Command { get; }

        public Package(byte typeOfPackage, Command command)
        {
            TypeOfPackage = typeOfPackage;
            Command = command;
        }

    }
}