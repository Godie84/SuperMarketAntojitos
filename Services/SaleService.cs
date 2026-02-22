using SuperMarketAntojitos.Data;
using SuperMarketAntojitos.Models.Entities;
using SuperMarketAntojitos.Models.ViewModels;
using SuperMarketAntojitos.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SuperMarketAntojitos.Services
{
    public class SaleService : ISaleService
    {
        private readonly ApplicationDbContext _context;

        public SaleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> FindCustomerByIdentificationAsync(string identificationNumber)
            => await _context.Customers
                .FirstOrDefaultAsync(c => c.IdentificationNumber == identificationNumber);

        public async Task<(bool Success, string Message)> RegisterSaleAsync(
            int customerId, string cashierId, List<SaleDetailViewModel> details)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate stock for each product
                foreach (var detail in details)
                {
                    var product = await _context.Products.FindAsync(detail.ProductId);

                    if (product is null)
                        return (false, $"Product '{detail.ProductName}' was not found.");

                    if (product.StockQuantity < detail.Quantity)
                        return (false, $"Insufficient stock for '{product.Name}'. " +
                                       $"Available: {product.StockQuantity} unit(s).");
                }

                // Create the sale header
                var sale = new Sale
                {
                    CustomerId = customerId,
                    CashierId = cashierId,
                    SaleDate = DateTime.Now,
                    TotalAmount = details.Sum(d => d.Subtotal)
                };

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                // Create details and discount stock
                foreach (var detail in details)
                {
                    var product = await _context.Products.FindAsync(detail.ProductId);
                    product!.StockQuantity -= detail.Quantity;

                    _context.SaleDetails.Add(new SaleDetail
                    {
                        SaleId = sale.Id,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.UnitPrice
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, $"Sale #{sale.Id} registered successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Error registering sale: {ex.Message}");
            }
        }
    }
}
