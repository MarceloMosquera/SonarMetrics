using Newtonsoft.Json;
using SonarMetrics.Lib;
using SonarMetrics.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SonarMetrics.IssuesReport
{
    public class IssuesReportDownloader
    {

        public DownloadConfig Config { get; set; }
        public IssuesReportDownloader(DownloadConfig config)
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
                select DownloadNewIssues(prj));

            return proyectos;
        }

        public async Task DownloadNewIssues(Project proyecto)
        {
            string url = String.Format($"{Config.SonarBaseUrl}{Config.IssuesUrl}", proyecto.Key);
            var client = new CustomHttpClient<IssuesResponse>(Config);
            var issuesResponse = await client.GetAsync(url);

            proyecto.Issues = issuesResponse.issues;
            proyecto.Components = issuesResponse.components;

            await Task.WhenAll(
              from comp in proyecto.Components
              where comp.qualifier == "FIL"
              select DownloadSource(comp));

        }

        public async Task DownloadSource(Component componente)
        {
            string url = String.Format($"{Config.SonarBaseUrl}{Config.SourcesUrl}", componente.key);
            //var client = new CustomHttpClient<List<KeyValuePair<int, string>>>(Config);
            var client = new CustomHttpClient<SourcesResponse>(Config);
            var sourceResponse = await client.GetAsync(url);

            componente.source = new Source();
            componente.source.lines = new List<SourceLine>();
            //{ 
            //    lines = sourceResponse.sources.Select( 
            //        s=> new SourceLine() { lineNum = (int)s[0], lineText = ((string)s[1])} ).ToList() 
            //};
            int lineNum = 1;
            foreach (var item in sourceResponse.sources)
            {
                var text = (string)item[1];
                componente.source.lines.Add(new SourceLine() { lineNum = lineNum, lineText = text });
                lineNum++;
            }
        }

    }
}
