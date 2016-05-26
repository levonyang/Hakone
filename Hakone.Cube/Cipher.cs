using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.Cube
{
    public class Cipher
    {
        private const char COMPRESSED = 'C';
        private const char NOTCOMPRESSED = 'P';
        private const int MINIMUM_LENGTH_FOR_COMPRESSION = 512;
        private const string HASH_ALGORITHM = "SHA1";
        private const int PASSWORD_ITERATIONS = 2;
        private const int KEY_SIZE = 256;



        public static string GenerateSalt()
        {
            byte[] bytes = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] bytes = new byte[value.Length + salt.Length];
            Buffer.BlockCopy(salt, 0, bytes, 0, salt.Length);
            Buffer.BlockCopy(value, 0, bytes, salt.Length, value.Length);
            return (new SHA1CryptoServiceProvider()).ComputeHash(bytes);
        }

        public static string Hash(string value, string salt)
        {
            byte[] valueBytes = Encoding.Unicode.GetBytes(value);
            byte[] saltBytes = Convert.FromBase64String(salt);
            return Convert.ToBase64String(Hash(valueBytes, saltBytes));
        }
    }
}
