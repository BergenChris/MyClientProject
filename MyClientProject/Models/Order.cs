namespace MyClientProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }

        public List<Item> Items { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public ShippingAdress

    }
}
