
using System;
using System.Security.Cryptography;

namespace lab1
{
    class Program
    {

        static void Main(string[] args)
        {
            Random rand = new Random(5);
            Random rand1 = new Random(56);
            Random rand2 = new Random(5);
            for (int i = 0; i < 15; i++)
            {
                Console.Write(rand.Next(1,10));
            }
            Console.WriteLine("");
            for (int i = 0; i < 15; i++)
            {
                Console.Write(rand1.Next(1,10));
            }
            Console.WriteLine("");
            for (int i = 0; i < 15; i++)
            {
                Console.Write(rand2.Next(1,1
                    0));
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
