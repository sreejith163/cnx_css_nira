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
            var str = Encoding.Default.GetString(bytes);
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
        #endregion
    }
}
