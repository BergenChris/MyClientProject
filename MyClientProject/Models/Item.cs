namespace MyClientProject.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double Weight { get; set; }
        public int DeliveryDays { get; set; }
        public string Description { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
    }
}
