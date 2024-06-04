using Microsoft.Build.Framework;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using System.ComponentModel.DataAnnotations;

namespace SleekClothing.Models
{
    public class Order
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Имя должно быть по длине меньше 50 символов. ")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Фамилия должна быть по длине меньше 50 символов.")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Email должен быть по длине меньше 10 символов.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool IsPickup { get; set; }

        [MaxLength(100, ErrorMessage = "Адрес должен быть по длине меньше 100 символов.")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Адрес 2")]
        public string? Address2 { get; set; } = null;

        [Display(Name = "Страна")]
        public string Country { get; set; }

        [Display(Name = "Провинция")]
        public string Province { get; set; }

        [MaxLength(10, ErrorMessage = "Почтовый индекс должен быть по длине меньше 10 символов.")]
        [Display(Name = "Почтовый индекс")]
        public string PostalCode { get; set; }

        [Required]
        public DateTime DateOrdered { get; set; } = DateTime.UtcNow;

        [Required]
        public string ProductDataAsJson { get; set; }

        public int? PickupStoreId { get; set; }

        public virtual Store PickupStore { get; set; }

        public double TotalCost { get; set; }

        [NotMapped]
        public List<Product> OrderedProducts
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<Product>>(ProductDataAsJson) ?? new List<Product>();
                }
                catch
                {
                    return new List<Product>();
                }
            }
        }
    }
}