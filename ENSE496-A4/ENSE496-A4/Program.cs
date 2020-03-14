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
        public int seed = 0;
        static void Main(string[] args)
        {
            Program p = new Program();
           // p.RandomGenerator();
            //bool FileCompare = FileEquals("C:/Users/Nickolas/Documents/GitHub/ENSE496-Assignment4/ENSE496-A4/ENSE496-A4/OutputA.txt",
              //  "C:/Users/Nickolas/Documents/GitHub/ENSE496-Assignment4/ENSE496-A4/ENSE496-A4/OutputANic.txt");
            //Console.WriteLine(FileCompare);
          //  p.DHAlgoroith();
            p.PowerFunct(5, 238, 13);
        }


        public void RandomGenerator()
        {
            Random random = new Random(42069);
            StreamWriter sw = new StreamWriter("C:/Users/Nickolas/Documents/GitHub/ENSE496-Assignment4/ENSE496-A4/ENSE496-A4/OutputA.txt");
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

        public BigInteger PowerFunct(BigInteger a, BigInteger b, BigInteger P)
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


