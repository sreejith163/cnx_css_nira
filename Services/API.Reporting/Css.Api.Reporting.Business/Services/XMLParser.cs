using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// The generic XMLParser class for XML operations
    /// </summary>
    /// <typeparam name="T">Any class with XMLSerialization attributes</typeparam>
    public class XMLParser<T> 
        where T : class
    {
        #region Public Methods
        
        /// <summary>
        /// The generic deserializer which deserializes the input bytes array
        /// </summary>
        /// <param name="bytes">The input byte[] to be deserialized</param>
        /// <returns>An instance of T</returns>
        public T Deserialize(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T) serializer.Deserialize(stream);
        }

        /// <summary>
        /// The generic deserializer which deserializes the input string
        /// </summary>
        /// <param name="data">The input string to be deserialized</param>
        /// <returns>An instance of T</returns>
        public T Deserialize(string data)
        {
            byte[] bytes = Encoding.Default.GetBytes(data);
            return Deserialize(bytes);
        }

        /// <summary>
        /// The generic serializer for the input data of type T
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Serialize(T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, data);
            return sb.ToString();
        }
        #endregion
    }
}
