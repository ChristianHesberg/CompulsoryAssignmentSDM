using CompulsoryPractice.Core.Model;

namespace CompulsoryPractice.Core.Repository;

public interface IReviewRepository
{
    BEReview[] GetAllReviews();
}