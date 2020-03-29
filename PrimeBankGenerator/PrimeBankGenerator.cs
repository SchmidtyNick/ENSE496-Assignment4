using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Numerics;

namespace ENSE496_A4
{
    class PrimeBankGenerator
    {
        static void Main(string[] args)
        {
            PrimeBankGenerator p = new PrimeBankGenerator();
            p.primeGenerator();
        }


        public void primeGenerator()
        {
            BigInteger p = 2147483629; //starting prime
            int i = 1;  //prime counter
            StreamWriter sw = new StreamWriter("E:/Documents/GitHub/ENSE496-Assignment4/PrimeBankNew4.txt");  //prime bank file path
            Stopwatch stopwatch = new Stopwatch();  //timer
            long dur = 0;   //timer duration

            using (sw)
                //run until the time to get the a prime takes at least 3600000ms or 1hr
                while (dur < 3600000)
                {
                    stopwatch.Start();      //start timer
                    
                    //increment p and check if it is prime
                    while (!isPrime(p))
                        p++;
                    
                    stopwatch.Stop();       //stop timer
                    dur = stopwatch.ElapsedMilliseconds;
                    stopwatch.Reset();

                    sw.WriteLine(p + "," + dur);  //output prime
                    Console.WriteLine("Prime " + i + ": " + p + " Time: " + dur + "ms");
                    Console.WriteLine("");

                    i++;
                    p++;
                }
        }

        //Compares the bytes of 2 files
        public bool isPrime(BigInteger n)
        {
            if (n <= 1)
                return false;
            for (var i = 2; i < n; i++)
                if (n % i == 0)
                    return false;
            return true;
        }
    }

}


