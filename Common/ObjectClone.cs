using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ObjectClone
    {
        public static T Clone<T>(this T obj) where T:class
        {
            using (var memStream = new MemoryStream())
            {
                var binaryFormatter  = new BinaryFormatter();
                binaryFormatter.Serialize(memStream, obj);
                memStream.Position = 0;
                return (T)binaryFormatter.Deserialize(memStream);
            }

        }
    }
}
