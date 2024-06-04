using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Models;

namespace SleekClothing.Data
{
    // ApplicationDbContext наследует от IdentityDbContext, что позволяет использовать функции аутентификации и авторизации
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        // Конструктор класса, принимающий параметры настроек контекста базы данных
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet для продуктов. Представляет коллекцию всех продуктов в базе данных
        public DbSet<Product> Products { get; set; }

        // DbSet для категорий. Позволяет хранить и управлять категориями продуктов
        public DbSet<Category> Categories { get; set; }

        // DbSet для корзины пользователя. Хранит информацию о товарах в корзине каждого пользователя
        public DbSet<UserCart> UserCarts { get; set; }

        // DbSet для списка желаемых товаров пользователя. Позволяет хранить список избранных товаров пользователей
        public DbSet<UserWishlist> UserWishlists { get; set; }

        // DbSet для заказов. Содержит данные о заказах, совершенных пользователями
        public DbSet<Order> Orders { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<StoreProduct> StoreProducts { get; set; }
    }
}
