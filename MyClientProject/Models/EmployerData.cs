namespace MyClientProject.Models
{
    public class EmployerData : UserData
    {
        public int YearsInBusiness { get; set; }
        public decimal Salary { get; set; }
        public int Schedule { get; set; }

        public int StoreCode { get; set; }
    }
}
