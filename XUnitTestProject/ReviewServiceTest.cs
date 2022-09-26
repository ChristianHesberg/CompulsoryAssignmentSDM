using System.Security;
using CompulsoryPractice.Core.Model;
using CompulsoryPractice.Core.Repository;
using CompulsoryPractice.Core.Service;
using Moq;
using ArgumentException = System.ArgumentException;

namespace XUnitTestProject;

public class ReviewServiceTest
{
    [Fact]
    public void CreateReviewServiceWithRepository()
    {
        //Arrange
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        IReviewRepository repository = mockRepository.Object;

        //Act
        IReviewService service = new ReviewService(repository);
        
        //Assert
        Assert.NotNull(service);
        Assert.True(service is ReviewService);
    }

    [Fact]
    public void CreateReviewServiceWithNoRepositoryExpectArgumentException()
    {
        //Arrange
        IReviewService service = null;

        //Act + Assert
        ArgumentException e = Assert.Throws<ArgumentException>(() => service = new ReviewService(null));
        Assert.Null(service);
        Assert.Equal("Missing repository", e.Message);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(3, 0)]
    public void GetNumberOfReviewsFromReviewer(int reviewer, int expectedResult)
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 3, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 3, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 1, Movie = 2, Grade = 3, ReviewDate = new DateTime()}
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        
        //Act
        int result = service.GetNumberOfReviewsFromReviewer(reviewer);
        
        //Assert
        Assert.Equal(expectedResult, result);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    [Theory]
    [InlineData(1, 1.5)]
    [InlineData(2, 4.5)]
    public void GetAverageRateFromReviewer(int reviewer, double expectedAverage)
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 4, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 5, ReviewDate = new DateTime()}
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);
        
        IReviewService service = new ReviewService(mockRepo.Object);
        //Act
        double actualAverage = service.GetAverageRateFromReviewer(reviewer);
        
        //Assert
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
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 4, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 5, ReviewDate = new DateTime()}
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);
        
        IReviewService service = new ReviewService(mockRepo.Object);

        //Act+Assert
        var ex = Assert.Throws<ArgumentException>(() => service.GetAverageRateFromReviewer(reviewer));
        Assert.Equal("Reviewer does not exist", ex.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    public void GetNumberOfReviews(int movie, int expectedAmount)
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 1, Movie = 2, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 4, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 5, ReviewDate = new DateTime()}
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);
        
        IReviewService service = new ReviewService(mockRepo.Object);

        int actualAmount = service.GetNumberOfReviews(movie);
        
        //Assert
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
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 1, Movie = 2, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 4, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 5, ReviewDate = new DateTime()}
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);
        
        IReviewService service = new ReviewService(mockRepo.Object);
        
        //Act+Assert
        var ex = Assert.Throws<ArgumentException>(() => service.GetNumberOfReviews(movie));
        Assert.Equal("Movie does not exist", ex.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
    }
    
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
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
            // 1 review with grade 4
            new BEReview() { Reviewer = 3, Movie = 2, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 3, Movie = 2, Grade = 4, ReviewDate = new DateTime()},
            // 2 reviews with grade 3
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 3, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 3, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 4, ReviewDate = new DateTime()},
            // 3 review with grade 3
            new BEReview() { Reviewer = 4, Movie = 1, Grade = 3, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 4, Movie = 1, Grade = 3, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 4, Movie = 2, Grade = 3, ReviewDate = new DateTime()}
            
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
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        //Assert
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
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 2, ReviewDate = new DateTime()},
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);

        //Assert
        var ex = Assert.Throws<ArgumentException>(() => service.GetNumberOfRatesByReviewer(1, 743));
        Assert.Equal("Invalid value of rating", ex.Message);
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Never);
        
    }
    
    [Theory]
    [InlineData(1,3.0)]
    [InlineData(2,2.0)]
    public void  GetAverageRateOfMovie(int movie, double expectedAverage)
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 5, ReviewDate = new DateTime()},
            
            new BEReview() { Reviewer = 3, Movie = 2, Grade = 2, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 4, Movie = 2, Grade = 2, ReviewDate = new DateTime()},
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        double averageResult = service.GetAverageRateOfMovie(movie);
        //Assert
        Assert.Equal(expectedAverage, averageResult );
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
        
    }
    
    [Fact]
    public void  GetAverageRateOfMovie_WithInvalidId()
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime()},
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 5, ReviewDate = new DateTime()}
        };

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(repo => repo.GetAllReviews()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepo.Object);
        
        //Assert
        var exception = Assert.Throws<ArgumentException>(() =>service.GetAverageRateOfMovie(-1));
        Assert.Equal("Invalid movie ID", exception.Message );
        mockRepo.Verify(repo => repo.GetAllReviews(), Times.Never);
        
    }
    
    
    
}