using System;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client
{
    internal static class Program
    {
        private static async Task Main()
        {
            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var client = new HttpClient(handler))
                {
                    var keysResponse = await client.GetAsync("http://localhost:5000/api/RSA");

                    if (!keysResponse.IsSuccessStatusCode) return;

                    var publicKeys =
                        JsonConvert.DeserializeObject<(BigInteger e, BigInteger n)>(
                            await keysResponse.Content.ReadAsStringAsync());

                    Console.WriteLine("Public keys:");
                    Console.WriteLine($"e:{publicKeys.e}");
                    Console.WriteLine($"n:{publicKeys.n}");
                    Console.WriteLine("Enter text for encryption:");

                    var text = Console.ReadLine();
                    var encryptedText = RSA.RSA.Encrypt(text, publicKeys);

                    Console.WriteLine("Encrypted text (base 64):");
                    Console.WriteLine(encryptedText);

                    var form = new MultipartFormDataContent {{new StringContent(encryptedText), "encryptedText"}};

                    var decryptedTextResponse = await client.PostAsync("http://localhost:5000/api/RSA", form);

                    if (!decryptedTextResponse.IsSuccessStatusCode) return;

                    var decryptedText = await decryptedTextResponse.Content.ReadAsStringAsync();

                    Console.WriteLine("Decrypted text");
                    Console.WriteLine(decryptedText);
                }
            }
        }
    }
}