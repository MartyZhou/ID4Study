using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace DemoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CallTestApi().GetAwaiter().GetResult();

            Console.Read();
        }

        private static async Task CallTestApi()
        {
            var disco = await DiscoveryClient.GetAsync("https://localhost:4999");

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            var tokenClientRo = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponseRo = await tokenClientRo.RequestResourceOwnerPasswordAsync("bob", "password", "api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            if (tokenResponseRo.IsError)
            {
                Console.WriteLine(tokenResponseRo.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine(tokenResponseRo.Json);

            var httpClient = new HttpClient();
            httpClient.SetBearerToken(tokenResponseRo.AccessToken);

            var response = await httpClient.GetAsync("https://localhost:5001/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var stringContent = JArray.Parse(content);
                Console.WriteLine(stringContent);
            }
        }
    }
}
