using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyClientProject.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [EmailAddress]
        [Display(Name = "E-mailadres")]
        public string UserEmail { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
      
        public List<int> Items { get; set; } = new();

        public DateTime OrderDate { get; set; } =  TimeZoneInfo
            .ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));



    }
}
