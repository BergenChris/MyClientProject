using MyClientProject.Models;

namespace MyClientProject.Repos.Interfaces
{
    public interface IUserRepo
    {
        Task AddUserAsync(User user); 
        Task<User> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(User user);
        Task SaveChangesAsync();
        IEnumerable<User> GetAll();
    }
}
