using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketAntojitos.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        [Display(Name = "Product Code")]
        public string? Code { get; set; }

        [Required, MaxLength(150)]
        [Display(Name = "Product Name")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Display(Name = "Stock")]
        public int StockQuantity { get; set; }

        [ValidateNever]
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }
}
