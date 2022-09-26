using System.Collections.ObjectModel;
using CompulsoryPractice.Core.Model;
using CompulsoryPractice.Core.Repository;

namespace CompulsoryPractice.Core.Service;

public class ReviewService : IReviewService
{
    private IReviewRepository _reviewRepository;
    public ReviewService(IReviewRepository repo)
    {
        if (repo == null)
            throw new ArgumentException("Missing repository");
        _reviewRepository = repo;
    }
    
    // 1. On input N, what are the number of reviews from reviewer N?
    public int GetNumberOfReviewsFromReviewer(int reviewer)
    {
        int count = 0;
        foreach (BEReview review in _reviewRepository.GetAllReviews())
        {
            if (review.Reviewer == reviewer)
                count++;
        }
        return count;
    }
    
    // 2. On input N, what is the average rate that reviewer N had given?
    public double GetAverageRateFromReviewer(int reviewer)
    {
        List<BEReview> reviewList = _reviewRepository.GetAllReviews().ToList();
        if (!reviewList.Select(review => review.Reviewer).Contains(reviewer))
        {
            throw new ArgumentException("Reviewer does not exist");
        }
        return reviewList
                .Where(r => r.Reviewer == reviewer)
                .Select(r => r.Grade)
                .Average();
    }

    // 3. On input N and R, how many times has reviewer N given rate R?
    public int GetNumberOfRatesByReviewer(int reviewer, int rate)
    {
        if (reviewer <= 0)
        {
            throw new ArgumentException( "Id of reviewer is not valid");
        }

        if (rate < 1 || rate > 5)
        {
            throw new ArgumentException("Invalid value of rating");
        }

        int count = 0;
        foreach (BEReview review in _reviewRepository.GetAllReviews())
        {
            if (review.Reviewer == reviewer && review.Grade == rate)
                count++;
        }
        return count;
    }
    
    // 4. On input N, how many have reviewed movie N?
    public int GetNumberOfReviews(int movie)
    {
        List<BEReview> reviews = _reviewRepository.GetAllReviews().ToList();
        if (!reviews.Select(r => r.Movie).Contains(movie))
            throw new ArgumentException("Movie does not exist");
        return reviews.Count(r => r.Movie == movie);
    }

    // 5. On input N, what is the average rate the movie N had received?
    public double GetAverageRateOfMovie(int movie)
    {
        if (movie <= 0)
        {
            throw new ArgumentException("Invalid movie ID");
        }

        int total = 0;
        int count = 0;
        foreach (BEReview review in _reviewRepository.GetAllReviews())
        {
            if (review.Movie == movie)
            {
                total += review.Grade;
                count++;
            }
        }
        return (double) total/count;
    }

    // 6. On input N and R, how many times had movie N received rate R?
    public int GetNumberOfRates(int movie, int rate)
    {
        if (!(1 <= rate && rate <= 5))
        {
            throw new ArgumentException("Invalid value for rate");
        }
        List<BEReview> reviewList = _reviewRepository.GetAllReviews().ToList();
        if (!reviewList.Select(review => review.Movie).Contains(movie))
        {
            throw new ArgumentException("Movie does not exist");
        }
        return reviewList
            .Where(review => review.Movie == movie)
            .Count(review => review.Grade == rate);
    }

    // 7. What is the id(s) of the movie(s) with the highest number of top rates (5)?
    public List<int> GetMoviesWithHighestNumberOfTopRates()
    {
        // This will create a dictionary that looks something like this
        //  key,       value
        //movieId, count of 5* reviews
        //  1    ,      1
        //  2    ,      2
        //  3    ,      2
        Dictionary<int, int> dictionary = _reviewRepository.GetAllReviews().Select(r => r)
            .Where(r => r.Grade == 5)
            .GroupBy(review => review.Movie)
            .ToDictionary(reviews => reviews.Key, reviews => reviews.Count());
        
        // We search for the biggest number (of occurrences) in values
        int maxValue = dictionary.Max(pair => pair.Value);
        
        // We list the movies (keys) that have the same value as our biggest number 
        return dictionary.Where(pair => pair.Value == maxValue).Select(pair => pair.Key).ToList();
    }

    // 8. What reviewer(s) had done most reviews?
    public List<int> GetMostProductiveReviewers()
    {
        Dictionary<int, int> dictionary = _reviewRepository.GetAllReviews()
            .GroupBy(review => review.Reviewer)
            .ToDictionary(reviews => reviews.Key, reviews => reviews.Count());
        int maxValue = dictionary.Max(pair => pair.Value);
        return dictionary.Where(pair => pair.Value == maxValue).Select(pair => pair.Key).ToList();

    }

    public List<int> GetTopRatedMovies(int amount)
    {
        throw new NotImplementedException();
    }

    public List<int> GetTopMoviesByReviewer(int reviewer)
    {
        throw new NotImplementedException();
    }

    public List<int> GetReviewersByMovie(int movie)
    {
        throw new NotImplementedException();
    }
}