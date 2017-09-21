using System;
using System.Security.Cryptography;

namespace Stock.UI.Utils
{
    public class HashGenerator
    {
        public byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            var plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
                plainTextWithSaltBytes[i] = plainText[i];
            for (int i = 0; i < salt.Length; i++)
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public string GenerateSaltedHash(string text, string salt)
        {
            byte[] textBytes = GetBytes(text);
            byte[] saltBytes = GetBytes(salt);

            byte[] hash = GenerateSaltedHash(textBytes, saltBytes);
            
            return Convert.ToBase64String(hash);
        }

        public string GenerateHash(string text, out string salt)
        {
            var textBytes = GetBytes(text);
            var saltBytes = new byte[32];
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltBytes);

            var saltStr = Convert.ToBase64String(saltBytes);
            saltBytes = GetBytes(saltStr);
            var hashBytes = GenerateSaltedHash(textBytes, saltBytes);
            
            salt = saltStr;
            return Convert.ToBase64String(hashBytes);
        }

        private string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
