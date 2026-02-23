using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperMarketAntojitos.Models.Entities;

namespace SuperMarketAntojitos.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Obtener todos los cajeros registrados
            var cashiers = await _userManager.GetUsersInRoleAsync("Cashier");
            return View(cashiers);
        }
    }
}
