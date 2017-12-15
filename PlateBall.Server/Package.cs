using System;
using System.IO;

namespace PlateBall.Server
{
    public class Package
    {
        public byte Command { get; set; }
        public int DataLenght { get; set; }
        public byte[] Data { get; set; }
        public Package(byte command = Byte.MinValue, byte[] data = null)
        {
            Command = command;
            Data = data;
            if (Data != null) DataLenght = Data.Length;
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
                    result.DataLenght = reader.ReadInt32();
                    result.Data = reader.ReadBytes(result.DataLenght);
                }
            }
            return result;
        }

    }
}