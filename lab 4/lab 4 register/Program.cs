using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace lab4_5____register
{
    class Program
    {
        static void Main(string[] args)
        {
            var logins = File.ReadLines("Login.txt").ToList();
            var passHash = File.ReadLines("Hash.txt").ToList();
            var salt = File.ReadLines("Salt.txt").ToList();
            bool valid = false;
            string what;
            while (!valid)
            {
                Console.WriteLine("Sign up - 1; Log in - 2");
                what = Console.ReadLine();
                switch (what)
                {
                    case "1":
                        SignUp(logins, passHash, salt);
                        valid = true;
                        break;
                    case "2":
                        LogIn(logins, passHash, salt);
                        valid = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Answer");
                        valid = false;
                        break;
                }
            }
        }
        public static void SignUp(List<string> logins, List<string> passHash, List<string> salt)
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
                var passSalt = PBKDF2.GenerateSalt();
                salt.Add(Convert.ToBase64String(passSalt));
                var pass = Encoding.Unicode.GetBytes(YourPassword);
                var hashpass = PBKDF2.HashPassword(pass, passSalt, 80000);
                passHash.Add(Convert.ToBase64String(hashpass));
                Console.WriteLine("You have successfully signed up");
                validPass = true;
            }
            File.WriteAllLines("Login.txt", logins);
            File.WriteAllLines("Hash.txt", passHash);
            File.WriteAllLines("Salt.txt", salt);

        }
        public static void LogIn(List<string> logins, List<string> passHash, List<string> salt)
        {
            string YourLogin;
            string YourPassword;
            int indLogin;
            string passSalt = "";
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
                    passSalt = salt[indLogin];
                    hashpass = passHash[indLogin];
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
                var hash = PBKDF2.HashPassword(pass, Convert.FromBase64String(passSalt), 80000);
                if (Convert.ToBase64String(hash) == hashpass)
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
}