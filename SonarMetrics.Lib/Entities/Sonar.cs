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
    }

    public class Measures
    {
        public Component component { get; set; }
    }
}
