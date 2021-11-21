using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Text;

namespace lab2
{
    class Program
    {
        static void Main(string[] args)
        {

            //getting text from a file 
            byte[] data = File.ReadAllBytes("../../../files/file.txt").ToArray();
            foreach (byte i in data)
            {
                Console.Write(i);
                Console.Write(" ");
            }
            Console.WriteLine();

            //generating a random key 
            var generator = new RNGCryptoServiceProvider();
            byte[] key = new byte[data.Length];
            generator.GetBytes(key);
            foreach (byte i in key)
            {
                Console.Write(i);
                Console.Write(" ");
            }
            Console.WriteLine();

            //encrypting bytes 
            byte[] dataEn = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                dataEn[i] = (byte)(data[i] ^ key[i]);
            }


            //writing encrypted bytes in a .dat file 
            File.WriteAllBytes("../../../files/encdata.dat", dataEn);
            foreach (byte i in dataEn)
            {
                Console.Write(i);
                Console.Write(" ");
            }

            Console.WriteLine();

            //decoding file 
            byte[] Message = File.ReadAllBytes("../../../files/encdata.dat").ToArray();
            byte[] MessageDec = new byte[Message.Length];
            for (int i = 0; i < Message.Length; i++)
            {
                MessageDec[i] = (byte)(Message[i] ^ key[i]);
            }
            Console.Write(Encoding.ASCII.GetString(MessageDec));

        }
    }
}
