using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Program
    {
        private static BigInteger Pow(BigInteger degree, BigInteger basis, BigInteger mod)
        {
            BigInteger p = 1, remainder;
            BigInteger m = basis;
            while (degree != 0 && m != 1)
            {
                remainder = degree % 2;
                m = m * m % mod;
                if (remainder == 1)
                {
                    p = m * p % mod;
                }
                degree /= 2;
            }
            return p;
        }

        private static BigInteger GCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
            BigInteger x1, y1;
            BigInteger d = GCD(b % a, a, out x1, out y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }

        static void Main(string[] args)
        {
            BigInteger p = BigInteger.genPseudoPrime(128, 10, new Random());
            while(!p.isProbablePrime(10))
            {
                p = BigInteger.genPseudoPrime(128, 10, new Random());
            }
            BigInteger q = BigInteger.genPseudoPrime(128, 10, new Random());
            while(!q.isProbablePrime(10))
            {
                q = BigInteger.genPseudoPrime(128, 10, new Random());
            }
            BigInteger n = q * p;
            BigInteger fi = (q - 1) * (p - 1);
            BigInteger e = BigInteger.genPseudoPrime(64, 10, new Random()),x,y;
            while (!e.isProbablePrime(10) && GCD(e, fi, out x,out  y) != 1)
            {
                e = BigInteger.genPseudoPrime(64, 10, new Random());
            }

            byte lengthBlock = 8;
            int i=0;

            Console.Write("Enter text: ");
            string text = Console.ReadLine();
            List<string> cipher = new List<string>();
            int lenghtText = text.Length;
            while (lenghtText % 8 != 0)
            {
                text += " ";
                lenghtText++;
            }

            int lenghtAlphabet = 256;
            BigInteger degree;
            BigInteger pt,ct;
            string block;

            while(i<lenghtText)
            {
                degree = 1;
                pt = 0;
                block = "";
                if (i % lengthBlock == 0 && i != 0)
                {
                    for (int j = i-1; j >= i-lengthBlock; j--)
                    {
                        pt += (int)text[j] * degree;
                        degree *= lenghtAlphabet;
                    }
                    ct = Pow(e, pt, n);
                    while(ct>0)
                    {
                        block = Convert.ToChar(Convert.ToInt32((ct % lenghtAlphabet).ToString())) + block;  
                        ct /= lenghtAlphabet;
                    }
                    cipher.Add(block);
                }
                i++;
            }

            foreach (string bl in cipher)
            {
                Console.WriteLine(bl);
            }

            BigInteger d;
            GCD(e, fi,out d,out y);

            i = 0;
            int lenCipherBlock;
            text = "";

            foreach(string bl in cipher)
            {
                degree = 1;
                lenCipherBlock = bl.Length;
                ct = 0;
                for (i=lenCipherBlock-1;i>=0;i--)
                {
                    ct += (int)bl[i] * degree;
                    degree *= lenghtAlphabet; 
                }
                pt = Pow(d, ct, n);
                while(pt >0 )
                {
                    text = Convert.ToChar(Convert.ToInt32((pt % lenghtAlphabet).ToString())) + text;
                    pt /= lenghtAlphabet;
                }
            }

            Console.WriteLine(text);
          
        }
    }
}
