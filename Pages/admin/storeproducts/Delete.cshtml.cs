using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SleekClothing.Data;
using SleekClothing.Models;
using System.Threading.Tasks;

namespace SleekClothing.Pages.admin.storeproducts
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var storeProductInDb = await _context.StoreProducts.FindAsync(StoreProduct.Id);

            if (storeProductInDb == null)
            {
                return NotFound();
            }

            _context.StoreProducts.Remove(storeProductInDb);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index", new { storeId = StoreProduct.StoreId });
        }
    }
}
