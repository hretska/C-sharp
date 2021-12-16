using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.IO;

namespace lab11
{
    class Program
    {
        static void Main(string[] args)
        {
            string what;
            while (true)
            {
                Console.WriteLine("To sign up write - 1; To log in write - 2");
                what = Console.ReadLine();
                switch (what)
                {
                    case "1":
                        Protector.Register();
                        break;
                    case "2":
                        Protector.LogIn();
                        break;
                    default:
                        Console.WriteLine("Your input is invalid");
                        break;
                }
            }
        }

    }
    public class User
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string[] Roles { get; set; }
    }
    public class Protector
    {
        private static Dictionary<string, User> _users = new Dictionary<string, User>();
        public static User Register()
        {
            Console.WriteLine("Create your Username: ");
            string userName = Console.ReadLine();
            if (_users.ContainsKey(userName))
            {
                Console.WriteLine("User with this login already exists");
                return null;
            }
            Console.WriteLine("Create your password: ");
            string password = Console.ReadLine();
            Console.WriteLine("Enter up to four assigned roles [admin, mod, editor, user]. If you have more than one role, press Enter after entering each one. Enter end to finish: ");
            string rol = "";
            string[] roles = new string[4];
            int rolesAmount = 0;
            while (rol != "Fin" && rolesAmount < 4)
            {
                rol = Console.ReadLine();
                if (rol == "admin" || rol == "mod" || rol == "editor" || rol == "user")
                {
                    roles[rolesAmount] = rol;
                    rolesAmount++;
                }
                else if (rol == "end")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            }
            User a = new User();
            byte[] salt = GenerateSalt();
            byte[] hash = HashPassword(Encoding.Unicode.GetBytes(password), salt);
            a.Login = userName;
            a.PasswordHash = Convert.ToBase64String(hash);
            a.Salt = Convert.ToBase64String(salt);
            a.Roles = roles;
            _users.Add(userName, a);
            Console.WriteLine("You have successfully registered");
            return null;
        }
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
        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(
            toBeHashed, salt, 80000))
            {
                return rfc2898.GetBytes(20);
            }
        }
        public static void LogIn()
        {
            Console.WriteLine("Enter your Username: ");
            string userName = Console.ReadLine();
            if (!(_users.ContainsKey(userName)))
            {
                Console.WriteLine("User with this Login does not exist");
            }
            else
            {
                Console.WriteLine("Enter your password: ");
                string password = Console.ReadLine();
                User a = _users.GetValueOrDefault(userName);
                byte[] salt = Convert.FromBase64String(a.Salt);
                string hash = a.PasswordHash;
                string hashCheck = Convert.ToBase64String(HashPassword(Encoding.Unicode.GetBytes(password), salt));
                if (hash != hashCheck)
                {
                    Console.WriteLine("Password Incorrect");
                }
                else
                {
                    Console.WriteLine("You have successfully logged in");
                    var identity = new GenericIdentity(userName, "OIBAuth");
                    var principal = new GenericPrincipal(identity, _users[userName].Roles);
                    Thread.CurrentPrincipal = principal;
                    try
                    {
                        OnlyForAdminsFeature();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.GetType()}: {ex.Message}");
                    }
                }
            }
        }
        public static void OnlyForAdminsFeature()
        {
            if (Thread.CurrentPrincipal == null)
            {
                throw new SecurityException("Thread.CurrentPrincipal cannot be null.");
            }
            else if (!Thread.CurrentPrincipal.IsInRole("admin"))
            {
                Console.WriteLine("User must be an admin to access this feature.");
            }
            else
            {
                Console.WriteLine("You are an admin. You now have the access to the secure feature");
                
            }

        }
    }
}