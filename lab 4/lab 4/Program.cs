using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace lab4_5_
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the password: ");
            string password = Console.ReadLine();
            byte[] salt = SaltedHash.GenerateSalt();
            Console.WriteLine("Password: " + password);
            Console.WriteLine("Salt = " + Convert.ToBase64String(salt));
            Console.WriteLine();
            var hashedPassword = SaltedHash.HashPasswordWithSalt(Encoding.UTF8.GetBytes(password), salt);
            Console.WriteLine("Hashed Password = " + Convert.ToBase64String(hashedPassword));
            Console.WriteLine();

            
            Console.WriteLine("Enter the password: ");
            string password1 = Console.ReadLine();
            int IterStart = 80000;
            for (int i = 0; i < 10; i++)
            {
                var sw = new Stopwatch();
                sw.Start();
                byte[] salt1 = PBKDF2.GenerateSalt();
                var hashedPassword1 = PBKDF2.HashPassword(Encoding.UTF8.GetBytes(password1), salt1, IterStart);
                Console.WriteLine("Password: " + password1);
                Console.WriteLine("Salt = " + Convert.ToBase64String(salt1));
                Console.WriteLine("Hashed Password = " + Convert.ToBase64String(hashedPassword1));
                Console.WriteLine("Iterations <" + IterStart + "> Elapsed Time: " + sw.ElapsedMilliseconds + "ms");
                Console.WriteLine();
                IterStart += 50000;
            }


        }

    }
    public class PBKDF2
    {
        public static byte[] GenerateSalt()
        {
            const int saltLength = 32;
            using (var randomNumberGenerator =
            new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }
        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(
            toBeHashed, salt, numberOfRounds))
            {
                return rfc2898.GetBytes(20);
            }
        }
    }

    public class SaltedHash
    {
        public static byte[] GenerateSalt()
        {
            const int saltLength = 32;
            using (var randomNumberGenerator =
            new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }
        private static byte[] Combine(byte[] pass, byte[] salt)
        {
            var ret = new byte[pass.Length + salt.Length];
            Buffer.BlockCopy(pass, 0, ret, 0, pass.Length);
            Buffer.BlockCopy(salt, 0, ret, pass.Length, salt.Length);
            return ret;
        }
        public static byte[] HashPasswordWithSalt(
        byte[] toBeHashed, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Combine(toBeHashed,
                salt));
            }
        }
    }
}