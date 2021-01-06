using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Css.Api.Reporting.Business.UnitTest.Services
{
    /// <summary>
    /// The unit testing class for encrypter
    /// </summary>
    public class EncrypterShould
    {
        #region Private Properties

        /// <summary>
        /// The encrypter
        /// </summary>
        private readonly IEncrypter _encrypter;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>
        public EncrypterShould()
        {
            _encrypter = new Encrypter();
        }
        #endregion

        #region Encrypt(string text) && Decrypt(string encryptedText)
        
        /// <summary>
        /// The method to test the encryption and decryption alogrithm
        /// </summary>
        /// <param name="inputText">a random input text</param>
        [Theory]
        [InlineData("sample")]
        [InlineData("this is a random text")]
        public async void CheckEncryption(string inputText)
        {
            var encryptedValue = await _encrypter.Encrypt(inputText);

            Assert.NotNull(encryptedValue);
            Assert.IsType<string>(encryptedValue);

            var decryptedValue = await _encrypter.Decrypt(encryptedValue);

            Assert.NotNull(decryptedValue);
            Assert.IsType<string>(decryptedValue);
            
            Assert.Equal(inputText, decryptedValue);
        }
        #endregion
    }
}
