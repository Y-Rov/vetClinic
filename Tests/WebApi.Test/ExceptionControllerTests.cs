using Core.Entities;
using Core.Models;
using Core.Pagginator;
using Core.Pagginator.Parameters;
using Core.ViewModel;
using Microsoft.AspNetCore.Http;
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
                .ReturnsAsync(ExceptionControllerFixture._paggedListExceptions);

            _exceptionControllerFixture.MockMapperException
                .Setup(m => m.Map(It.IsAny<IEnumerable<ExceptionEntity>>()))
                .Returns(ExceptionControllerFixture._paggedListViewModelsExceptions);

            _exceptionControllerFixture.MockExceptionController.ControllerContext = new ControllerContext();
            _exceptionControllerFixture.MockExceptionController.ControllerContext.HttpContext = new DefaultHttpContext();
            // Act

            var result = await _exceptionControllerFixture.MockExceptionController.GetAsync(ExceptionControllerFixture.paggingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, ExceptionControllerFixture._paggedListViewModelsExceptions);
        }

        [Fact]
        public async Task GetAsync_Exception_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(ExceptionControllerFixture._exceptionEntity);

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetAsync(ExceptionControllerFixture._id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, ExceptionControllerFixture._exceptionEntity);
        }

        [Fact]
        public async Task GetStatsAsync_ExceptionStats_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetStatsAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(ExceptionControllerFixture._paggedListExceptionStats);

            _exceptionControllerFixture.MockExceptionController.ControllerContext = new ControllerContext();
            _exceptionControllerFixture.MockExceptionController.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetStatsAsync(ExceptionControllerFixture.paggingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, ExceptionControllerFixture._paggedListExceptionStats);
        }

        [Fact]
        public async Task GetTodayAsync_TodayExceptions_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetTodayAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(ExceptionControllerFixture._paggedListExceptions);

            _exceptionControllerFixture.MockMapperException
                .Setup(m => m.Map(It.IsAny<IEnumerable<ExceptionEntity>>()))
                .Returns(ExceptionControllerFixture._paggedListViewModelsExceptions);

            _exceptionControllerFixture.MockExceptionController.ControllerContext = new ControllerContext();
            _exceptionControllerFixture.MockExceptionController.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetTodayAsync(ExceptionControllerFixture.paggingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, ExceptionControllerFixture._paggedListViewModelsExceptions);
        }

        [Fact]
        public async Task GetTodayStatsAsync_TodayExceptionStats_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetTodayStatsAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(ExceptionControllerFixture._paggedListExceptionStats);

            _exceptionControllerFixture.MockExceptionController.ControllerContext = new ControllerContext();
            _exceptionControllerFixture.MockExceptionController.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetTodayStatsAsync(ExceptionControllerFixture.paggingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, ExceptionControllerFixture._paggedListExceptionStats);
        }
    }
}
