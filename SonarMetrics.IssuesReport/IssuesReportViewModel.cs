using SonarMetrics.Lib;
using SonarMetrics.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonarMetrics.IssuesReport
{
   public class IssuesReportViewModel
    {
        public List<Project> Proyectos { get; set; }
        public DownloadConfig Config { get; set; }

        public string ViewInSonarUrl(string componente, int line)
        {
            return $"{Config.SonarBaseUrl}code/index?id={componente}";
        }

        public bool HasSource (Issue issue)
        {
            var prj = Proyectos.First(p => p.Key == issue.project);
            var comp = prj.Components.First(c => c.key == issue.component);
            return comp.source != null;
        }

        public List<SourceLine> SourceLines(Issue issue)
        {
            var comp = Proyectos.First(p => p.Key == issue.project).Components.First(c => c.key == issue.component);
            var startIndex = issue.line - 1;
            var endIndex = Math.Min (issue.line + 3, comp.source.lines.Count()) ;
            return comp.source.lines.Skip(startIndex).Take(endIndex - startIndex).ToList();
        }

    }

}
