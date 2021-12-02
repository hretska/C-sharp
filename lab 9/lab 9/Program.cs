
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace lab9
{
    class Program
    {
        static void Main(string[] args)
        {
            string publicPath = "C:/repository-github-ira/GitHub/C-sharp/lab 9/PublicKey.xml";
            string signaturePath = "C:/repository-github-ira/GitHub/C-sharp/lab 9/Signature.dat";
            string dataPath = "C:/repository-github-ira/GitHub/C-sharp/lab 9/Data.dat";
            Console.WriteLine("1 - Create a signature for data from console; 2 - Verify signature with data from file");
            string a = Console.ReadLine();
            if (a == "1")
            {
                Console.WriteLine("Enter the text to sign");
                byte[] DataToSign = Encoding.UTF8.GetBytes(Console.ReadLine());
                File.WriteAllBytes(dataPath, DataToSign);
                byte[] signature = CreateSign(publicPath, DataToSign);
                File.WriteAllBytes(signaturePath, signature);
                Console.WriteLine(Encoding.ASCII.GetString(signature));
            }
            else if (a == "2")
            {
                byte[] DataToSign = File.ReadAllBytes(dataPath);
                byte[] signature = File.ReadAllBytes(signaturePath);
                if (VerifySignature(publicPath, DataToSign, signature))
                {

                    Console.WriteLine("Data verified");
                }
                else
                {
                    Console.WriteLine("Verification Incorrect");
                }
            }
            else
            {
                Console.WriteLine("Invalid Input");
            }

        }

        private readonly static string CspContainerName = "RsaContainer";
        public static void DeleteKeyInCsp()
        {
            CspParameters cspParameters = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            var rsa = new RSACryptoServiceProvider(2048, cspParameters)
            {
                PersistKeyInCsp = false
            };
            rsa.Clear();
        }
        public static byte[] CreateSign(string publicKeyPath, byte[] dataToSign)
        {
            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
            {
                rsa.PersistKeyInCsp = true;
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm(nameof(SHA512));
                byte[] hashOfData;
                using (var sha512 = SHA512.Create())
                {
                    hashOfData = sha512.ComputeHash(dataToSign);
                }
                return rsaFormatter.CreateSignature(hashOfData);

            }
        }
        public static bool VerifySignature(string publicKeyPath, byte[] data, byte[] signature)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm(nameof(SHA512));
                byte[] hashOfData;
                using (var sha512 = SHA512.Create())
                {
                    hashOfData = sha512.ComputeHash(data);

                }
                return rsaDeformatter.VerifySignature(hashOfData, signature);

            }
        }

    }
}