using System.Threading.Tasks;

namespace SonarMetrics.Lib
{
    public interface IReport
    {
        Task Download();
        void WriteResultHtml();
    }
}