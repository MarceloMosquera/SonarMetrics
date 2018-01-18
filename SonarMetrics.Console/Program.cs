using SonarMetrics.IssuesReport;
using SonarMetrics.Lib;
using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading.Tasks;

namespace SonarMetrics.Console
{
    class Program
    {

        enum TipoReporte
        {
            Metrics,
            Issues
        }

        static void Main(string[] args)
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                // RazorEngine cannot clean up from the default appdomain...
                AppDomainSetup adSetup = new AppDomainSetup();
                adSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var current = AppDomain.CurrentDomain;
                // You only need to add strongnames when your appdomain is not a full trust environment.
                var strongNames = new StrongName[0];

                var domain = AppDomain.CreateDomain(
                    "MyMainDomain", null,
                    current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
                    strongNames);
                var exitCode = domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location, args);
                // RazorEngine will cleanup. 
                AppDomain.Unload(domain);
                //return exitCode;
            }
            else
            {
                MainAsync(args).GetAwaiter().GetResult();
            }

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
                System.Console.WriteLine("Parametros Necesarios: Usuario Password ProjectFilter(Separados por @) [Reporte=Metrics|Issues] outFile");
            }
            else
            {
                var reporteAEmitir = (args.GetUpperBound(0) > 2) ? (TipoReporte)Enum.Parse(typeof(TipoReporte), args[3]) : TipoReporte.Metrics;

                var conf = new DownloadConfig
                {
                    Usuario = args[0],
                    Password = args[1],
                    SonarBaseUrl = Properties.Settings.Default.SonarBaseUrl,
                    ProjectsUrl = Properties.Settings.Default.ProjectsUrl,
                    MetricsUrl = Properties.Settings.Default.MetricsUrl,
                    IssuesUrl = Properties.Settings.Default.IssuesUrl,
                    SourcesUrl = Properties.Settings.Default.SourcesUrl,
                    ProjectFilter = args[2]
                };

                try
                {
                    switch (reporteAEmitir)
                    {
                        case TipoReporte.Issues:
                            var report = new IssuesReport.IssuesReport(conf, args);
                            await report.Download();
                            report.WriteResultHtml();
                            break;
                        default:
                            //var downloadHelper = new DownloadHelper(conf);
                            //await downloadHelper.DownloadProjects();
                            //(new MetricsReport(downloadHelper.Proyectos)).WriteResultHtml();
                            var report2 = new MetricsReport.MetricsReport(conf, args);
                            await report2.Download();
                            report2.WriteResultHtml();
                            break;
                    }


                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    //System.Console.Write(e.StackTrace);
                    System.Console.ReadKey();
                }
                //System.Console.ReadKey();
            }
        }

    }
}
