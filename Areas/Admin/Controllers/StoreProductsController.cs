using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Data;
using SleekClothing.Models;

namespace SleekClothing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoreProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoreProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int storeId)
        {
            var store = _context.Stores.Find(storeId);
            ViewBag.Store = store;

            var storeProducts = _context.StoreProducts
                .Where(sp => sp.StoreId == storeId)
                .ToList();

            return View(storeProducts);
        }

        public IActionResult Create(int storeId)
        {
            var products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            ViewBag.Products = products;
            ViewBag.StoreId = storeId;

            return View();
        }

        [HttpPost]
        public IActionResult Create(StoreProduct storeProduct)
        {
            _context.StoreProducts.Add(storeProduct);
            _context.SaveChanges();
            return RedirectToAction("Index", new { storeId = storeProduct.StoreId });
        }

        public IActionResult Edit(int id)
        {
            var storeProduct = _context.StoreProducts.Find(id);

            var products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            ViewBag.Products = products;

            return View(storeProduct);
        }

        [HttpPost]
        public IActionResult Edit(StoreProduct storeProduct)
        {
            _context.StoreProducts.Update(storeProduct);
            _context.SaveChanges();
            return RedirectToAction("Index", new { storeId = storeProduct.StoreId });
        }

        public IActionResult Delete(int id)
        {
            var storeProduct = _context.StoreProducts
                                .Include(sp => sp.Product)
                                .FirstOrDefault(sp => sp.Id == id);
            return View(storeProduct);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var storeProduct = _context.StoreProducts.Find(id);
            var storeId = storeProduct.StoreId;

            _context.StoreProducts.Remove(storeProduct);
            _context.SaveChanges();

            return RedirectToAction("Index", new { storeId });
        }
    }
}