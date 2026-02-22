using Microsoft.AspNetCore.Mvc.Rendering;

namespace SuperMarketAntojitos.Models.ViewModels
{
    public class SaleViewModel
    {
        public string? CustomerIdentificationNumber { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerFullName { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;

        public int SelectedProductId { get; set; }
        public int SelectedQuantity { get; set; } = 1;

        public List<SaleDetailViewModel> Details { get; set; } = new();
        public decimal TotalAmount => Details.Sum(d => d.Subtotal);

        public List<SelectListItem> ProductList { get; set; } = new();
    }
}
