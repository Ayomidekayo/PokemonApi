using PokemonApi.Moldels;

namespace PokemonApi.InterFace
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategogy(int CategoryId);
        bool CategoryExist(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool save();
    }
}
