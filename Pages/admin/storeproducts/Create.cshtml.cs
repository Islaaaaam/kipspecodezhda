using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SleekClothing.Data;
using SleekClothing.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SleekClothing.Pages.admin.storeproducts
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public StoreProduct StoreProduct { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Stores"] = new SelectList(_context.Stores, "Id", "Name");
            ViewData["Products"] = new SelectList(_context.Products, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Проверка обязательных полей
            if (StoreProduct.StoreId == 0)
            {
                StoreProduct.StoreId = _context.Stores.Select(s => s.Id).FirstOrDefault();
            }

            if (StoreProduct.ProductId == 0)
            {
                StoreProduct.ProductId = _context.Products.Select(p => p.Id).FirstOrDefault();
            }

            if (StoreProduct.Quantity == 0)
            {
                StoreProduct.Quantity = 1;
            }

            try
            {
                _context.StoreProducts.Add(StoreProduct);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index", new { storeId = StoreProduct.StoreId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating store product.");
                // Обработка ошибки и возврат соответствующего результата
                return RedirectToPage("./Index", new { storeId = StoreProduct.StoreId, error = "Произошла ошибка при добавлении товара в магазин." });
            }
        }
    }
}
