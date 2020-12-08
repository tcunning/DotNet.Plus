using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotNet.Plus.Security
{
    public static partial class Crypto
    {
        /// <summary>
        /// Legacy support for older style DES (Data Encryption Standard) support for simple encryption/decryption. 
        /// </summary>
        /// <param name="encryptedBase64String">The  base 64 encrypted string to decrypt</param>
        /// <param name="encryptionKey">The DES key, should be exactly 8 bytes</param>
        /// <returns>The decrypted result as a string</returns>
        /// <exception cref="CryptographicException">If there was an error during decryption</exception>
        public static string DesDecrypt(this string encryptedBase64String, string encryptionKey)
        {
            if( string.IsNullOrEmpty(encryptedBase64String) )
                return "";

            byte[] keyBytes = Encoding.ASCII.GetBytes(encryptionKey);
            byte[] stringBytes = Convert.FromBase64String(encryptedBase64String);

            using DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            using var decryptor = cryptoProvider.CreateDecryptor(keyBytes, keyBytes);

            using MemoryStream memoryStream = new MemoryStream(stringBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);  // Will be disposed by StreamReader
            using StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Legacy support for older style DES (Data Encryption Standard) support for simple encryption/decryption. 
        /// </summary>
        /// <param name="originalString">The unencrypted string to encrypt</param>
        /// <param name="encryptionKey">The DES key, should be exactly 8 bytes</param>
        /// <returns>The encrypted result as a base 64 string</returns>
        /// <exception cref="CryptographicException">If there was an error during encryption</exception>
        public static string DesEncrypt(this string originalString, string encryptionKey)
        {
            if( string.IsNullOrEmpty(originalString) )
                return "";

            byte[] keyBytes = Encoding.ASCII.GetBytes(encryptionKey);
            byte[] stringBytes = Encoding.ASCII.GetBytes(originalString);

            using DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            using var encryptor = cryptoProvider.CreateEncryptor(keyBytes, keyBytes);

            using MemoryStream input = new MemoryStream(stringBytes);
            CryptoStream cryptoStream = new CryptoStream(input, encryptor, CryptoStreamMode.Read);       // Will be disposed by StreamReader
            using MemoryStream output = new MemoryStream();
            cryptoStream.CopyTo(output);
            var encryptedBytes = output.ToArray();

            return Convert.ToBase64String(encryptedBytes);
        }
    }
}