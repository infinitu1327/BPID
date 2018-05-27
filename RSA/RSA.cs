using System;
using System.Numerics;
using System.Text;

namespace RSA
{
    public class RSA
    {
        private static readonly Random Rnd = new Random();
        private readonly (BigInteger d, BigInteger n) _privateKeys;
        public readonly (BigInteger e, BigInteger n) PublicKeys;

        public RSA()
        {
            var p = GetRandomPrime();
            var q = GetRandomPrime();
            var n = BigInteger.Multiply(p, q);
            var f = BigInteger.Multiply(p - 1, q - 1);
            var e = Rnd.Next(2, 65535);

            while (BigInteger.GreatestCommonDivisor(f, e) != 1) e++;

            BigInteger x = 1;
            BigInteger d;

            while (true)
            {
                d = (x * f + 1) / e;

                if (d * e == x * f + 1) break;

                x++;
            }

            PublicKeys = (e, n);
            _privateKeys = (d, n);
        }

        public static string Encrypt(string text, (BigInteger e, BigInteger n) publicKeys)
        {
            var number = new BigInteger(Encoding.UTF8.GetBytes(text));

            BigInteger.ModPow(number, publicKeys.e, publicKeys.n);

            return Convert.ToBase64String(number.ToByteArray());
        }

        public string Encrypt(string text)
        {
            var number = new BigInteger(Encoding.UTF8.GetBytes(text));

            BigInteger.ModPow(number, PublicKeys.e, PublicKeys.n);

            return Convert.ToBase64String(number.ToByteArray());
        }

        public string Decrypt(string encryptedText)
        {
            var number = new BigInteger(Convert.FromBase64String(encryptedText));

            BigInteger.ModPow(number, _privateKeys.d, _privateKeys.n);

            return Encoding.UTF8.GetString(number.ToByteArray());
        }

        private static int GetRandomPrime()
        {
            var number = Rnd.Next();

            while (!IsPrime(number)) number++;

            return number;
        }

        private static bool IsPrime(int number)
        {
            for (var i = 2; i < Math.Sqrt(number); i++)
                if (number % i == 0)
                    return false;

            return true;
        }
    }
}