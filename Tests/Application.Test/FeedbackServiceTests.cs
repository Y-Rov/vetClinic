using Application.Test.Fixtures;
using Core.Entities;
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
            string filterParam = null;
            int takeCount = 10;
            int skipCount = 0;

            _fixture.MockRepository.Setup(repository =>
                repository.QueryAsync(
                    It.IsAny<Expression<Func<Feedback, bool>>>(),
                    It.IsAny<Func<IQueryable<Feedback>, IOrderedQueryable<Feedback>>>(),
                    It.IsAny<Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>>>(),
                    It.Is<int>(take => take == takeCount),
                    It.Is<int>(skip => skip == skipCount),
                    It.IsAny<bool>()))
                .ReturnsAsync(_fixture.ExpectedFeedbacks);

            var result = await _fixture.MockService.GetAllFeedbacks(
                    filterParam,
                    takeCount,
                    skipCount
                );

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IList<Feedback>>(result);
        }

        [Fact]
        public async Task GetAllFeedbacks_whenFeedbacksDontExist_thenReturnEmptyList()
        {
            string? filterParam = null;
            int takeCount = 10;
            int skipCount = 0;

            List<Feedback> emptyFeedbacks = new List<Feedback>();

            _fixture.MockRepository.Setup(repository =>
                repository.QueryAsync(
                    It.IsAny<Expression<Func<Feedback, bool>>>(),
                    It.IsAny<Func<IQueryable<Feedback>, IOrderedQueryable<Feedback>>>(),
                    It.IsAny<Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>>>(),
                    It.Is<int>(take => take == takeCount),
                    It.Is<int>(skip => skip == skipCount),
                    It.IsAny<bool>()))
                .ReturnsAsync(emptyFeedbacks);

            var result = await _fixture.MockService.GetAllFeedbacks(
                    filterParam,
                    takeCount,
                    skipCount
                );

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