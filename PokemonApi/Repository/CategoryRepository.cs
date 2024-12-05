using PokemonApi.Data;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Appdbcontext _dbcontext;

        public CategoryRepository(Appdbcontext dbcontext)
        {
            this._dbcontext = dbcontext;
        }
        public bool CategoryExist(int id)
        {
            return _dbcontext.Categories.Any(u => u.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _dbcontext.Categories.Add(category);
            return save();
        }

        public bool DeleteCategory(Category category)
        {
            _dbcontext.Remove(category);
            return save();
        }

        public ICollection<Category> GetCategories()
        {
           return _dbcontext.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int id)
        {
          return _dbcontext.Categories.Where(u=>u.Id==id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategogy(int CategoryId)
        {
           return _dbcontext.PokemonCategories.Where(u=>u.CategoryId==CategoryId).Select(u=>u.Pokemon).ToList();
        }

        public bool save()
        {
          var save=  _dbcontext.SaveChanges();
            return save >0 ? true: false;
        }

        public bool UpdateCategory(Category category)
        {
           _dbcontext.Update(category);
            return save();
        }
    }
}
