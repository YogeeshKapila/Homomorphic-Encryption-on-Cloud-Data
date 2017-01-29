using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Extreme.Mathematics;
using System.Threading;
using System.IO;


namespace evoting
{
    public class evoting
    {

        /**
    * p and q are two large primes. 
    * lambda = lcm(p-1, q-1) = (p-1)*(q-1)/gcd(p-1, q-1).
    */
        public Extreme.Mathematics.BigInteger p, q, lambda;
        /**
         * n = p*q, where p and q are two large primes.
         */
        public Extreme.Mathematics.BigInteger n;
        /**
         * nsquare = n*n
         */
        public Extreme.Mathematics.BigInteger nsquare;
        /**
         * a random integer in Z*_{n^2} where gcd (L(g^lambda mod n^2), n) = 1.
         */
        public Extreme.Mathematics.BigInteger g;
        /**
         * number of bits of modulus
         */
        public int bitLength;


        public evoting(int bitLengthVal, int certainty)
        {
            KeyGeneration(bitLengthVal, certainty);
        }

        public evoting()
        {
            KeyGeneration(512, 64);
        }
        public void KeyGeneration(int bitLengthVal, int certainty)
        {
            bitLength = bitLengthVal;
            // string hexstring = "7";
            //string hexstring1 = "B";
            /*Constructs two randomly generated positive BigIntegers that are probably prime, with the specified bitLength and certainty.*/
            //p = Extreme.Mathematics.BigInteger.Parse(hexstring,NumberStyles.AllowHexSpecifier);
            //q = Extreme.Mathematics.BigInteger.Parse(hexstring1, NumberStyles.AllowHexSpecifier);

            p = new Extreme.Mathematics.BigInteger(11715168891919189253);
            q = new Extreme.Mathematics.BigInteger(9739237618060016849);

            n = Extreme.Mathematics.BigInteger.Multiply(p, q);
            nsquare = Extreme.Mathematics.BigInteger.Pow(n, 2);
            g = new Extreme.Mathematics.BigInteger(2);

            Extreme.Mathematics.BigInteger x1 = Extreme.Mathematics.BigInteger.Multiply(Extreme.Mathematics.BigInteger.Subtract(p, Extreme.Mathematics.BigInteger.One), (Extreme.Mathematics.BigInteger.Subtract(q, Extreme.Mathematics.BigInteger.One)));
            Extreme.Mathematics.BigInteger x2 = Extreme.Mathematics.BigInteger.GreatestCommonDivisor(Extreme.Mathematics.BigInteger.Subtract(p, Extreme.Mathematics.BigInteger.One), Extreme.Mathematics.BigInteger.Subtract(q, Extreme.Mathematics.BigInteger.One));

            lambda = Extreme.Mathematics.BigInteger.Divide(x1, x2);

        }
        /**
     * Encrypts plaintext m. ciphertext c = g^m * r^n mod n^2. This function explicitly requires random input r to help with encryption.
     * @param m plaintext as a BigInteger
     * @param r random plaintext to help with encryption
     * @return ciphertext as a BigInteger
     */


        public Extreme.Mathematics.BigInteger Encryption(Extreme.Mathematics.BigInteger m, Extreme.Mathematics.BigInteger r)
        {
            return Extreme.Mathematics.BigInteger.Mod(Extreme.Mathematics.BigInteger.Multiply(Extreme.Mathematics.BigInteger.ModularPow(g, m, nsquare), Extreme.Mathematics.BigInteger.ModularPow(r, n, nsquare)), nsquare);

        }

        /**
     * Encrypts plaintext m. ciphertext c = g^m * r^n mod n^2. This function automatically generates random input r (to help with encryption).
     * @param m plaintext as a BigInteger
     * @return ciphertext as a BigInteger
     */


