using System.Xml.Serialization;

namespace Maybe.Common.Utils
{
    public static class Serialization
    {
        public static byte[] Serialize<T>(T obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static void Serialize<T>(T obj, string path)
        {
            File.WriteAllBytes(path, Serialize(obj));
        }

        public static T Deserialize<T>(byte[] obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var ms = new MemoryStream(obj))
            {
                return (T)xmlSerializer.Deserialize(ms);
            }
        }

        public static T Deserialize<T>(string path)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var fs = new FileStream(path,FileMode.Open))
            {
                return (T)xmlSerializer.Deserialize(fs);
            }
        }
    }
}
