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

        public string GetMetric(Project prj, string metric)
        {
            return prj.Measure.component.measures.FirstOrDefault(m => m.metric == metric)?.value;
        }
        public string GetMetricAvg(string metric)
        {
            try
            {
                var metrics = from p in Proyectos
                       select p.Measure.component.measures.FirstOrDefault(m => m.metric == metric) ;
                var vals = metrics.Select(m => decimal.Parse(m.value, System.Globalization.CultureInfo.InvariantCulture)).ToList();
                var avg = vals.Average(v => v);
                return avg.ToString("0.00");
            }
            catch { return "0"; }
        }
        public string GetMetricSum(string metric)
        {
            try {
                var prom = from p in Proyectos
                           select p.Measure.component.measures.FirstOrDefault(m => m.metric == metric);

                return prom.Sum(m => decimal.Parse(m.value)  ).ToString();
            }
            catch  { return "0"; }
        }
        public string ViewInSonarUrl(string componente)
        {
            return $"{Config.SonarBaseUrl}component_measures/?id={componente}";
        }
    }

}
