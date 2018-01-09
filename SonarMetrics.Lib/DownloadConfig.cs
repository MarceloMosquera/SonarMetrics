using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonarMetrics.Lib
{
    public class DownloadConfig
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string SonarBaseUrl { get; set; } //"http://g100603sv47b:9000/"
        public string ProjectsUrl { get; set; } //"api/projects/index"
        public string MetricsUrl { get; set; } //"api/measures/component?componentKey={0}&metricKeys=sqale_rating,coverage"
        public string ProjectFilter { get; set; } //"SuperNet-@VUC@TSAV"
    }
}
