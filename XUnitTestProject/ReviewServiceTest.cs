using System.Runtime.InteropServices;
using System.Security;
using Castle.Components.DictionaryAdapter;
using CompulsoryPractice.Core.Model;
using CompulsoryPractice.Core.Repository;
using CompulsoryPractice.Core.Service;
using Moq;
using ArgumentException = System.ArgumentException;

namespace XUnitTestProject;

public class ReviewServiceTest
{
    #region Service Creation

    [Fact]
    public void CreateReviewServiceWithRepository()
    {
        // Arrange
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        IReviewRepository repository = mockRepository.Object;

        // Act
        IReviewService service = new ReviewService(repository);

        // Assert
        Assert.NotNull(service);
        Assert.True(service is ReviewService);
    }

    [Fact]
    public void CreateReviewServiceWithNoRepositoryExpectArgumentException()
    {
        // Arrange
        IReviewService service = null;

        // Act + Assert
        ArgumentException e = Assert.Throws<ArgumentException>(() => service = new ReviewService(null));
        Assert.Null(service);
        Assert.Equal("Missing repository", e.Message);
    }

    #endregion
    

    #region Method 1

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(3, 0)]
    public void GetNumberOfReviewsFromReviewer(int reviewer, int expectedResult)
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 2, Grade = 3, ReviewDate = new DateTime() }
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        // Act
        int result = service.GetNumberOfReviewsFromReviewer(reviewer);

        // Assert
        Assert.Equal(expectedResult, result);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    #endregion

    
    #region Method 2

    [Theory]
    [InlineData(1, 1.5)]
    [InlineData(2, 4.5)]
    public void GetAverageRateFromReviewer(int reviewer, double expectedAverage)
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 4, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 5, ReviewDate = new DateTime() }
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        
        // Act
        double actualAverage = service.GetAverageRateFromReviewer(reviewer);

        // Assert
        Assert.Equal(expectedAverage, actualAverage);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(17)]
    [InlineData(null)]
    public void GetAverageRateFromReviewer_WithNonExistentReviewer(int reviewer)
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 4, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 5, ReviewDate = new DateTime() }
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.GetAverageRateFromReviewer(reviewer));
        Assert.Equal("Reviewer does not exist", ex.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    #endregion

    
    #region Method 3
    
    [Theory]
    [InlineData(1, 3, 0)]
    [InlineData(3, 4, 1)]
    [InlineData(2, 3, 2)]
    [InlineData(4, 3, 3)]
    public void GetNumberOfRatesByReviewer(int reviewer, int rating, int expectedResult)
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            // no review with the grade
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
            // 1 review with grade 4
            new BEReview() { Reviewer = 3, Movie = 2, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 3, Movie = 2, Grade = 4, ReviewDate = new DateTime() },
            // 2 reviews with grade 3
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 4, ReviewDate = new DateTime() },
            // 3 review with grade 3
            new BEReview() { Reviewer = 4, Movie = 1, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 4, Movie = 1, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 4, Movie = 2, Grade = 3, ReviewDate = new DateTime() }
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        //Act
        int result = service.GetNumberOfRatesByReviewer(reviewer, rating);

        //Assert
        Assert.Equal(expectedResult, result);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }


    [Fact]
    public void GetNumberOfRatesByReviewer_WithNegativeReviewerId_ExpectArgumentException()
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.GetNumberOfRatesByReviewer(-1, 5));
        Assert.Equal("Id of reviewer is not valid", ex.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Never);
    }

    [Fact]
    public void GetNumberOfRatesByReviewer_WithInvalidRating_ExpectArgumentException()
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.GetNumberOfRatesByReviewer(1, 743));
        Assert.Equal("Invalid value of rating", ex.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Never);
    }

    #endregion
    
    
    #region Method 4

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    public void GetNumberOfReviews(int movie, int expectedAmount)
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 2, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 4, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 5, ReviewDate = new DateTime() }
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        // Act
        int actualAmount = service.GetNumberOfReviews(movie);

        // Assert
        Assert.Equal(expectedAmount, actualAmount);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(222)]
    [InlineData(null)]
    public void GetNumberOfReviewsInvalidDataExpectArgumentException(int movie)
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 2, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 4, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 5, ReviewDate = new DateTime() }
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.GetNumberOfReviews(movie));
        Assert.Equal("Movie does not exist", ex.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    #endregion
    
    
    #region Method 5

    [Theory]
    [InlineData(1, 3.0)]
    [InlineData(2, 2.0)]
    public void GetAverageRateOfMovie(int movie, double expectedAverage)
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 5, ReviewDate = new DateTime() },

            new BEReview() { Reviewer = 3, Movie = 2, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 4, Movie = 2, Grade = 2, ReviewDate = new DateTime() },
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        
        // Act
        double averageResult = service.GetAverageRateOfMovie(movie);
        
        // Assert
        Assert.Equal(expectedAverage, averageResult);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    [Fact]
    public void GetAverageRateOfMovie_WithInvalidId()
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 5, ReviewDate = new DateTime() }
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        // Act + Assert
        var exception = Assert.Throws<ArgumentException>(() => service.GetAverageRateOfMovie(-1));
        Assert.Equal("Invalid movie ID", exception.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Never);
    }

    #endregion

    
    #region Method 6

    [Theory]
    [InlineData(1, 2, 0)]
    [InlineData(2, 3, 1)]
    [InlineData(3, 5, 3)]
    public void GetNumberOfRates(int movie, int rate, int expected)
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 5, ReviewDate = new DateTime() },

            new BEReview() { Reviewer = 3, Movie = 2, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 3, Movie = 2, Grade = 4, ReviewDate = new DateTime() },

            new BEReview() { Reviewer = 4, Movie = 3, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 4, Movie = 3, Grade = 5, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 4, Movie = 3, Grade = 5, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 4, Movie = 3, Grade = 5, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 4, Movie = 3, Grade = 4, ReviewDate = new DateTime() },
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        
        // Act
        int actual = service.GetNumberOfRates(movie, rate);
        
        // Assert
        Assert.Equal(expected, actual);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }
    
    [Theory]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    [InlineData(1, 6)]
    public void GetNumberOfRates_WithInvalidRating_ExpectArgumentException(int movie, int rate)
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime() }
        };
        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        
        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.GetNumberOfRates(movie, rate));
        Assert.Equal("Invalid value for rate", ex.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Never);
    }

    [Theory]
    [InlineData(123, 1)]
    [InlineData(13131, 2)]
    [InlineData(-7, 3)]
    public void GetNumberOfRates_WithInvalidMovie_ExpectArgumentException(int movie, int rate)
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime() }
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.GetNumberOfRates(movie, rate));
        Assert.Equal("Movie does not exist", ex.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    #endregion


    #region Method 7

    [Fact]
    public void GetMoviesWithHighestNumberOfTopRates()
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 5, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 5, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 3, Movie = 2, Grade = 5, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 4, Movie = 3, Grade = 5, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 5, Movie = 3, Grade = 5, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 6, Movie = 4, Grade = 4, ReviewDate = new DateTime() },
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        // Act
        List<int> result = service.GetMoviesWithHighestNumberOfTopRates();

        // Assert
        Assert.NotEmpty(result);

        Assert.Contains(2, result);
        Assert.Contains(3, result);

        Assert.DoesNotContain(1, result);
        Assert.DoesNotContain(4, result);

        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    #endregion


    #region Method8

    [Fact]
    public void GetMostProductiveReviewers()
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 3, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 3, Movie = 1, Grade = 2, ReviewDate = new DateTime() },
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        
        // Act
        List<int> actual = service.GetMostProductiveReviewers();
        
        // Assert
        Assert.Single(actual);
        Assert.Equal(new List<int>(){1}, actual);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);

    }
    
    [Fact]
    public void GetMostProductiveReviewers_WithTwoMostProductiveReviewer()
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 3, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 3, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 3, Movie = 1, Grade = 2, ReviewDate = new DateTime()}
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        // Act
        List<int> actual = service.GetMostProductiveReviewers();

        // Assert
        Assert.Equal(2, actual.Count);
        Assert.Equal(new List<int>(){1,3}, actual);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    #endregion
    
    //Should we test for an empty list of most productive reviewers if there are no reviews? 

    #region Method 9

    [Theory]
    [InlineData(5)]
    [InlineData(4)]
    [InlineData(3)]
    [InlineData(2)]
    [InlineData(1)]
    public void GetTopRatedMovies(int amount)
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 5, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 4, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 3, Movie = 3, Grade = 3, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 4, Movie = 4, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 5, Movie = 5, Grade = 1, ReviewDate = new DateTime()},
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        
        // Act
        List<int> topRatedMovies = service.GetTopRatedMovies(amount);

        // Assert
        Assert.Equal(amount, topRatedMovies.Count);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.AtLeastOnce);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-20000)]
    public void GetTopRatedMovies_InvalidArgument_ExpectArgumentException(int amount)
    {
        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();

        IReviewService service = new ReviewService(mockRepo.Object);
        
        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.GetTopRatedMovies(amount));
        Assert.Equal("Amount can't be less or equal to 0!", ex.Message);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void GetTopRatedMovies_AmountIsMoreThanAvailable_ExpectException(int amount)
    {
        // Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 5, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 4, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 3, Movie = 3, Grade = 3, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 4, Movie = 4, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 5, Movie = 5, Grade = 1, ReviewDate = new DateTime()},
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        
        // Act + Assert
        var ex = Assert.Throws<Exception>(() => service.GetTopRatedMovies(amount));
        Assert.Equal("Desired amount is more than available!", ex.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.AtLeastOnce);
    }

    #endregion
}