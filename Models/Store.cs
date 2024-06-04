using System.ComponentModel.DataAnnotations;

namespace SleekClothing.Models
{
    public class Store
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}