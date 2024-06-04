// Models/StoreProduct.cs
using System.ComponentModel.DataAnnotations;

namespace SleekClothing.Models
{
    public class StoreProduct
    {
        public int Id { get; set; }

        [Required]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать товар.")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required(ErrorMessage = "Необходимо указать количество.")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть положительным числом.")]
        public int Quantity { get; set; }
    }
}