using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SleekClothing.Data;
using SleekClothing.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SleekClothing.Pages.admin.storeproducts
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StoreProduct StoreProduct { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            StoreProduct = await _context.StoreProducts.FindAsync(id);

            if (StoreProduct == null)
            {
                return NotFound();
            }

            ViewData["Products"] = new SelectList(_context.Products, "Id", "Name", StoreProduct.ProductId);
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

            var storeProductInDb = await _context.StoreProducts.FindAsync(StoreProduct.Id);
            if (storeProductInDb == null)
            {
                return NotFound();
            }

            storeProductInDb.Quantity = StoreProduct.Quantity;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index", new { storeId = StoreProduct.StoreId });
        }
    }
}
