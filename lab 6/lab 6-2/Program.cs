using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace lab6_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the password to create the AES key");
            var aesKeyPass = Encoding.UTF8.GetBytes(Console.ReadLine());
            byte[] aesKeySalt = RandNum(32);
            var aesKey = Hash(aesKeyPass, aesKeySalt, 32);
            Console.WriteLine("Enter the password to create the AES IV");
            var aesIvPass = Encoding.UTF8.GetBytes(Console.ReadLine());
            byte[] aesIvSalt = RandNum(32);
            var aesIv = Hash(aesIvPass, aesIvSalt, 16);

            const string aestext = "Encrypt this with AES";
            var aesencrypted = aesCipher.Encrypt(Encoding.UTF8.GetBytes(aestext), aesKey, aesIv);
            var aesdecrypted = aesCipher.Decrypt(aesencrypted, aesKey, aesIv);
            Console.WriteLine("Original Text = " + aestext);
            Console.WriteLine("AES Encrypted Text = " + Convert.ToBase64String(aesencrypted));
            Console.WriteLine("AES Decrypted Text = " + Encoding.ASCII.GetString(aesdecrypted));
            Console.WriteLine();



            Console.WriteLine("Enter the password to create the DES key");
            var desKeyPass = Encoding.UTF8.GetBytes(Console.ReadLine());
            byte[] desKeySalt = RandNum(32);
            var desKey = Hash(desKeyPass, desKeySalt, 8);
            Console.WriteLine("Enter the password to create the des IV");
            var desIvPass = Encoding.UTF8.GetBytes(Console.ReadLine());
            byte[] desIvSalt = RandNum(32);
            var desIv = Hash(desIvPass, desIvSalt, 8);

            const string destext = "Encrypt this with DES";
            var desencrypted = desCipher.Encrypt(Encoding.UTF8.GetBytes(destext), desKey, desIv);
            var desdecrypted = desCipher.Decrypt(desencrypted, desKey, desIv);
            Console.WriteLine("Original Text = " + destext);
            Console.WriteLine("DES Encrypted Text = " + Convert.ToBase64String(desencrypted));
            Console.WriteLine("DES Decrypted Text = " + Encoding.ASCII.GetString(desdecrypted));
            Console.WriteLine();



            Console.WriteLine("Enter the password to create the 3DES key");
            var ThreedesKeyPass = Encoding.UTF8.GetBytes(Console.ReadLine());
            byte[] ThreedesKeySalt = RandNum(32);
            var ThreedesKey = Hash(ThreedesKeyPass, ThreedesKeySalt, 24);
            Console.WriteLine("Enter the password to create the 3DES IV");
            var ThreedesIvPass = Encoding.UTF8.GetBytes(Console.ReadLine());
            byte[] ThreedesIvSalt = RandNum(32);
            var ThreedesIv = Hash(ThreedesIvPass, ThreedesIvSalt, 8);

            const string threedestext = "Encrypt this with 3DES";
            var Threedesencrypted = TripledesCipher.Encrypt(Encoding.UTF8.GetBytes(threedestext), ThreedesKey, ThreedesIv);
            var Threedesdecrypted = TripledesCipher.Decrypt(Threedesencrypted, ThreedesKey, ThreedesIv);
            Console.WriteLine("Original Text = " + threedestext);
            Console.WriteLine("3DES Encrypted Text = " + Convert.ToBase64String(Threedesencrypted));
            Console.WriteLine("3DES Decrypted Text = " + Encoding.ASCII.GetString(Threedesdecrypted));
            Console.WriteLine();
        }
        public static byte[] Hash(byte[] pass, byte[] salt, int length)
        {
            using (var rfc = new Rfc2898DeriveBytes(pass, salt, 130000))
            {
                return rfc.GetBytes(length);
            }
        }
        public static byte[] RandNum(int length)
        {
            var RandNumGen = new RNGCryptoServiceProvider();
            byte[] Rn = new byte[length];
            RandNumGen.GetBytes(Rn);
            return Rn;
        }
    }
    public class aesCipher
    {
        public static byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
        public static byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
    }
    public class desCipher
    {
        public static byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {

            using (var des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
        public static byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
    }

    public class TripledesCipher
    {
        public static byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {

            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
        public static byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
    }

}