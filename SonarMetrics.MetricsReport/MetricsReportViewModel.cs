using SonarMetrics.Lib;
using SonarMetrics.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonarMetrics.MetricsReport
{
   public class MetricsReportViewModel
    {
        public List<Project> Proyectos { get; set; }
        public DownloadConfig Config { get; set; }

        public string GetMetricSQALE(Project prj)
        {
            return GetLetterSQALE(prj.Measure.component.measures.FirstOrDefault(m => m.metric == "sqale_rating")?.value);
        }
        private string GetLetterSQALE(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            int val = int.Parse(value.Substring(0, 1)) - 1;
            return "ABCDE"[val].ToString();
        }

        public string MetricCoverage(Project prj)
        {
            return prj.Measure.component.measures.FirstOrDefault(m => m.metric == "coverage")?.value;
        }

    }

}
