using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;
using nicts_probate_sqs_api.Models;

namespace nicts_probate_sqs_api.Services
{
    public static class XmlExtensions
    {
        public static T Deserialize<T>(this string value)
        {
            var xmlSerializer = new DataContractSerializer(typeof(T));

            return (T)xmlSerializer.ReadObject(GenerateStreamFromString(value));
        }

        public static string Serialize<T>(this T value)
        {
            if (value == null)
                return string.Empty;

            using (TextWriter writer = new Utf8StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer))
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(xmlWriter, value);
                }
                return writer.ToString();
            }

        }
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
