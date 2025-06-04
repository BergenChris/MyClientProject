using Microsoft.AspNetCore.Identity;
using MyClientProject.Models;
using System.ComponentModel.DataAnnotations;

namespace MyClientProject.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int Discount { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime LastPurchaseDate { get; set; }

        public List<ShippingAdress> ShippingAdresses { get; set; } = null!;

        public  List<Item>? ShoppingList { get; set; }
    
    }

    
}
