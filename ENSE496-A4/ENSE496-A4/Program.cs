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
            int seed = 802797117; //Seed created using Requirement 3
            Console.Write(" The value of the seed is: ");
            Console.WriteLine(seed);
            p.RandomGenerator(); //Generating a list of random integers and loading them into the file "OutputAlice.txt"
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
                    int num = random.Next(0, 100000);
                    sw.WriteLine(num + ",");
                }
            }
        }
        //Compares the bytes of 2 files
        static bool FileEquals(String fPath1, String fPath2) //File Comparision function
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
        public void DHAlgoroith() //Deffie-Hellman algorithm to find a shared key to be used as Seed
        {
            BigInteger P, g, a, b, x, y, sa, sb;
            P = 1000000007; //Prime number P=1'000'000'000'007
            g = 2; //Primative root for p
            a = 599; //private key of alice (Achter in ASCII)
            b = 389;// private key of Bob (Nick in ASCII)
            x = PowerFunct(g, a, P); //Bobs generated key
            y = PowerFunct(g, b, P); //Alice's generated key
            sa = PowerFunct(y, a, P); // Secret key for Alive
            sb = PowerFunct(x, b, P); //Secret key for Bob
            Console.Write(" The Secret Key for Alice is ", sa);
            Console.WriteLine(sa);
            Console.Write(" The Secret Key for Bob is ", sb);
            Console.WriteLine(sb);
        }

        public BigInteger PowerFunct(BigInteger a, BigInteger b, BigInteger P) //Powerfunction to use BigInteger values instead of ulong
        {
            BigInteger temp = 1; //Begin with a temp variable set to 1
            if (b == 1) //Base case of b=1
                return a;
            else
                for (int i = 1; i <= b; i++) //iterate through 'b' number of times
                {
                temp *= a; //Power function
                }
            Console.Write(temp);
                return (temp % P); //Modular function

        }
    }

}
