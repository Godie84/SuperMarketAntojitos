using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuperMarketAntojitos.Data;
using SuperMarketAntojitos.Models.ViewModels;
using SuperMarketAntojitos.Services.Interfaces;
using SuperMarketAntojitos.Models.Entities;

namespace SuperMarketAntojitos.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalesController(ISaleService saleService,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _saleService = saleService;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = new SaleViewModel
            {
                ProductList = _context.Products
                    .Where(p => p.StockQuantity > 0)
                    .OrderBy(p => p.Name)
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"[{p.Code}] {p.Name} - ${p.UnitPrice:N2}"
                    }).ToList()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FindCustomer(string identificationNumber)
        {
            var customer = await _saleService
                .FindCustomerByIdentificationAsync(identificationNumber);

            if (customer is null)
                return Json(new { found = false });

            return Json(new
            {
                found = true,
                id = customer.Id,
                fullName = customer.FullName,
                phone = customer.Phone
            });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSale(
            int customerId, [FromBody] List<SaleDetailViewModel> details)
        {
            var cashierId = _userManager.GetUserId(User);
            var result = await _saleService.RegisterSaleAsync(customerId, cashierId, details);

            return Json(new { success = result.Success, message = result.Message });
        }
    }
}
