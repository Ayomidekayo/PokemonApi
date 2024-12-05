using PokemonApi.Moldels;

namespace PokemonApi.InterFace
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewsOfAPokemon(int PokeId);
        bool ReviewExist(int Id);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> review);
        bool Save();
    }
}
