using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace lab8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Encrypt message from console - 1, Decrypt encrypted message from file - 2, Assign a new pair of keys - 3");
            string i = Console.ReadLine();
            if (i == "1")
            {
                Console.WriteLine("Please enter the data to encrypt: ");
                byte[] textToEncrypt = Encoding.UTF8.GetBytes(Console.ReadLine());
                Console.WriteLine("Please enter the name of the file to read the open key: ");
                string filename = Console.ReadLine();
                string publicPath = "C:/repository-github-ira/GitHub/C-sharp/lab 8/lab 8/" + filename + ".xml";
                var encText = EncryptData(textToEncrypt, publicPath);
                File.WriteAllBytes("C:/repository-github-ira/GitHub/C-sharp/lab 8/lab 8/" + filename + ".dat", encText);
                Console.WriteLine("Encrypted message: " + Convert.ToBase64String(encText));
            }
            else if (i == "2")
            {
                Console.WriteLine("Please enter the name of the file with the message: ");
                string filename = Console.ReadLine();
                string publicPath = "C:/repository-github-ira/GitHub/C-sharp/lab 8/lab 8/" + filename + ".dat";
                var encText = File.ReadAllBytes(publicPath);
                var decText = DecryptData(encText);
                Console.WriteLine("Decrypted message: " + Encoding.UTF8.GetString(decText));
            }
            else if (i == "3")
            {
                Console.WriteLine("Please enter the name of the file to write the open key: ");
                string filename = Console.ReadLine();
                string publicPath = "C:/repository-github-ira/GitHub/C-sharp/lab 8/lab 8/" + filename + ".xml";
                AssignNewKey(publicPath);
            }
        }


        private readonly static string CspContainerName = "RsaContainer";

        public static void AssignNewKey(string publicKeyPath)
        {
            CspParameters cspParameters = new CspParameters(1)
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore,
                ProviderName = "Microsoft Strong Cryptographic Provider"
            };
            using (var rsa = new RSACryptoServiceProvider(cspParameters))
            {
                rsa.PersistKeyInCsp = true;
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
            }
        }

        public static byte[] EncryptData(byte[] dataToEncrypt, string path)
        {
            byte[] cypherBytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(File.ReadAllText(path));
                cypherBytes = rsa.Encrypt(dataToEncrypt, true);
            }
            return cypherBytes;
        }

        public static byte[] DecryptData(byte[] dataToDecrypt)
        {
            byte[] plainBytes;
            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            using (var rsa = new RSACryptoServiceProvider(cspParams))
            {
                rsa.PersistKeyInCsp = true;
                plainBytes = rsa.Decrypt(dataToDecrypt, true);
            }
            return plainBytes;
        }

    }
}