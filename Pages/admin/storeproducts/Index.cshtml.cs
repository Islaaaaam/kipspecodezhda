using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Data;
using SleekClothing.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleekClothing.Pages.admin.storeproducts
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<StoreProduct> StoreProducts { get; set; }

        public async Task OnGetAsync()
        {
            StoreProducts = await _context.StoreProducts
                .Include(sp => sp.Product)
                .Include(sp => sp.Store)
                .ToListAsync();
        }
    }
}