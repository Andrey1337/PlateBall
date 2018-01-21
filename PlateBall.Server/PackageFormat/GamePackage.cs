using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;

namespace PlateBall.Server.PackageFormat
{
    public class GamePackage
    {
        public byte Command { get; set; }
         
        public int PackageLenght { get; set; }

        public byte[] Data { get; set; }

        public GamePackage(byte command = Byte.MinValue, int packageLenght = Byte.MinValue, byte[] data = null)
        {
            Command = command;
            PackageLenght = packageLenght;
            Data = data;
        }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Command);
                    writer.Write(PackageLenght);
                    writer.Write(Data);
                }
                return m.ToArray();
            }
        }

        public static GamePackage Desserialize(byte[] data)
        {
            GamePackage result = new GamePackage();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    result.Command = reader.ReadByte();
                    result.PackageLenght = reader.ReadInt32();
                    result.Data = reader.ReadBytes(result.PackageLenght);
                }
            }
            return result;
        }

    }
}
