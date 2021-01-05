using Css.Api.Reporting.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Services
{
	/// <summary>
	/// An encrypter class
	/// </summary>
    public class Encrypter : IEncrypter
    {
        #region Private Properties
        
		/// <summary>
        /// The private secret key used for encryption and decryption
        /// </summary>
        private const string _privateKey = "B40267AA91D84F7D8463DDD8C430725B";
        #endregion

        #region Public Methods
        /// <summary>
        /// The method encrypts the input string
        /// </summary>
        /// <param name="text">The input text to be encrypted</param>
        /// <returns>The encrypted string</returns>
        public async Task<string> Encrypt(string text)
		{
			var key = Encoding.UTF8.GetBytes(_privateKey);

			using (var aesAlg = Aes.Create())
			{
				using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
				{
					using (var msEncrypt = new MemoryStream())
					{
						using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
						using (var swEncrypt = new StreamWriter(csEncrypt))
						{
							await swEncrypt.WriteAsync(text);
						}

						var iv = aesAlg.IV;

						var decryptedContent = msEncrypt.ToArray();

						var result = new byte[iv.Length + decryptedContent.Length];

						Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
						Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

						return Convert.ToBase64String(result);
					}
				}
			}
		}

		/// <summary>
		/// The method decrypts the string
		/// </summary>
		/// <param name="encryptedText">The input encrypted string</param>
		/// <returns>The original string</returns>
		public async Task<string> Decrypt(string encryptedText)
		{
			if (string.IsNullOrEmpty(encryptedText)) return encryptedText;
			try
			{
				encryptedText = encryptedText.Replace(" ", "+");
				var fullCipher = Convert.FromBase64String(encryptedText);

				var iv = new byte[16];
				var cipher = new byte[fullCipher.Length - iv.Length];

				Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
				Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);
				var key = Encoding.UTF8.GetBytes(_privateKey);

				using (var aesAlg = Aes.Create())
				{
					using (var decryptor = aesAlg.CreateDecryptor(key, iv))
					{
						string result;
						using (var msDecrypt = new MemoryStream(cipher))
						{
							using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
							{
								using (var srDecrypt = new StreamReader(csDecrypt))
								{
									result = await srDecrypt.ReadToEndAsync();
								}
							}
						}

						return result;
					}
				}
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
		}
        #endregion
    }
}
