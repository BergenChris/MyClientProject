using MyClientProject.Data;
using MyClientProject.Models;

namespace MyClientProject.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly ShopDbContext context;

        public UserRepo(ShopDbContext context)
        {
            this.context = context;
        }

        public User? Get(int id)
        {
            return context.Users.FirstOrDefault(x=>x.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users;
        }
    }
}
