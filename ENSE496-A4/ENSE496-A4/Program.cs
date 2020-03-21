using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace ENSE496_A4
{
    class Program
    {
      
        static void Main(string[] args)
        {
            Program p = new Program();
            int seed = 802797117;
            Console.Write(" The value of the seed is: ");
            Console.WriteLine(seed);
            p.RandomGenerator();
            Console.WriteLine("Creating File OutputAlice.txt");
            Console.Write("The two files, OutputAlice and OutputBob are equal: ");
          bool FileCompare = FileEquals("C:/Users/Nickolas/Documents/GitHub/ENSE496-Assignment4/ENSE496-A4/ENSE496-A4/OutputAlice.txt",
             "C:/Users/Nickolas/Documents/GitHub/ENSE496-Assignment4/ENSE496-A4/ENSE496-A4/OutputBob.txt");
            Console.WriteLine(FileCompare);
        }


        public void RandomGenerator()
        {
            int seed = 802797117;
            Random random = new Random(seed);
            StreamWriter sw = new StreamWriter("C:/Users/Nickolas/Documents/GitHub/ENSE496-Assignment4/ENSE496-A4/ENSE496-A4/OutputAlice.txt");
            using (sw)
            { 
                for (var i = 0; i < 500; i++)
                {
                    int num = random.Next(0, 100000);  //Range of a signed 16-bit int
                    sw.WriteLine(num + ",");
                }
            }
        }
        //Compares the bytes of 2 files
        static bool FileEquals(String fPath1, String fPath2)
        {
            byte[] file1 = File.ReadAllBytes(fPath1); //File 1
            byte[] file2 = File.ReadAllBytes(fPath2); //File 2
            for(int i=0; i<file1.Length; i++) //Assuming files are equal sizes
            {
                if(file1[i] != file2[i])
                {
                    return false;
                }
            }
            return true;
        }
        public void DHAlgoroith()
        {
            long P, g, a, b;
            double x, y, ka, kb;
            P = 17; //Prime number P
            g = 5; //Primative root for p
            a = 599; //private key of alice (Achter in ASCII)
            b = 389;// private key of Bob (Nick in ASCII)

            x = (Math.Pow(g, a)) % P; //Bobs generated key
            y = (Math.Pow(g, b) % P); //Alice's generated key

            ka = (Math.Pow(y, a) % P); // Secret key for Alive
            kb = (Math.Pow(x, b) % P); //Seccret key for Bob
        }

        public ulong PowerFunct(ulong a, ulong b, ulong P)
        {
            ulong temp = 0;
            ulong i;
            if (a == b)
                return a;
            else
                for (i = 0; i < b; i++)
                { 
                temp += a * a;
                }
                return (temp % P);
                
        }
    }

}


