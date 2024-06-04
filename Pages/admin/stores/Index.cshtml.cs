using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Data;
using SleekClothing.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleekClothing.Pages.admin.stores
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Store> Stores { get; set; }

        public async Task OnGetAsync()
        {
            Stores = await _context.Stores.ToListAsync();
        }
    }
}
