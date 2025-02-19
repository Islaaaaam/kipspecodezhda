﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Data;
using SleekClothing.Models;

namespace SleekClothing.Pages.products
{
    public class DetailsModel : PageModel
    {
        private readonly SleekClothing.Data.ApplicationDbContext _context;

        public DetailsModel(SleekClothing.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; }

        // Добавлено: свойство для хранения информации о наличии товара в магазинах
        public List<StoreProduct> StoreProducts { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;

                // Добавлено: загрузка информации о наличии товара в магазинах
                StoreProducts = await _context.StoreProducts
                    .Include(sp => sp.Store)
                    .Where(sp => sp.ProductId == product.Id)
                    .ToListAsync();
            }

            return Page();
        }
    }
}