using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SleekClothing.Data;
using SleekClothing.Models;
using System.Threading.Tasks;

namespace SleekClothing.Pages.admin.stores
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var storeInDb = await _context.Stores.FindAsync(Store.Id);
            if (storeInDb == null)
            {
                return NotFound();
            }

            storeInDb.Name = Store.Name;
            storeInDb.Address = Store.Address;
            storeInDb.Latitude = Store.Latitude;
            storeInDb.Longitude = Store.Longitude;

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
