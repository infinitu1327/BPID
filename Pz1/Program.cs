using System;
using static VernamEncryption.VernamEncryption;

namespace Pz1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var encryptedText = Encrypt("лес", "сон");
            Console.WriteLine(encryptedText);

            var decryptedText = Decrypt(encryptedText, "сон");
            Console.WriteLine(decryptedText);
        }
    }
}