using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// An interface for all encrypters
    /// </summary>
    public interface IEncrypter
    {
        /// <summary>
        /// The method encrypts the input string
        /// </summary>
        /// <param name="text">The input text to be encrypted</param>
        /// <returns>The encrypted string</returns>
        Task<string> Encrypt(string text);

        /// <summary>
        /// The method decrypts the string
        /// </summary>
        /// <param name="encryptedText">The input encrypted string</param>
        /// <returns>The original string</returns>
        Task<string> Decrypt(string encryptedText);
    }
}