        public Extreme.Mathematics.BigInteger Encryption(Extreme.Mathematics.BigInteger m)
        {
            //string hexstring2 = "D";
            //Extreme.Mathematics.BigInteger r = Extreme.Mathematics.BigInteger.Parse(hexstring2, NumberStyles.AllowHexSpecifier);
            //Extreme.Mathematics.BigInteger r = new Extreme.Mathematics.BigInteger(4509402529411668222);

            Random r1 = new Random();
            // Int64 r = r1.Next(4509402529411668222, 9223372036854775807);

            var buffer = new byte[sizeof(Int64)];
            r1.NextBytes(buffer);
            Int64 r2 = BitConverter.ToInt64(buffer, 0);
            Extreme.Mathematics.BigInteger r = new Extreme.Mathematics.BigInteger(r2);

            return Extreme.Mathematics.BigInteger.Mod(Extreme.Mathematics.BigInteger.Multiply(Extreme.Mathematics.BigInteger.ModularPow(g, m, nsquare), Extreme.Mathematics.BigInteger.ModularPow(r, n, nsquare)), nsquare);

        }

        /**
         * Decrypts ciphertext c. plaintext m = L(c^lambda mod n^2) * u mod n, where u = (L(g^lambda mod n^2))^(-1) mod n.
         * @param c ciphertext as a BigInteger
         * @return plaintext as a BigInteger
         */

        public Extreme.Mathematics.BigInteger Decryption(Extreme.Mathematics.BigInteger c)
        {

            Extreme.Mathematics.BigInteger u;

            u = Extreme.Mathematics.BigInteger.ModularInverse(Extreme.Mathematics.BigInteger.Divide(Extreme.Mathematics.BigInteger.Subtract(Extreme.Mathematics.BigInteger.ModularPow(g, lambda, nsquare), Extreme.Mathematics.BigInteger.One), n), n);


            return Extreme.Mathematics.BigInteger.Mod(Extreme.Mathematics.BigInteger.Multiply(Extreme.Mathematics.BigInteger.Divide(Extreme.Mathematics.BigInteger.Subtract(Extreme.Mathematics.BigInteger.ModularPow(c, lambda, nsquare), Extreme.Mathematics.BigInteger.One), n), u), n);
        }
        /**
   * main function
   * @param str intput string
   */


