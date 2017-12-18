using System;
using System.IO;

namespace PlateBall.Server.PackageFormat
{
    public class Package
    {
        public int Key { get; set; }
        public byte Command { get; set; }
        public string Data { get; set; }
        public Package(byte command = Byte.MinValue, string data = null)
        {
            Command = command;
            Data = data;
        }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Command);
                    writer.Write(Data);
                }
                return m.ToArray();
            }
        }

        public static Package Desserialize(byte[] data)
        {
            Package result = new Package();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    result.Command = reader.ReadByte();
                    result.Data = reader.ReadString();
                }
            }
            return result;
        }

    }
}