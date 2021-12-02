using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            //завдання 1
            string str1 = "hash this";
            var MD5hashStr1 = ComputeHashMd5(Encoding.Unicode.GetBytes(str1));
            var SHA1hashStr1 = ComputeHashSha1(Encoding.Unicode.GetBytes(str1));
            var SHA256hashStr1 = ComputeHashSha256(Encoding.Unicode.GetBytes(str1));
            var SHA384hashStr1 = ComputeHashSha384(Encoding.Unicode.GetBytes(str1));
            var SHA512hashStr1 = ComputeHashSha512(Encoding.Unicode.GetBytes(str1));
            Guid guid1 = new Guid(MD5hashStr1);
            Console.WriteLine($"Str1: {str1}");
            Console.WriteLine($"Str1 Hash MD5: {Convert.ToBase64String(MD5hashStr1)}");
            Console.WriteLine($"Str1 GUID: {guid1}");
            Console.WriteLine($"Str1 Hash SHA1: {Convert.ToBase64String(SHA1hashStr1)}");
            Console.WriteLine($"Str1 Hash SHA256: {Convert.ToBase64String(SHA256hashStr1)}");
            Console.WriteLine($"Str1 Hash SHA384: {Convert.ToBase64String(SHA384hashStr1)}");
            Console.WriteLine($"Str1 Hash SHA512: {Convert.ToBase64String(SHA512hashStr1)}");
            Console.WriteLine();

            string str2 = "ihuDTu=^&%v3846``1!gG=-";
            var MD5hashStr2 = ComputeHashMd5(Encoding.Unicode.GetBytes(str2));
            var SHA1hashStr2 = ComputeHashSha1(Encoding.Unicode.GetBytes(str2));
            var SHA256hashStr2 = ComputeHashSha256(Encoding.Unicode.GetBytes(str2));
            var SHA384hashStr2 = ComputeHashSha384(Encoding.Unicode.GetBytes(str2));
            var SHA512hashStr2 = ComputeHashSha512(Encoding.Unicode.GetBytes(str2));
            Guid guid2 = new Guid(MD5hashStr2);
            Console.WriteLine($"Str2: {str2}");
            Console.WriteLine($"Str2 Hash MD5: {Convert.ToBase64String(MD5hashStr2)}");
            Console.WriteLine($"Str2 GUID: {guid2}");
            Console.WriteLine($"Str2 Hash SHA1: {Convert.ToBase64String(SHA1hashStr2)}");
            Console.WriteLine($"Str2 Hash SHA256: {Convert.ToBase64String(SHA256hashStr2)}");
            Console.WriteLine($"Str2 Hash SHA384: {Convert.ToBase64String(SHA384hashStr2)}");
            Console.WriteLine($"Str2 Hash SHA512: {Convert.ToBase64String(SHA512hashStr2)}");
            Console.WriteLine();

            //завдання 2 відновлення пароля
            string password;
            for (int i = 0; i <= 99999999; i++)
            {
                string pass = i.ToString();
                while (pass.Length < 8)
                {
                    pass = "0" + pass;
                }
                var MD5hashPass = ComputeHashMd5(Encoding.Unicode.GetBytes(pass));
                string hashStr = Convert.ToBase64String(MD5hashPass);
                Guid guidPass = new Guid(MD5hashPass);
                string guidStr = guidPass.ToString();
                if ((hashStr == "po1MVkAE7IjUUwu61XxgNg==") && (guidStr == "564c8da6-0440-88ec-d453-0bbad57c6036"))
                {
                    password = pass;
                    Console.WriteLine(password);
                    break;
                }
            }
            Console.WriteLine();
            //пароль - 20192020

            //завдання 3
            var randGen = new RNGCryptoServiceProvider();
            byte[] key = new byte[32];
            randGen.GetBytes(key);
            File.WriteAllBytes("../../../key.txt", key);
            byte[] message = File.ReadAllBytes("../../../fileToHash.txt");
            var HMACkey = ComputeHmacsha1(message, key);
            Console.WriteLine("SHA1 HMAC");
            string hashMessage = Convert.ToBase64String(HMACkey);
            Console.WriteLine($"hash: {hashMessage}");

            //перевірка
            byte[] messageTest = File.ReadAllBytes("../../../fileToHash.txt");
            var key2 = File.ReadAllBytes("../../../key.txt");
            var testHMACkey = ComputeHmacsha1(messageTest, key2);
            if (Convert.ToBase64String(testHMACkey) == hashMessage)
            {
                Console.WriteLine("Authentic");
            }
            else
            {
                Console.WriteLine("Not Authentic");
            }
        }

        static byte[] ComputeHashMd5(byte[] byteForHash)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(byteForHash);
            }
        }
        public static byte[] ComputeHashSha1(byte[] toBeHashed)
        {
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(toBeHashed);
            }
        }
        public static byte[] ComputeHashSha256(byte[] toBeHashed)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(toBeHashed);
            }
        }
        public static byte[] ComputeHashSha384(byte[] toBeHashed)
        {
            using (var sha384 = SHA384.Create())
            {
                return sha384.ComputeHash(toBeHashed);
            }
        }
        public static byte[] ComputeHashSha512(byte[] toBeHashed)
        {
            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(toBeHashed);
            }
        }
        public static byte[] ComputeHmacsha1(byte[] toBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA1(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }
    }
}
        