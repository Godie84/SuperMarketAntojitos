using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarketAntojitos.Models.ViewModels;
using SuperMarketAntojitos.Services.Interfaces;

namespace SuperMarketAntojitos.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index() => View(new ReportViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(ReportViewModel model)
        {
            var fileName = $"SalesReport_{model.ReportType}_{DateTime.Now:yyyyMMdd}";

            if (model.Format == ReportFormat.Excel)
            {
                var bytes = await _reportService
                    .GenerateExcelAsync(model.ReportType, model.ReferenceDate);

                return File(bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{fileName}.xlsx");
            }
            else
            {
                var bytes = await _reportService
                    .GeneratePdfAsync(model.ReportType, model.ReferenceDate);

                return File(bytes, "application/pdf", $"{fileName}.pdf");
            }
        }
    }
}
