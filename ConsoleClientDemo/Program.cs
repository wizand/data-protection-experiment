using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ConsoleClientDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string toEncrypt = "tekstikontenttia";
            string encrypted = GetEncryptedString(toEncrypt);
            string decrypted = GetDecryptedString(encrypted);
            Console.WriteLine($"{toEncrypt} as encrypted: {encrypted}\n");
            Console.WriteLine($"{encrypted} as decrypted: {decrypted}\n");


            TokenRequest tokenRequest = new TokenRequest()
            {
                Username = "admin",
                Password = "admin",
                ClientId = "consoleclient"
            };

            string encryptedTokenRequest = GetEncryptedTokenRequestString(tokenRequest);
            Console.WriteLine($"TokenReqeust as encrypted: {encryptedTokenRequest}\n");
            string decryptedTokenRequest = GetDecryptedString(encryptedTokenRequest);
            Console.WriteLine($"{encryptedTokenRequest} as decrypted {decryptedTokenRequest}\n");
        }

        private static string GetEncryptedTokenRequestString(TokenRequest tokenRequest)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7171");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/createRequestTokenString");

            //Add allow */* to accept header
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(tokenRequest), Encoding.UTF8, "application/json");

            request.Content = httpContent;

            var result = client.Send(request);

            return result.Content.ReadAsStringAsync().Result;
        }

        private static string GetEncryptedString(string stringToEncrypt)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7171");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/encryptString");

            //Add allow */* to accept header
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            HttpContent httpContent = new StringContent($"\"{stringToEncrypt}\"", Encoding.UTF8, "application/json");

            request.Content = httpContent;

            var result = client.Send(request);

            return result.Content.ReadAsStringAsync().Result;
        }
    

        private static string GetDecryptedString(string stringTodecrypt)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7171");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/decryptString");

            //Add allow */* to accept header
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            HttpContent httpContent = new StringContent($"\"{stringTodecrypt}\"", Encoding.UTF8, "application/json");

            request.Content = httpContent;

            var result = client.Send(request);

            return result.Content.ReadAsStringAsync().Result;
        }
    }


record TokenRequest()
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
}
}
