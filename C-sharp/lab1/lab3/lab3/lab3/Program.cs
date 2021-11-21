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
            //Завдання №1 
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

            //Завдання №2 
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
            //Отриманий пароль -> 20192020 

            //Завдання №3 
            var randGen = new RNGCryptoServiceProvider();
            byte[] key = new byte[32];
            randGen.GetBytes(key);
            File.WriteAllBytes("../../../key.txt", key);
            byte[] message = File.ReadAllBytes("../../../../../fileToHash.txt");
            var HMACkey = ComputeHmacsha1(message, key);
            Console.WriteLine("SHA1 HMAC");
            string hashMessage = Convert.ToBase64String(HMACkey);
            Console.WriteLine($"hash: {hashMessage}");

            //Перевірка 
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

            //Завдання №4 
            var logins = File.ReadLines("../../../../../logins.txt").ToList();
            var passHMAC = File.ReadLines("../../../../../passHMAC.txt").ToList();
            var keys = File.ReadLines("../../../../../keys.txt").ToList();

            bool valid = false;
            string what;
            while (!valid)
            {
                Console.WriteLine("Sign up - 1; Log in - 2");
                what = Console.ReadLine();
                switch (what)
                {
                    case "1":
                        SignUp(logins, passHMAC, keys);
                        valid = true;
                        break;
                    case "2":
                        LogIn(logins, passHMAC, keys);
                        valid = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Answer");
                        valid = false;
                        break;
                }
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
        public static void SignUp(List<string> logins, List<string> passHMAC, List<string> keys)
        {
            string YourLogin;
            string YourPassword;
            string RepeatPassword;
            bool validLogin = false;
            bool validPass = false;
            while (!validLogin)
            {
                Console.WriteLine("Enter your Login");
                YourLogin = Console.ReadLine();
                if (logins.Contains(YourLogin))
                {
                    Console.WriteLine("That Login is already in use");
                    validLogin = false;
                }
                else
                {
                    logins.Add(YourLogin);
                    validLogin = true;
                }
            }
            while (!validPass)
            {
                Console.WriteLine("Create a password");
                YourPassword = Console.ReadLine();
                Console.WriteLine("Repeat your password");
                RepeatPassword = Console.ReadLine();
                if (YourPassword == RepeatPassword)
                {
                    var randGen = new RNGCryptoServiceProvider();
                    byte[] key = new byte[32];
                    randGen.GetBytes(key);
                    keys.Add(Convert.ToBase64String(key));
                    var pass = Encoding.Unicode.GetBytes(YourPassword);
                    var HMACpass = ComputeHmacsha1(pass, key);
                    passHMAC.Add(Convert.ToBase64String(HMACpass));
                    Console.WriteLine("You have successfully signed up");
                    validPass = true;
                }
                else
                {
                    Console.WriteLine("Passwords don't match");
                    validPass = false;
                }
            }
            File.WriteAllLines("../../../../../logins.txt", logins);
            File.WriteAllLines("../../../../../keys.txt", keys);
            File.WriteAllLines("../../../../../passHMAC.txt", passHMAC);

        }
        public static void LogIn(List<string> logins, List<string> passHMAC, List<string> keys)
        {
            string YourLogin;
            string YourPassword;
            int indLogin;
            string key = "";
            string hashpass = "";
            bool validLogin = false;
            bool validPass = false;
            while (!validLogin)
            {
                Console.WriteLine("Enter your Login");
                YourLogin = Console.ReadLine();
                if (logins.Contains(YourLogin))
                {
                    indLogin = logins.IndexOf(YourLogin);
                    key = keys[indLogin];
                    hashpass = passHMAC[indLogin];
                    validLogin = true;
                }
                else
                {
                    Console.WriteLine("User with this Login does not exist");
                    validLogin = false;
                }
            }
            while (!validPass)
            {
                Console.WriteLine("Enter your Password");
                YourPassword = Console.ReadLine();
                byte[] pass = Encoding.Unicode.GetBytes(YourPassword);
                var thisHmac = ComputeHmacsha1(pass, Convert.FromBase64String(key));
                if (Convert.ToBase64String(thisHmac) == hashpass)
                {
                    Console.WriteLine("You have successfully Logged In");
                    validPass = true;
                }
                else
                {
                    Console.WriteLine("Invalid Password");
                    validPass = false;
                }
            }

        }

    }
}