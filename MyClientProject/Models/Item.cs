using System.ComponentModel.DataAnnotations;

namespace MyClientProject.Models
{
    public class Item
    {
        [Key]
        [Required]
        public int ItemId { get; set; }
        public string Category { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public double Weight { get; set; }
        public int DeliveryDays { get; set; }
        public string Description { get; set; } = string.Empty;
        [Required]
        public int StockQuantity { get; set; }
    }
}
