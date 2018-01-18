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
        public string IssuesUrl { get; set; } //"api/issues/search?componentKeys={0}&statuses=OPEN&createdInLast={1}&severities=BLOCKER,CRITICAL,MAJOR,MINOR"
        public string SourcesUrl { get; set; } //"api/sources/show?key={0}"
        public string OutputFile { get; set; }
    }
}
