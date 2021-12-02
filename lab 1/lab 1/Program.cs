using System;
using System.Security.Cryptography;

namespace Lab1
{
    class Program
    {

        static void Main(string[] args)
        {
            Random rand = new Random(1);
            Random rand1 = new Random(1234);
            Random rand2 = new Random(1);
            for (int i = 0; i < 10; i++)
            {
                Console.Write(rand.Next(1, 10));
            }
            Console.WriteLine("");
            for (int i = 0; i < 10; i++)
            {
                Console.Write(rand1.Next(1, 10));
            }
            Console.WriteLine("");
            for (int i = 0; i < 10; i++)
            {
                Console.Write(rand2.Next(1, 10));
            }
            Console.WriteLine("");
            //при різних початкових значеннях випадкові числа різні. при однакових - однакові


            var randGen = new RNGCryptoServiceProvider();
            var randNum = new byte[15];
            for (int i = 0; i < 10; i++)
            {
                randGen.GetBytes(randNum);
                String res = Convert.ToBase64String(randNum);
                Console.WriteLine(res);
            }
            //послідовності різні

        }
    }
}