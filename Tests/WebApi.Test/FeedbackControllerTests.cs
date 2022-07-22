using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.FeedbackViewModels;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class FeedbackControllerTests : IClassFixture<FeedbackControllerFixture>, IDisposable
    {
        private readonly FeedbackControllerFixture _fixture;
        private bool _disposed;

        public FeedbackControllerTests(FeedbackControllerFixture fixture)
        {
            _fixture = fixture;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _fixture.MockFeedbackService.ResetCalls();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetAllFeedbacks_whenFeedbacksExist_thenReturnFeedbacks()
        {
            _fixture.MockFeedbackService.Setup(service =>
                service.GetAllFeedbacks(It.IsAny<FeedbackParameters>()))
            .ReturnsAsync(_fixture.ExpectedFeedbacks);

            _fixture.MockPagedMapper.Setup(mapper =>
                mapper.Map(It.IsAny<PagedList<Feedback>>()))
            .Returns(_fixture.ExpectedFeedbacksViewModel);

            var result = await _fixture.MockController.GetAllFeedbacks(_fixture.TestParameters);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Entities);
            Assert.IsType<PagedReadViewModel<FeedbackReadViewModel>>(result);
        }

        [Fact]
        public async Task GetAllFeedbacks_whenFeedbacksDontExist_thenReturnEmptyList()
        {
            _fixture.MockFeedbackService.Setup(service =>
                service.GetAllFeedbacks(It.IsAny<FeedbackParameters>()))
            .ReturnsAsync(_fixture.EmptyFeedbacks);

            _fixture.MockPagedMapper.Setup(mapper =>
                mapper.Map(It.IsAny<PagedList<Feedback>>()))
            .Returns(_fixture.EmptyReadFeedbacks);

            var result = await _fixture.MockController.GetAllFeedbacks(_fixture.TestParameters);

            Assert.NotNull(result);
            Assert.Empty(result.Entities);
            Assert.IsType<PagedReadViewModel<FeedbackReadViewModel>>(result);
        }

        [Fact]
        public async Task AddFeedback_whenDataIsCorrect_thenExecuteMethods()
        {
            _fixture.MockCreateFeedbackMapper.Setup(mapper =>
                mapper.Map(It.IsAny<FeedbackCreateViewModel>()))
            .Returns(_fixture.TestFeedback);

            await _fixture.MockController.AddFeedback(_fixture.TestFeedbackCreateViewModel);

            _fixture.MockCreateFeedbackMapper.Verify(
                mapper => mapper.Map(_fixture.TestFeedbackCreateViewModel), Times.Once);
            _fixture.MockFeedbackService.Verify(
                service => service.AddFeedback(_fixture.TestFeedback), Times.Once);
        }
    }
}
