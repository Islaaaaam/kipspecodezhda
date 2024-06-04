using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SleekClothing.Data;
using SleekClothing.Helpers;
using SleekClothing.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekClothing.Pages.checkout
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Order Order { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<SelectListItem> Stores { get; set; }
        public decimal CartTotal { get; set; }
        public decimal CartTotalAfterGst { get; set; }

        public bool IsEligibleForDiscount { get; set; }

        public async Task OnGet()
        {
            await LoadPageDataAsync();
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var cartItems = CartHelper.GetGroupedCartItemsDb(user.Id, _context);
            Order.ProductDataAsJson = JsonConvert.SerializeObject(cartItems);

            // Проверка количества заказов пользователя и применение скидки
            var orderCount = _context.Orders.Count(o => o.Email == user.Email);
            if (orderCount > 10)
            {
                Order.TotalCost = (double)(CartHelper.GetCartTotalDb(user.Id, _context) * (decimal)0.95 * (decimal)1.13); // Применение скидки 5% и добавление НДС
            }
            else
            {
                Order.TotalCost = (double)(CartHelper.GetCartTotalDb(user.Id, _context) * (decimal)1.13);
            }

            if (Order.IsPickup)
            {
                // Получение списка всех магазинов и их товаров
                var allStores = await _context.Stores.ToListAsync();
                var storeProducts = await _context.StoreProducts.ToListAsync();

                // Найти первый магазин, в котором есть все необходимые товары в нужном количестве
                var availableStore = allStores
                    .FirstOrDefault(store => cartItems.All(item =>
                        storeProducts.Any(sp => sp.StoreId == store.Id && sp.ProductId == item.Id && sp.Quantity >= item.CartQuantity)));


                if (availableStore == null)
                {
                    ModelState.AddModelError(string.Empty, "Нет магазина, в котором есть все товары в нужном количестве.");
                    await LoadPageDataAsync(); // Перезагрузка данных при ошибке
                    return Page();
                }
                if (!Order.PickupStoreId.HasValue)
                {
                    Order.PickupStoreId = availableStore.Id;
                }

                var store = _context.Stores.Find(Order.PickupStoreId);
                if (store == null)
                {
                    ModelState.AddModelError(string.Empty, "Выбранный магазин не существует.");
                    await LoadPageDataAsync(); // Перезагрузка данных при ошибке
                    return Page();
                }

                foreach (var item in cartItems)
                {
                    var storeProduct = _context.StoreProducts
                        .FirstOrDefault(sp => sp.StoreId == store.Id && sp.ProductId == item.Id);
                    if (storeProduct != null)
                    {
                        storeProduct.Quantity -= item.CartQuantity;
                        _context.StoreProducts.Update(storeProduct); // Обновление количества товара
                    }
                }

                // Установка стандартных значений для полей адреса при самовывозе
                Order.Address = "Самовывоз";
                Order.Address2 = "Самовывоз";
                Order.Country = "Самовывоз";
                Order.Province = "Самовывоз";
                Order.PostalCode = "000000";
            } else
            {
                // Проверка и установка стандартных значений для полей адреса при доставке
                if (string.IsNullOrEmpty(Order.Address))
                {
                    Order.Address = "Стандартный адрес";
                }

                if (string.IsNullOrEmpty(Order.Country))
                {
                    Order.Country = "Canada";
                }

                if (string.IsNullOrEmpty(Order.Province))
                {
                    Order.Province = "Alberta";
                }

                if (string.IsNullOrEmpty(Order.PostalCode))
                {
                    Order.PostalCode = "000000";
                }
            }

            // Уменьшение количества товаров на складе
            foreach (var item in cartItems)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.Id);
                if (product != null)
                {
                    product.Quantity -= item.CartQuantity;
                    _context.Products.Update(product); // Обновление количества товара
                }
            }

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            CartHelper.ClearCartDb(User, _context); // передаем ClaimsPrincipal вместо user.Id
            return RedirectToPage("/myorders/Index");
        } 

    

        private async Task LoadPageDataAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Если пользователь не найден, редирект на страницу логина
                RedirectToPage("/Identity/Account/Login");
                return;
            }

            // Получение данных корзины пользователя
            Products = CartHelper.GetGroupedCartItemsDb(user.Id, _context);
            CartTotal = CartHelper.GetCartTotalDb(user.Id, _context);

            // Проверка количества заказов пользователя и применение скидки
            var orderCount = _context.Orders.Count(o => o.Email == user.Email);
            if (orderCount > 10)
            {
                CartTotalAfterGst = CartTotal * (decimal)0.95 * (decimal)1.13; // Применение скидки 5% и добавление НДС
            }
            else
            {
                CartTotalAfterGst = CartTotal * (decimal)1.13;
            }

            IsEligibleForDiscount = _context.Orders.Count(o => o.Email == user.Email) > 10;

            // Получение списка всех магазинов и их товаров
            var allStores = _context.Stores.ToList();
            var storeProducts = _context.StoreProducts.ToList();

            // Фильтрация магазинов, в которых есть все товары в нужном количестве
            var availableStores = allStores
                .Where(store => Products.All(item =>
                    storeProducts.Any(sp => sp.StoreId == store.Id && sp.ProductId == item.Id && sp.Quantity >= item.CartQuantity)))
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList();

            // Если нет ни одного магазина, выводим сообщение об ошибке
            if (!availableStores.Any())
            {
                ModelState.AddModelError(string.Empty, "Нет магазинов, в которых есть все товары в нужном количестве.");
            }

            Stores = availableStores;
        }

        public IActionResult OnGetStoreInfo(int storeId)
        {
            var store = _context.Stores.Find(storeId);
            return Partial("_StoreInfo", store);
        }
    }
}