using PokemonApi.Moldels;

namespace PokemonApi.InterFace
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int id);
        ICollection<Review>  GetReviewsByReviewer(int reviewId);
        bool ReviewerExist(int id);
        bool CreateReviewer(Reviewer reviewer);
        bool UpateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        bool Save();
    }
}
