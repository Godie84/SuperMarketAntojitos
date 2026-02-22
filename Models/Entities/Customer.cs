using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketAntojitos.Models.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        [Display(Name = "ID Number")]
        public string? IdentificationNumber { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required, MaxLength(200)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required, MaxLength(15)]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [Required, EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [ValidateNever]
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public string FullName => $"{FirstName} {LastName}";
    }
}
