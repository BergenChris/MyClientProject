using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyClientProject.Models
{
    public class ShippingAdress
    {
        [Key]
        public int ShippingAdressId { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StateOrProvince { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

      
        public int? UserId { get; set; }  
        public User? User { get; set; } = null!;

        public int? StoreId { get; set; }
        public Store? Store { get; set; }

    }
}
