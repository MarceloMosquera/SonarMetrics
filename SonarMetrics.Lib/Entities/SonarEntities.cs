using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonarMetrics.Lib.Entities
{
    public class Project
    {
        public int id { get; set; }
        [JsonProperty(PropertyName = "k")]
        public string Key { get; set; }
        public string nm { get; set; }
        public string sc { get; set; }
        public string qu { get; set; }

        public virtual Measures Measure { get; set; }

        public virtual List<Issue> Issues { get; set; }
        public virtual List<Component> Components { get; set; }
    }


    public class Period
    {
        public int index { get; set; }
        public string value { get; set; }
    }

    public class Measure
    {
        public string metric { get; set; }
        public string value { get; set; }
        public List<Period> periods { get; set; }
    }

    public class Component
    {
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string qualifier { get; set; }
        public List<Measure> measures { get; set; }
        public virtual List<Component> Components { get; set; }
        public virtual Source source { get; set; }

    }

    public class Measures
    {
        public Component component { get; set; }
    }

    public class Source {
        public List<SourceLine> lines { get; set; } 
    }
    public class SourceLine
    {
        public int lineNum { get; set; }
        public string lineText { get; set; }

    }

    public class Paging
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
    }

    public class TextRange
    {
        public int startLine { get; set; }
        public int endLine { get; set; }
        public int startOffset { get; set; }
        public int endOffset { get; set; }
    }

    public class Issue
    {
        public string key { get; set; }
        public string rule { get; set; }
        public string severity { get; set; }
        public string component { get; set; }
        public int componentId { get; set; }
        public string project { get; set; }
        public int line { get; set; }
        public TextRange textRange { get; set; }
        public List<object> flows { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string effort { get; set; }
        public string debt { get; set; }
        public string author { get; set; }
        public List<string> tags { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime updateDate { get; set; }
        public string type { get; set; }
    }
}
