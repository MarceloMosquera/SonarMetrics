using RazorEngine;
using RazorEngine.Templating;
using SonarMetrics.Lib;
using SonarMetrics.Lib.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SonarMetrics.IssuesReport
{
    public class IssuesReport : IReport
    {
        public List<Project> Proyectos { get; set; }
        public DownloadConfig Config { get; set; }
        public IssuesReport(DownloadConfig config, string[] args)
        {
            Config = config;
            Config.OutputFile = (args.GetUpperBound(0) > 3) ? args[4] : "IssuesReport-Generated.html";

        }
        public async Task Download()
        {
            var downloadIssues = new IssuesReportDownloader(Config);
            Proyectos =await downloadIssues.DownloadProjects();
        }
        public void WriteResultHtml()
        {
            string template = System.IO.File.ReadAllText("IssuesReport.chtml");
            var vm =  new IssuesReportViewModel();
            vm.Proyectos = Proyectos.Where(p => p.Issues.Count() > 1).OrderBy(p => p.Issues.Count()).ToList();
            vm.Config = Config;

            var result = Engine.Razor.RunCompile(template, "templateKey", typeof(IssuesReportViewModel), vm);
            System.IO.File.WriteAllText(Config.OutputFile, result);
        }
        
    }
}
