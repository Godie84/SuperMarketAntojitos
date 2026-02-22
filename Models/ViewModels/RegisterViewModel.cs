using System.ComponentModel.DataAnnotations;

namespace SuperMarketAntojitos.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required, MaxLength(100)]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required, EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required, DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
