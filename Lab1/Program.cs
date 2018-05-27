using System;
using System.Text;

namespace Lab1
{
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Enter key (8 symbols):");
            var key = Console.ReadLine();

            Console.WriteLine("Enter text for encrypting:");
            var text = Console.ReadLine();

            var encrypted = DES.DES.Encrypt(text, key);
            Console.WriteLine("Encrypted text (base 64):");
            Console.WriteLine(encrypted);

            var decrypted = DES.DES.Decrypt(encrypted, key);
            Console.WriteLine("Decrypted text:");
            Console.WriteLine(decrypted);
        }
    }
}