        static void Main(string[] args)
        {
            /* instantiating an object of Evoting cryptosystem*/
            evoting paillier = new evoting();
            /* instantiating two plaintext msgs*/

            Extreme.Mathematics.BigInteger[] em = new Extreme.Mathematics.BigInteger[20];
            ; //array of bigintger

            Extreme.Mathematics.BigInteger m2;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("************************  CRYPTOGRAPHIC ELECTRONIC VOTING SYSTEM  ************************\n\n\t\t\tINSTRUCTIONS\n\n");
            Console.WriteLine("1. Choose No. of candidates and No. of Voters  for election");
            Console.WriteLine("2. Default value for Candidates is 2 and cannot be more than 8");
            Console.WriteLine("3. Enter candidate number for whom you want to vote");
            Console.WriteLine("4. Do not disclose your vote to anybody");
            Console.WriteLine("5. Contact the admininstrator for any query\n\n\n\n\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Select No. of Candidates standing for election");
            int nocand;
            string input = Console.ReadLine();
            nocand = Convert.ToInt32(input);
            Console.WriteLine();

            if (nocand < 2 || nocand > 8)
            {
                nocand = 2;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Incompatible value entered , default value has been chosen. ");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("Enter no.of voters:");
            int voters;
            string input1 = Console.ReadLine();
            voters = Convert.ToInt32(input1);
            Console.WriteLine("\n\n");

            string path = @"C:\Users\Yogi\Documents\Visual Studio 2012\Projects\ConsoleApplication1\ConsoleApplication1\bin\Debug\Encrypted Votes.txt";
            if (!File.Exists(path))
            {
                var myfile = File.Create("Encrypted Votes.txt");
                myfile.Close();
            }
            
                

                for (int i = 1; i <= voters; i++)
                {
                    Console.WriteLine("\n\n---------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("voter " + i + ":");
                    Console.WriteLine("Choose candidate (1-" + nocand + ") :  ");

                    // c=10^(a-1);

                    //BigInteger m1 = new BigInteger(a);

                    String a = Console.ReadLine();
                    Console.WriteLine();
                    int m1 = Convert.ToInt32(a);

                    if (m1 < 1 || m1 > nocand)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nWRONG VALUE ENTERED, KINDLY CHECK THE INPUT AGAIN\n\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        i -= 1;
                        continue;
                    }

                    m2 = new Extreme.Mathematics.BigInteger(10);

                    m1 = m1 - 1;

                  //  Thread.Sleep(1500);


                    Console.WriteLine(m1 + "  ");
                    m2 = Extreme.Mathematics.BigInteger.Pow(m2, 2 * m1);
                    Console.WriteLine(m2);
                    em[i] = paillier.Encryption(m2);

                    if(File.Exists(path))
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine();
                        sw.WriteLine(em[i]);
                        
                    }   

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(em[i]);
                    Console.ForegroundColor = ConsoleColor.White;
                }

            /*    if (File.Exists(path))
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine();
                        sw.WriteLine("****************************************************************************************");
                    }
            */
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------");

                /* test homomorphic properties -> D(E(m1)*E(m2) mod n^2) = (m1 + m2) mod n */
                Console.WriteLine("\n");
                Console.WriteLine("PROCEDURE FOR VOTING HAS FINISHED\nTHANKS FOR VOTING!!\n");
                //Thread.Sleep(500);
              /*  Console.WriteLine("ADDING ALL VOTES AND PREPARING RESULT........");
                Thread.Sleep(2500);
                Console.WriteLine("\n");
                //Console.WriteLine("");
                Console.WriteLine("\n");


                Extreme.Mathematics.BigInteger product_em1em2 = new Extreme.Mathematics.BigInteger(1);
                for (int j = 1; j <= voters; j++)
                {
                    product_em1em2 = Extreme.Mathematics.BigInteger.Multiply(product_em1em2, em[j]);

                }

                product_em1em2 = Extreme.Mathematics.BigInteger.Mod(product_em1em2, paillier.nsquare);
                //BigInteger sum_m1m2 = m1.add(m2).mod(paillier.n);

                Console.WriteLine("Encrypted Sum of the votes is: \n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(product_em1em2);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n");

                Console.WriteLine("decrypted sum: " + paillier.Decryption(product_em1em2).ToString());
                String x1 = paillier.Decryption(product_em1em2).ToString();
                Extreme.Mathematics.BigInteger x2, x3;
                x2 = paillier.Decryption(product_em1em2);
                int max, maxp, mm;
                int[] d = new int[x1.Length];
                max = maxp = mm = 0;


                //System.out.println(x1.length());  
                Console.WriteLine("\n\nRESULT\n");
                Extreme.Mathematics.BigInteger m4 = new Extreme.Mathematics.BigInteger(100);
                Extreme.Mathematics.BigInteger m5 = new Extreme.Mathematics.BigInteger(0);
                for (int k = 1; k <= x1.Length; k++)
                {
                    x3 = Extreme.Mathematics.BigInteger.Mod(x2, m4);
                    maxp = Convert.ToInt32(x3.ToString());
                    d[k - 1] = maxp;

                    if (max < maxp)
                    {
                        max = maxp;
                        mm = k;
                    }

                    //System.out.println(x3); 
                    //x3=x2%10;
                    x2 = Extreme.Mathematics.BigInteger.Divide(x2, m4);


                    //  System.out.println(x2); 

                    Console.WriteLine("No. of votes for candidate " + k + " :-  " + x3);

                    if (Extreme.Mathematics.BigInteger.Equals(x2, m5))
                        break;
                }

                Console.WriteLine();
                int c = 0;
                for (int k = 0; k < x1.Length; k++)
                {
                    if (d[k] == max)
                        c += 1;

                }
                if (c == 1)
                {
                    Console.WriteLine("Winner of election is candidate no: " + mm + " having a total no. of votes:  " + max);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There's a draw between candidates, hence no clear winner!");
                    Console.ForegroundColor = ConsoleColor.White;


                }*/
                Console.ReadLine();
            }
        }
    }




