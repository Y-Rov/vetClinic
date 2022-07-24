using Application.Test.Fixtures;
using Core.Entities;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace Application.Test
{
    public class FeedbackServiceTests : IClassFixture<FeedbackServiceFixture>, IDisposable
    {
        private readonly FeedbackServiceFixture _fixture;
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _fixture.MockRepository.ResetCalls();
            }

            _disposed = true;
        }

        public FeedbackServiceTests(FeedbackServiceFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetAllFeedbacks_whenFeedbacksExist_thenReturnFeedbacks()
        {
            // Arrange
            _fixture.MockRepository.Setup(repository =>
                repository.GetPaged(
                    It.IsAny<FeedbackParameters>(),
                    It.IsAny<Expression<Func<Feedback, bool>>>(),
                    It.IsAny<Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>>>()))
                .ReturnsAsync(_fixture.ExpectedFeedbacks);

            // Act
            var result = await _fixture.MockService.GetAllFeedbacks(
                    _fixture.TestParameters
                );

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IList<Feedback>>(result);
        }

        [Fact]
        public async Task GetAllFeedbacks_whenFeedbacksDontExist_thenReturnEmptyList()
        {
            // Arrange
            _fixture.MockRepository.Setup(repository =>
                repository.GetPaged(
                It.IsAny<FeedbackParameters>(),
                It.IsAny<Expression<Func<Feedback, bool>>>(),
                It.IsAny<Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>>>()))
            .ReturnsAsync(_fixture.ExpectedEmptyFeedbacks);

            // Act
            var result = await _fixture.MockService.GetAllFeedbacks(_fixture.TestParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IList<Feedback>>(result);
        }

        [Fact]
        public async Task AddFeedback_whenFeedbackIsValid_thenAddFeedback()
        {
            await _fixture.MockService.AddFeedback(_fixture.TestFeedback);

            _fixture.MockRepository.Verify(repo => repo.InsertAsync(_fixture.TestFeedback), Times.Once);
            _fixture.MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
}