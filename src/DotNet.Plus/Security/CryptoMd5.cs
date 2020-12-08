using System;
using System.Security.Cryptography;
using System.Text;

namespace DotNet.Plus.Security
{
    public static partial class Crypto
    {
        /// <summary>
        /// Generate an MD5 Hash from the given string.  The security of the MD5 hash function is severely compromised, but
        /// is included for compatibility with system that still require it.
        /// </summary>
        /// <param name="fromString"></param>
        /// <returns></returns>
        public static byte[] Md5Hash(this string fromString)
        {
            using var md5Crypto = new MD5CryptoServiceProvider();

            md5Crypto.Initialize();
            var hashBytes = md5Crypto.ComputeHash(Encoding.UTF8.GetBytes(fromString));
            md5Crypto.Clear();
            return hashBytes ?? throw new ArgumentException($"Unable to compute hash for {fromString}", nameof(fromString));
        }
    }
}
