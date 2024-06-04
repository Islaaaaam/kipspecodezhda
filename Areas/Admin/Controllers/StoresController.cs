using Microsoft.AspNetCore.Mvc;
using SleekClothing.Data;
using SleekClothing.Models;

namespace SleekClothing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var stores = _context.Stores.ToList();
            return View(stores);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Store store)
        {
            _context.Stores.Add(store);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var store = _context.Stores.Find(id);
            return View(store);
        }

        [HttpPost]
        public IActionResult Edit(Store store)
        {
            _context.Stores.Update(store);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var store = _context.Stores.Find(id);
            return View(store);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var store = _context.Stores.Find(id);
            _context.Stores.Remove(store);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}