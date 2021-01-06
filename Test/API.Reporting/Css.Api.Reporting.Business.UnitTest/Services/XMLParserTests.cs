using Css.Api.Reporting.Business.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Css.Api.Reporting.Business.UnitTest.Services
{
    /// <summary>
    ///  The abstract unit testing class for generic XMLParser
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class XMLParserTests<T>
        where T : class
    {
        #region Serialize(T data)

        /// <summary>
        /// The method to test xml serialization
        /// </summary>
        [Fact]
        public void CheckSerialization()
        {
            T input = this.CreateSerializationSample();

            XMLParser<T> parser = new XMLParser<T>();
            var result = parser.Serialize(input);

            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }
        #endregion

        #region Deserialize(string data)

        /// <summary>
        /// The method to test xml deserialization
        /// </summary>
        [Fact]
        public void CheckDeserialization()
        {
            string input = this.CreateDeserializationSample();

            XMLParser<T> parser = new XMLParser<T>();
            var result = parser.Deserialize(input);

            Assert.NotNull(result);
            Assert.IsType<T>(result);
        }
        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// The abstract method which generates the serialization sample
        /// </summary>
        /// <returns></returns>
        protected abstract T CreateSerializationSample();

        /// <summary>
        /// The abstract method which generates the deserialization sample
        /// </summary>
        /// <returns></returns>
        protected abstract string CreateDeserializationSample();
        #endregion
    }

}
