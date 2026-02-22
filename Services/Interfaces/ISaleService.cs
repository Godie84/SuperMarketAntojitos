using SuperMarketAntojitos.Models.Entities;
using SuperMarketAntojitos.Models.ViewModels;

namespace SuperMarketAntojitos.Services.Interfaces
{
    public interface ISaleService
    {
        Task<Customer?> FindCustomerByIdentificationAsync(string identificationNumber);

        Task<(bool Success, string Message)> RegisterSaleAsync(
            int customerId,
            string cashierId,
            List<SaleDetailViewModel> details);
    }
}