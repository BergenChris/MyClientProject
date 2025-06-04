using System.ComponentModel.DataAnnotations;

namespace MyClientProject.Models
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        public required List<ShippingAdress> ShippingAdresses { get; set; }
    }
}
