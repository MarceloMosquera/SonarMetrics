using Newtonsoft.Json;
using SonarMetrics.Lib;
using SonarMetrics.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SonarMetrics.MetricsReport
{
    public class MetricsReportDownloader
    {

        public DownloadConfig Config { get; set; }
        public MetricsReportDownloader(DownloadConfig config)
        {
            Config = config;
        }

        public async Task<List<Project>> DownloadProjects()
        {
            string url = $"{Config.SonarBaseUrl}{Config.ProjectsUrl}";
            var client = new CustomHttpClient<List<Project>>(Config);
            var projectsResponse = await client.GetAsync(url);
            
            string[] filters = Config.ProjectFilter.Split('@');

            var proyectos = projectsResponse
                .Where(p => filters.Any(f => p.Key.StartsWith(f)))
                .ToList();

            await Task.WhenAll(
                from prj in proyectos
                select DownloadMetrics(prj));

            return proyectos;
        }


        public async Task DownloadMetrics(Project proyecto)
        {
            string url = String.Format($"{Config.SonarBaseUrl}{Config.MetricsUrl}", proyecto.Key);

            var client = new CustomHttpClient<Measures>(Config);
            proyecto.Measure = await client.GetAsync(url);
        }

    }
}
