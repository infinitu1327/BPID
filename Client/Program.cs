using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client
{
    internal class Program
    {
        private static async Task Main()
        {
            var rnd = new Random();
            var id = rnd.Next().ToString();

            using (var client = new HttpClient())
            {
                var getResponse = await client.GetAsync($"http://localhost:5000/api/RSA/{id}");
                if (!getResponse.IsSuccessStatusCode) return;
                var publicKeys =
                    JsonConvert.DeserializeObject<(BigInteger e, BigInteger n)>(
                        await getResponse.Content.ReadAsStringAsync());

                Console.WriteLine("Public keys:");
                Console.WriteLine($"e:{publicKeys.e}");
                Console.WriteLine($"n:{publicKeys.n}");
                Console.WriteLine("Enter text for encryption:");

                var text = Console.ReadLine();
                var encryptedText = RSA.RSA.Encrypt(text, publicKeys);

                Console.WriteLine("Encrypted text (base 64):");
                Console.WriteLine(encryptedText);

                var form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("encryptedText", encryptedText)
                });

                var postResponse = await client.PostAsync($"http://localhost:5000/api/RSA/{id}", form);
                var decryptedText = await postResponse.Content.ReadAsStringAsync();

                Console.WriteLine("Decrypted text");
                Console.WriteLine(decryptedText);
            }
        }
    }
}