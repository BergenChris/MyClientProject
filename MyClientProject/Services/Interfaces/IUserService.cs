using MyClientProject.Models;

namespace MyClientProject.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(User user);
        Task<List<int>> GetShoppingListIdsFromUser(int userId);
        Task<List<Item>> GetShoppingListFromUser(int userId);
        Task ClearShoppingListAfterOrder(int userId);
        Task SaveChangesAsync();
        public List<User> GetAll();
    }
}
