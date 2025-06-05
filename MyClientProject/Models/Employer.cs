using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyClientProject.Models
{
    public class Employer : User
    {

      
        public int EmployerDataId { get; set; }
        public int YearsInBusiness { get; set; }
        public decimal Salary { get; set; }
        public int Schedule { get; set; }
        [ForeignKey("Store")]
        public int StoreCode { get; set; }
    }
}
