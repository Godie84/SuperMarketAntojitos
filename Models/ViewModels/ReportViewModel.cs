namespace SuperMarketAntojitos.Models.ViewModels
{
    public class ReportViewModel
    {
        public ReportType ReportType { get; set; }
        public ReportFormat Format { get; set; }
        public DateTime? ReferenceDate { get; set; } = DateTime.Today;
    }

    public enum ReportType
    {
        Daily,
        Weekly,
        Monthly,
        Annual
    }

    public enum ReportFormat
    {
        Excel,
        PDF
    }
}
