using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SleekClothing.Data;
using SleekClothing.Models;
using System.Threading.Tasks;

namespace SleekClothing.Pages.admin.stores
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Store Store { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Store = await _context.Stores.FindAsync(id);

            if (Store == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var storeInDb = await _context.Stores.FindAsync(Store.Id);

            if (storeInDb == null)
            {
                return NotFound();
            }

            _context.Stores.Remove(storeInDb);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
