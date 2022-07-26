using Core.Entities;
using Core.Models;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.ExceptionViewModel;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class ExceptionControllerTests : IClassFixture<ExceptionControllerFixture>
    {
        public ExceptionControllerTests(ExceptionControllerFixture exceptionControllerFixture)
        {
            _exceptionControllerFixture = exceptionControllerFixture;
        }

        private readonly ExceptionControllerFixture _exceptionControllerFixture;

        [Fact]
        public async Task GetAsync_Exceptions_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(_exceptionControllerFixture._pagedListExceptions);

            _exceptionControllerFixture.MockExceptionToPagedModel
                 .Setup(m => m.Map(It.IsAny<PagedList<ExceptionEntity>>()))
                .Returns(_exceptionControllerFixture._pagedExceptionsReadViewModel);

            // Act

            var result = await _exceptionControllerFixture.MockExceptionController.GetAsync(_exceptionControllerFixture.pagingParameters);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<PagedReadViewModel<ExceptionEntityReadViewModel>>>(result);
        }

        [Fact]
        public async Task GetAsync_Exception_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(_exceptionControllerFixture._exceptionEntity);

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetAsync(_exceptionControllerFixture._id);
            var exceptions = (result.Result as OkObjectResult)!.Value as ExceptionEntity;
            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _exceptionControllerFixture._exceptionEntity);
            Assert.NotNull(exceptions);
        }

        [Fact]
        public async Task GetStatsAsync_ExceptionStats_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetStatsAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(_exceptionControllerFixture._pagedListExceptionStats);

            _exceptionControllerFixture.MockExceptionStatesToPagedModel
                    .Setup(m => m.Map(It.IsAny<PagedList<ExceptionStats>>()))
                   .Returns(_exceptionControllerFixture._pagedExceptionsStatsReadViewModel);

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetStatsAsync(_exceptionControllerFixture.pagingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<PagedReadViewModel<ExceptionStatsReadViewModel>>>(result);
        }

        [Fact]
        public async Task GetTodayAsync_TodayExceptions_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetTodayAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(_exceptionControllerFixture._pagedListExceptions);

            _exceptionControllerFixture.MockExceptionToPagedModel
                .Setup(m => m.Map(It.IsAny<PagedList<ExceptionEntity>>()))
               .Returns(_exceptionControllerFixture._pagedExceptionsReadViewModel);

            // Act

            var result = await _exceptionControllerFixture.MockExceptionController.GetAsync(_exceptionControllerFixture.pagingParameters);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<PagedReadViewModel<ExceptionEntityReadViewModel>>>(result);

        }

        [Fact]
        public async Task GetTodayStatsAsync_TodayExceptionStats_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetTodayStatsAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(_exceptionControllerFixture._pagedListExceptionStats);

            _exceptionControllerFixture.MockExceptionStatesToPagedModel
                               .Setup(m => m.Map(It.IsAny<PagedList<ExceptionStats>>()))
                              .Returns(_exceptionControllerFixture._pagedExceptionsStatsReadViewModel);

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetStatsAsync(_exceptionControllerFixture.pagingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<PagedReadViewModel<ExceptionStatsReadViewModel>>>(result);
        }
    }
}
