using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace lab7
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the data to encrypt");
            byte[] textToEncrypt = Encoding.UTF8.GetBytes(Console.ReadLine());
            AssignNewKey();

            var encText = EncryptData(textToEncrypt);
            var decText = DecryptData(encText);
            Console.WriteLine("Encrypted message: " + Convert.ToBase64String(encText));
            Console.WriteLine("Encrypted message: " + Encoding.ASCII.GetString(decText));
        }

        private static RSAParameters publicKey, privateKey;

        public static void AssignNewKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                publicKey = rsa.ExportParameters(false);
                privateKey = rsa.ExportParameters(true);

            }
        }

        public static byte[] EncryptData(byte[] dataToEncrypt)
        {
            byte[] cypherBytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = true;
                rsa.ImportParameters(publicKey);
                cypherBytes = rsa.Encrypt(dataToEncrypt, true);
            }
            return cypherBytes;
        }

        public static byte[] DecryptData(byte[] dataToDecrypt)
        {
            byte[] plainBytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = true;
                rsa.ImportParameters(privateKey);
                plainBytes = rsa.Decrypt(dataToDecrypt, true);
            }
            return plainBytes;
        }

    }
}