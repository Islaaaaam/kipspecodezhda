using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Data;
using SleekClothing.Helpers;
using SleekClothing.Models;

namespace SleekClothing.Pages.admin.orders
{
    public class IndexModel : PageModel
    {
        private readonly SleekClothing.Data.ApplicationDbContext _context;

        public IndexModel(SleekClothing.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Orders { get; set; }


        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                Orders = await _context.Orders.ToListAsync();
            }
        }
    }
}
