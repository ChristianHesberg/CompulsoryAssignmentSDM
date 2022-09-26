﻿using CompulsoryPractice.Core.Model;
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

    public int GetNumberOfReviews(int movie)
    {
        List<BEReview> reviews = _reviewRepository.GetAllReviews().ToList();
        if (!reviews.Select(r => r.Movie).Contains(movie))
            throw new ArgumentException("Movie does not exist");
        return reviews.Count(r => r.Movie == movie);
    }

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

    public int GetNumberOfRates(int movie, int rate)
    {
        throw new NotImplementedException();
    }

    public List<int> GetMoviesWithHighestNumberOfTopRates()
    {
        throw new NotImplementedException();
    }

    public List<int> GetMostProductiveReviewers()
    {
        throw new NotImplementedException();
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