using Newtonsoft.Json;
using SonarMetrics.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SonarMetrics.Lib
{
    public class DownloadHelper
    {
        public List<Project> Proyectos { get; set; }

        public DownloadConfig Config { get; set; }
        public DownloadHelper(DownloadConfig config)
        {
            Config = config;
        }

        public async Task DownloadProjects()
        {
            string url = $"{Config.SonarBaseUrl}{Config.ProjectsUrl}";

            HttpClient client = GetHttpClient();
            var resp = await client.GetAsync(url);

            string[] filters = Config.ProjectFilter.Split('@');
            if (resp.IsSuccessStatusCode)
            {
                string json = await resp.Content.ReadAsStringAsync();
                var proyectos = JsonConvert.DeserializeObject<List<Project>>(json)
                    .Where( p => filters.Any( f => p.Key.StartsWith(f)))
                    .ToList();

                await Task.WhenAll(
                    from prj in proyectos
                    select DownloadMetrics(prj));

                Proyectos = proyectos;
            }
            else
            {
                throw new Exception(resp.StatusCode.ToString());
            }
        }

        public async Task DownloadMetrics(Project proyecto)
        {
            string url = String.Format($"{Config.SonarBaseUrl}{Config.MetricsUrl}", proyecto.Key);

            HttpClient client = GetHttpClient();
            var resp = await client.GetAsync(url);
            if (resp.IsSuccessStatusCode)
            {
                string json = await resp.Content.ReadAsStringAsync();
                proyecto.Measure = JsonConvert.DeserializeObject<Measures>(json);
            }
            else
            {
                throw new Exception(resp.StatusCode.ToString());
            }
        }

        private HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", GetBasicAuth());
            return client;
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
    }
}
