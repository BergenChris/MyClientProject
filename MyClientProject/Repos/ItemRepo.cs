using MyClientProject.Data;
using MyClientProject.Models;

namespace MyClientProject.Repos
{
    public class ItemRepo : IItemRepo
    {
        private readonly ShopDbContext context;

        public ItemRepo(ShopDbContext context)
        {
            this.context = context;
        }

        public Item? Get(int id)
        {
            return context.
        }

        IEnumerable<Item> IItemRepo.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
