using MyClientProject.Models;

namespace MyClientProject.Repos.Interfaces
{
    public interface IUserRepo
    {
        Task<User> GetUserByIdAsync(int userId);
        IEnumerable<User> GetAll();

        Task<List<Item>> GetShoppingListFromUser(int id);
    }
}
