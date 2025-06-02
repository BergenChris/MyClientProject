using MyClientProject.Models;

namespace MyClientProject.Repos
{
    public interface IUserRepo
    {
        User? Get(int id);
        IEnumerable<User> GetAll();
    }
}
