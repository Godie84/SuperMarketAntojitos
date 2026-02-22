using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketAntojitos.Models.Entities
{
    public class Sale
    {
        public int Id { get; set; }

        [Display(Name = "Sale Date")]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        public string? CashierId { get; set; }
        public ApplicationUser Cashier { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public ICollection<SaleDetail> SaleDetails { get; set; } = null!;
    }
}
