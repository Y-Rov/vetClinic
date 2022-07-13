using Core.Entities;
using Core.Models;
using Core.ViewModels.FeedbackViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //_fixture.MockFeedbackService.Setup(service =>
            //    service.GetAllFeedbacks(
            //        It.IsAny<string?>(),
            //        It.IsAny<int>(),
            //        It.IsAny<int>()))
            //    .ReturnsAsync(_fixture.ExpectedFeedbacks);

            //_fixture.MockListFeedbackMapper.Setup(mapper =>
            //    mapper.Map(It.IsAny<IEnumerable<Feedback>>()))
            //.Returns(_fixture.ExpectedFeedbacksViewModel);

            //var result = await _fixture.MockController.GetAllFeedbacks(_fixture.TestParameters);

            //Assert.NotNull(result);
            //Assert.NotEmpty(result);
            //Assert.IsAssignableFrom<IEnumerable<FeedbackReadViewModel>>(result);
        }

        [Fact]
        public async Task GetAllFeedbacks_whenFeedbacksDontExist_thenReturnEmptyList()
        {
            //List<Feedback> emptyFeedbacks = new List<Feedback>();
            //List<FeedbackReadViewModel> emptyViewModelFeedbacks = new List<FeedbackReadViewModel>();

            //_fixture.MockFeedbackService.Setup(service =>
            //    service.GetAllFeedbacks(
            //        It.IsAny<string?>(),
            //        It.IsAny<int>(),
            //        It.IsAny<int>()))
            //    .ReturnsAsync(emptyFeedbacks);

            //_fixture.MockListFeedbackMapper.Setup(mapper =>
            //    mapper.Map(It.IsAny<IEnumerable<Feedback>>()))
            //.Returns(emptyViewModelFeedbacks);

            //var result = await _fixture.MockController.GetAllFeedbacks(_fixture.TestParameters);

            //Assert.NotNull(result);
            //Assert.Empty(result);
            //Assert.IsAssignableFrom<IEnumerable<FeedbackReadViewModel>>(result);
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
