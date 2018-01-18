using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SonarMetrics.Lib
{
    public class CustomHttpClient<T> : HttpClient
    {
        public DownloadConfig Config { get; set; }

        public CustomHttpClient(DownloadConfig config) : base()
        {
            Config = config;
            DefaultRequestHeaders.Add("Accept", "application/json");
            DefaultRequestHeaders.Add("Authorization", GetBasicAuth());
        }
        
        private string GetBasicAuth()
        {
            string encodedToken = Base64Encode(Config.Usuario + ":" + Config.Password);
            return "Basic " + encodedToken;
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public new async Task<T> GetAsync(string url) 
        {
            //Console.WriteLine(url);
            var x = await base.GetAsync(url);
            return await HandleResponseAsync(x);
        }

        private async Task<T> HandleResponseAsync (HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                Console.WriteLine($"{response.RequestMessage.RequestUri.PathAndQuery}: {response.StatusCode}");
                throw new Exception(response.StatusCode.ToString());
            }
        }

    }
}
