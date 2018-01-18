using RazorEngine;
using RazorEngine.Templating;
using SonarMetrics.Lib;
using SonarMetrics.Lib.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SonarMetrics.MetricsReport
{
    public class MetricsReport : IReport
    {
        public List<Project> Proyectos { get; set; }
        public DownloadConfig Config { get; set; }
        public MetricsReport(DownloadConfig config, string[] args)
        {
            Config = config;
            Config.OutputFile = (args.GetUpperBound(0) > 3) ? args[4] : "MetricsReport-Generated.html";

        }
        public async Task Download()
        {
            var downloadIssues = new MetricsReportDownloader(Config);
            Proyectos =await downloadIssues.DownloadProjects();
        }
        public void WriteResultHtml()
        {
            string template = System.IO.File.ReadAllText("MetricsReport.chtml");
            var vm =  new MetricsReportViewModel();
            vm.Proyectos = Proyectos.ToList();
            vm.Config = Config;

            var result = Engine.Razor.RunCompile(template, "templateKey", typeof(MetricsReportViewModel), vm);
            System.IO.File.WriteAllText(Config.OutputFile, result);
        }
        
    }
}
