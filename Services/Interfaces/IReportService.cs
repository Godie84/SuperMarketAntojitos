using SuperMarketAntojitos.Models.ViewModels;

namespace SuperMarketAntojitos.Services.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GenerateExcelAsync(ReportType reportType, DateTime? referenceDate = null);
        Task<byte[]> GeneratePdfAsync(ReportType reportType, DateTime? referenceDate = null);
    }
}
