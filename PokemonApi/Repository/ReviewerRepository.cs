using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly Appdbcontext _dbcontext;

        public ReviewerRepository(Appdbcontext dbcontext)
        {
            this._dbcontext = dbcontext;
        }
        public Reviewer GetReviewer(int id)
        {
            return _dbcontext.Reviewers.Where(u => u.Id == id).Include(x=>x.Reviews) .FirstOrDefault();
        }

        public bool ReviewerExist(int id)
        {
           return _dbcontext.Reviewers.Any(u => u.Id == id);    
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _dbcontext.Reviewers.OrderBy(u => u.Id).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewId)
        {
            return _dbcontext.Reviews.Where(u=>u.Reviewer.Id==reviewId).ToList();
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _dbcontext.Add(reviewer);
            return Save();
                
        }

        public bool Save()
        {
            var Saved=_dbcontext.SaveChanges();
            return Saved >0? true: false;
        }

        public bool UpateReviewer(Reviewer reviewer)
        {
            _dbcontext.Update(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _dbcontext.Remove(reviewer);
            return Save();
        }
    }
}
