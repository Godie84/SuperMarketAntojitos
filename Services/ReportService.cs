using SuperMarketAntojitos.Data;
using SuperMarketAntojitos.Models.Entities;
using SuperMarketAntojitos.Models.ViewModels;
using SuperMarketAntojitos.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;

namespace SuperMarketAntojitos.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ─── Helpers ─────────────────────────────────────────────────────────────

        private (DateTime Start, DateTime End) GetDateRange(ReportType reportType, DateTime reference)
        {
            return reportType switch
            {
                ReportType.Daily => (reference.Date, reference.Date.AddDays(1)),
                ReportType.Weekly => (reference.AddDays(-(int)reference.DayOfWeek).Date,
                                       reference.AddDays(7 - (int)reference.DayOfWeek).Date),
                ReportType.Monthly => (new DateTime(reference.Year, reference.Month, 1),
                                       new DateTime(reference.Year, reference.Month, 1).AddMonths(1)),
                ReportType.Annual => (new DateTime(reference.Year, 1, 1),
                                       new DateTime(reference.Year + 1, 1, 1)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private async Task<List<Sale>> GetSalesAsync(ReportType reportType, DateTime? referenceDate)
        {
            var reference = referenceDate ?? DateTime.Today;
            var (start, end) = GetDateRange(reportType, reference);

            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Cashier)
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                .Where(s => s.SaleDate >= start && s.SaleDate < end)
                .OrderBy(s => s.SaleDate)
                .ToListAsync();
        }

        // ─── Excel ────────────────────────────────────────────────────────────────

        public async Task<byte[]> GenerateExcelAsync(ReportType reportType, DateTime? referenceDate = null)
        {
            var sales = await GetSalesAsync(reportType, referenceDate);

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Sales Report");

            // Title
            ws.Cell(1, 1).Value = $"Sales Report - {reportType} | {DateTime.Now:dd/MM/yyyy HH:mm}";
            ws.Range(1, 1, 1, 9).Merge().Style
                .Font.SetBold(true)
                .Font.SetFontSize(14)
                .Fill.SetBackgroundColor(XLColor.DarkBlue)
                .Font.SetFontColor(XLColor.White)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Headers
            string[] headers = { "Sale ID", "Date", "Customer", "ID Number",
                              "Product", "Qty", "Unit Price", "Subtotal", "Sale Total" };
            for (int col = 0; col < headers.Length; col++)
            {
                var cell = ws.Cell(2, col + 1);
                cell.Value = headers[col];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.SteelBlue;
                cell.Style.Font.FontColor = XLColor.White;
            }

            // Data rows
            int row = 3;
            foreach (var sale in sales)
            {
                foreach (var detail in sale.SaleDetails)
                {
                    ws.Cell(row, 1).Value = sale.Id;
                    ws.Cell(row, 2).Value = sale.SaleDate.ToString("dd/MM/yyyy HH:mm");
                    ws.Cell(row, 3).Value = sale.Customer?.FullName ?? "";
                    ws.Cell(row, 4).Value = sale.Customer?.IdentificationNumber ?? "";
                    ws.Cell(row, 5).Value = detail.Product.Name;
                    ws.Cell(row, 6).Value = detail.Quantity;
                    ws.Cell(row, 7).Value = detail.UnitPrice;
                    ws.Cell(row, 7).Style.NumberFormat.Format = "$#,##0.00";
                    ws.Cell(row, 8).Value = detail.Subtotal;
                    ws.Cell(row, 8).Style.NumberFormat.Format = "$#,##0.00";
                    ws.Cell(row, 9).Value = sale.TotalAmount;
                    ws.Cell(row, 9).Style.NumberFormat.Format = "$#,##0.00";

                    // Alternate row color
                    if (row % 2 == 0)
                        ws.Row(row).Style.Fill.BackgroundColor = XLColor.LightCyan;

                    row++;
                }
            }

            // Grand total row
            ws.Cell(row, 7).Value = "Grand Total:";
            ws.Cell(row, 7).Style.Font.Bold = true;
            ws.Cell(row, 8).Value = sales.Sum(s => s.TotalAmount);
            ws.Cell(row, 8).Style.Font.Bold = true;
            ws.Cell(row, 8).Style.NumberFormat.Format = "$#,##0.00";

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        // ─── PDF ──────────────────────────────────────────────────────────────────

        public async Task<byte[]> GeneratePdfAsync(ReportType reportType, DateTime? referenceDate = null)
        {
            var sales = await GetSalesAsync(reportType, referenceDate);

            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // Title
            document.Add(new Paragraph($"Antojitos Supermarket - Sales Report")
                .SetFontSize(18).SetBold());
            document.Add(new Paragraph($"Period: {reportType} | Generated: {DateTime.Now:dd/MM/yyyy HH:mm}")
                .SetFontSize(10));
            document.Add(new Paragraph(" "));

            // Table
            float[] columnWidths = { 1f, 2f, 3f, 2f, 2.5f, 1f, 1.5f, 1.5f };
            var table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

            string[] headers = { "Sale ID", "Date", "Customer", "ID Number",
                              "Product", "Qty", "Unit Price", "Subtotal" };

            foreach (var header in headers)
            {
                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph(header).SetBold())
                    .SetBackgroundColor(new DeviceRgb(30, 80, 140))
                    .SetFontColor(ColorConstants.WHITE));
            }

            bool alternate = false;
            foreach (var sale in sales)
            {
                foreach (var detail in sale.SaleDetails)
                {
                    var bgColor = alternate
                        ? new DeviceRgb(230, 240, 255)
                        : ColorConstants.WHITE;

                    table.AddCell(CreateCell(sale.Id.ToString(), bgColor));
                    table.AddCell(CreateCell(sale.SaleDate.ToString("dd/MM/yy HH:mm"), bgColor));
                    table.AddCell(CreateCell(sale.Customer?.FullName ?? "", bgColor));
                    table.AddCell(CreateCell(sale.Customer?.IdentificationNumber ?? "", bgColor));
                    table.AddCell(CreateCell(detail.Product?.Name ?? "", bgColor));
                    table.AddCell(CreateCell(detail.Quantity.ToString(), bgColor));
                    table.AddCell(CreateCell($"${detail.UnitPrice:N2}", bgColor));
                    table.AddCell(CreateCell($"${detail.Subtotal:N2}", bgColor));

                    alternate = !alternate;
                }
            }

            document.Add(table);

            // Grand total
            document.Add(new Paragraph($"\nGrand Total: ${sales.Sum(s => s.TotalAmount):N2}")
                .SetBold().SetFontSize(13));
            document.Add(new Paragraph($"Total transactions: {sales.Count}")
                .SetFontSize(10));

            document.Close();
            return stream.ToArray();
        }

        private static Cell CreateCell(string text, Color bgColor) =>
            new Cell().Add(new Paragraph(text)).SetBackgroundColor(bgColor);
    }
}
