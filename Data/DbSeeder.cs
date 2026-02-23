using Microsoft.AspNetCore.Identity;
using SuperMarketAntojitos.Models.Entities;

namespace SuperMarketAntojitos.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Crear roles si no existen
            string[] roles = { "Admin", "Cashier" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Crear usuario administrador si no existe
            string adminEmail = "admin@antojitos.com";
            string adminPassword = "Febrero2026";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Administrador",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }
    }
}
