using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonarMetrics.Issues
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }
        static async Task MainAsync(string[] args)
        {
            if (args.GetUpperBound(0) < 2)
            {
                System.Console.WriteLine("Parametros:");
                for (int i = 0; i <= args.GetUpperBound(0); i++)
                {
                    System.Console.WriteLine($"{i}-{args[i]}");
                }
                System.Console.WriteLine("Parametros Necesarios: Usuario Password ProjectFilter(Separados por @)");
            }
            else
            {
                var conf = new DownloadConfig
                {
                    Usuario = args[0],
                    Password = args[1],
                    SonarBaseUrl = Properties.Settings.Default.SonarBaseUrl,
                    ProjectsUrl = Properties.Settings.Default.ProjectsUrl,
                    MetricsUrl = Properties.Settings.Default.MetricsUrl,
                    ProjectFilter = args[2]
                };
                var Downloadhelper = new DownloadHelper(conf);

                try
                {
                    await Downloadhelper.DownloadProjects();
                    WriteResultHtml(Downloadhelper.Proyectos);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    System.Console.Write(e.StackTrace);
                }
                //System.Console.ReadKey();
            }
        }

        static private void WriteResult(List<Project> proyectos)
        {
            var maxKey = MaxProjectKeyLength(proyectos.Select(p => p.Key).ToList()) + 1;
            System.Console.WriteLine($"{"KEY".PadRight(maxKey)}\tSQALE\tCOVERAGE");
            foreach (var prj in proyectos)
            {
                var metricSQALE = GetLetterSQALE(prj.Measure.component.measures.FirstOrDefault(m => m.metric == "sqale_rating")?.value);
                var metricCoverage = prj.Measure.component.measures.FirstOrDefault(m => m.metric == "coverage")?.value;
                System.Console.WriteLine($"{prj.Key.PadRight(maxKey)}\t{metricSQALE}\t{metricCoverage}");
            }

        }
        static private void WriteResultHtml(List<Project> proyectos)
        {
            System.Console.WriteLine("<table><tr><th>KEY</th><th>SQALE</th><th>COVERAGE</th></tr>");
            foreach (var prj in proyectos)
            {
                var metricSQALE = GetLetterSQALE(prj.Measure.component.measures.FirstOrDefault(m => m.metric == "sqale_rating")?.value);
                var metricCoverage = prj.Measure.component.measures.FirstOrDefault(m => m.metric == "coverage")?.value;
                System.Console.WriteLine($"<tr><td>{prj.Key}</td><td>{metricSQALE}</td><td>{metricCoverage}</td></tr>");
            }
            System.Console.WriteLine("</table>");
        }

        static private string GetLetterSQALE(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            int val = int.Parse(value.Substring(0, 1)) - 1;
            return "ABCDE"[val].ToString();
        }

        static int MaxProjectKeyLength(List<String> Keys)
        {
            return Keys.Max(p => p.Length);
        }

    }
}
