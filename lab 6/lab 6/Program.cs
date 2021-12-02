using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace lab6
{
    class Program
    {
        static void Main(string[] args)
        {

            var aeskey = RandNum(32);
            var aesiv = RandNum(16);
            const string text = "Text to encrypt";
            var aesencrypted = aesCipher.Encrypt(Encoding.UTF8.GetBytes(text), aeskey, aesiv);
            var aesdecrypted = aesCipher.Decrypt(aesencrypted, aeskey, aesiv);
            Console.WriteLine("Original Text = " + text);
            Console.WriteLine("AES Encrypted Text = " + Convert.ToBase64String(aesencrypted));
            Console.WriteLine("AES Decrypted Text = " + Encoding.ASCII.GetString(aesdecrypted));

            Console.WriteLine();

            var deskey = RandNum(8);
            var desiv = RandNum(8);
            var desencrypted = desCipher.Encrypt(Encoding.UTF8.GetBytes(text), deskey, desiv);
            var desdecrypted = desCipher.Decrypt(desencrypted, deskey, desiv);
            Console.WriteLine("Original Text = " + text);
            Console.WriteLine("DES Encrypted Text = " + Convert.ToBase64String(desencrypted));
            Console.WriteLine("DES Decrypted Text = " + Encoding.ASCII.GetString(desdecrypted));

            Console.WriteLine();

            var tripledeskey = RandNum(24);
            var tripledesiv = RandNum(8);
            var tripledesencrypted = TripledesCipher.Encrypt(Encoding.UTF8.GetBytes(text), tripledeskey, tripledesiv);
            var tripledesdecrypted = TripledesCipher.Decrypt(tripledesencrypted, tripledeskey, tripledesiv);
            Console.WriteLine("Original Text = " + text);
            Console.WriteLine("3DES Encrypted Text = " + Convert.ToBase64String(tripledesencrypted));
            Console.WriteLine("3DES Decrypted Text = " + Encoding.ASCII.GetString(tripledesdecrypted));

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