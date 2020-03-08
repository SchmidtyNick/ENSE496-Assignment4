using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ENSE496_A4
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.RandomGenerator();
            bool FileCompare = FileEquals("C:/Users/Nickolas/Documents/GitHub/ENSE496-Assignment4/ENSE496-A4/ENSE496-A4/OutputA.txt",
                "C:/Users/Nickolas/Documents/GitHub/ENSE496-Assignment4/ENSE496-A4/ENSE496-A4/OutputANic.txt");
            Console.WriteLine(FileCompare);
        }


        public void RandomGenerator()
        {
            var rand = new Random(42069); //Random number generator with seed 42069
           
            StreamWriter sw = new StreamWriter("C:/Users/Nickolas/Documents/GitHub/ENSE496-Assignment4/ENSE496-A4/ENSE496-A4/OutputA.txt");
            using (sw)
            { 
                for (var i = 0; i < 500; i++)
                {
                    int num = rand.Next(-100000, 100000);  //Range of a signed 16-bit int
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
    }

}


