using MyClientProject.Models;

namespace MyClientProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public UserData UserData { get; set; } = new();
    }

    
}
