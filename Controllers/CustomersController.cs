using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarketAntojitos.Data;
using SuperMarketAntojitos.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace SuperMarketAntojitos.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
            => View(await _context.Customers.OrderBy(c => c.LastName).ToListAsync());

        [HttpGet]
        public IActionResult Create(string? identificationNumber = null)
        {
            var model = new Customer
            {
                IdentificationNumber = identificationNumber ?? string.Empty
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            bool exists = await _context.Customers
                .AnyAsync(c => c.IdentificationNumber == model.IdentificationNumber);

            if (exists)
            {
                ModelState.AddModelError("IdentificationNumber",
                    "A customer with this ID number already exists.");
                return View(model);
            }

            _context.Customers.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Customer registered successfully.";

            return !string.IsNullOrEmpty(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return customer is null ? NotFound() : View(customer);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Customers.Update(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Customer updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
