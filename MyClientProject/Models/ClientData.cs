namespace MyClientProject.Models
{
    public class ClientData : UserData
    {
        public List<ShippingAdress> ShippingAdresses { get; set; } = new();
    }
}
