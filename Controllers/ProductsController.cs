using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarketAntojitos.Data;
using SuperMarketAntojitos.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace SuperMarketAntojitos.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
            => View(await _context.Products.OrderBy(p => p.Name).ToListAsync());

        [HttpGet] public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            bool codeExists = await _context.Products.AnyAsync(p => p.Code == model.Code);
            if (codeExists)
            {
                ModelState.AddModelError("Code", "Ya existe un producto con este código.");
                return View(model);
            }

            _context.Products.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Producto creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product is null ? NotFound() : View(product);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Products.Update(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Producto actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
