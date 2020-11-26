using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace xApi.Data
{
    public class SHAHelper
    {
        public class SHA1
        {
            /// <summary>
            /// Compute hash for string encoded as UTF8
            /// </summary>
            /// <param name="s">String to be hashed</param>
            /// <returns>40-character hex string</returns>
            public static string ComputeHash(string s)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                return ComputeHash(bytes);
            }

            /// <summary>
            /// Compute hash for bytes
            /// </summary>
            /// <param name="s">String to be hashed</param>
            /// <returns>40-character hex string</returns>
            public static string ComputeHash(byte[] bytes)
            {
                var sha1 = new SHA1Managed();
                sha1.ComputeHash(bytes);
                return HexStringFromBytes(sha1.Hash);
            }
        }

        public class SHA2
        {
            /// <summary>
            /// Compute hash for string encoded as UTF8
            /// </summary>
            /// <param name="s">String to be hashed</param>
            /// <returns>40-character hex string</returns>
            public static string ComputeHash(string s)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                return ComputeHash(bytes);
            }

            /// <summary>
            /// Compute hash for bytes
            /// </summary>
            /// <param name="s">String to be hashed</param>
            /// <returns>40-character hex string</returns>
            public static string ComputeHash(byte[] bytes)
            {
                var sha2 = new SHA512Managed();
                sha2.ComputeHash(bytes);
                return HexStringFromBytes(sha2.Hash);
            }
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}