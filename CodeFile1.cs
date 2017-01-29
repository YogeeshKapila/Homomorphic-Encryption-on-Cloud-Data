using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Globalization;
using Extreme.Mathematics;
using System.Threading;
using System.IO;
using evoting;

namespace evoting
{
    public class eevoting : evoting  
    {


        public static void main(String[] arr)
        {
            eevoting e1 = new eevoting();

            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader("C:\\Users\\Yogi\\Documents\\Visual Studio 2012\\Projects\\ConsoleApplication1\\ConsoleApplication1\\bin\\Debug\\Encrypted Votes.txt");
            
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }

            file.Close();

            // Suspend the screen.
            Console.ReadLine();
            
            
            //e1.Decryption();
 
        }
    }

}