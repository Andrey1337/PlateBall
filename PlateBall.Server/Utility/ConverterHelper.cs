using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PlateBall.Server.Utility
{
    static class ConverterHelper
    {
        //public byte[] Serialize()
        //{
        //    using (MemoryStream m = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(m))
        //        {
        //            writer.Write(Id);
        //            writer.Write(Name);
        //        }
        //        return m.ToArray();
        //    }
        //}


        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
