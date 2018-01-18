using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonarMetrics.Lib.Entities
{
    
    public class IssuesResponse
    {
        public int total { get; set; }
        public int p { get; set; }
        public int ps { get; set; }
        public Paging paging { get; set; }
        public List<Issue> issues { get; set; }
        public List<Component> components { get; set; }
    }

    public class SourcesResponse
    {
        public List<List<object>> sources { get; set; }
    }
}
