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
      
        public List<int> Items { get; set; } = new();

        public DateTime OrderDate { get; set; } = DateTime.Now;

      

    }
}
