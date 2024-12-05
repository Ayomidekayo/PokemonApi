using PokemonApi.Data;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly Appdbcontext _context;

        public ReviewRepository(Appdbcontext context)
        {
            this._context = context;
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
           _context.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> review)
        {
            _context.RemoveRange(review);
            return Save();
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.Where(u => u.Id == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.OrderBy(u => u.Id).ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int PokeId)
        {
           return _context.Reviews.Where(u=>u.Pokemon.Id==PokeId).OrderBy(u => u.Id).ToList();
        }

        public bool ReviewExist(int Id)
        {
           return _context.Reviews.Any(u => u.Id == Id);
        }

        public bool Save()
        {
           var saved=_context.SaveChanges();
            return saved > 0? true: false;
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }
    }
}
