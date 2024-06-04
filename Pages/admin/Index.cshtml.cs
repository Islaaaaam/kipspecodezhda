using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SleekClothing.Data;
using SleekClothing.Models;

namespace SleekClothing.Pages.admin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public decimal TotalProfit { get; private set; }

        public IList<StoreProduct> StoreProducts { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            TotalProfit = CalculateTotalProfit();
            StoreProducts = await _context.StoreProducts
                .Include(s => s.Product)
                .Include(s => s.Store)
                .ToListAsync();
        }

        private decimal CalculateTotalProfit()
        {
            decimal totalProfit = 0;

            try
            {
                var orders = _context.Orders.ToList();

                foreach (var order in orders)
                {
                    var orderedProducts = JsonConvert.DeserializeObject<List<Product>>(order.ProductDataAsJson);
                    if (orderedProducts != null)
                    {
                        foreach (var product in orderedProducts)
                        {
                            var productFromDb = _context.Products.FirstOrDefault(p => p.Id == product.Id);
                            if (productFromDb != null)
                            {
                                totalProfit += product.CartQuantity * productFromDb.Price;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details to diagnose the issue
                Console.WriteLine("Exception in CalculateTotalProfit: " + ex.Message);
                // Optionally, rethrow the exception or handle it as per your application's error handling strategy
            }

            return totalProfit;
        }

    }
}

