using MyClientProject.Models;

namespace MyClientProject.Repos.Interfaces
{
    public interface IOrderRepo
    {
        public Order? Get(int id);
        public IEnumerable<Order> GetAllFromUser(int id);
    }
}
