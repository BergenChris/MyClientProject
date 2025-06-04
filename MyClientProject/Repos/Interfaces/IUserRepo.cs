using MyClientProject.Models;

namespace MyClientProject.Repos.Interfaces
{
    public interface IUserRepo
    {
        User? Get(int id);
        IEnumerable<User> GetAll();

        Task<List<Item>> GetShoppingListFromUser(int id);
    }
}
