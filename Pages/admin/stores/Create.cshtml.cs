using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SleekClothing.Data;
using SleekClothing.Models;
using System.Threading.Tasks;

namespace SleekClothing.Pages.admin.stores
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Store Store { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Stores.Add(Store);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
