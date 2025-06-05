using MyClientProject.Data;
using MyClientProject.Models;
using MyClientProject.Repos.Interfaces;

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
            return context.Items.FirstOrDefault(x => x.ItemId == id);
        }

        public IEnumerable<Item> GetAll()
        {
            return context.Items;
        }
    }
}
