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
}