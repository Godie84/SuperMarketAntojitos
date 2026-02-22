using Microsoft.AspNetCore.Identity;

namespace SuperMarketAntojitos.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
