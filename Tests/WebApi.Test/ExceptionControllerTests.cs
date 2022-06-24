using Core.Entities;
using Core.Models;
using Core.Pagginator;
using Core.ViewModel;
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
        private static readonly int _id = 1;
        private static readonly ExceptionEntity _exceptionEntity = new()
        {
            Id = _id,
            Name = "NotFoundException",
            DateTime = DateTime.Now,
            StackTrace = @" at Application.Services.ExceptionEntityService.GetAsync(Int32 id)",
            Path = @"/api/exceptions/1"
        };
        private static readonly ExceptionEntityReadViewModel _exceptionEntityReadViewModel = new()
        {
            Id = _id,
            Name = "NotFoundException",
            DateTime = DateTime.Now,
            Path = @"/api/exceptions/1"
        };
        private static readonly ExceptionStats _exceptionStat = new()
        {
            Count = 1,
            Name = "NotFoundException"
        };

        private static readonly List<ExceptionStats> _exceptionStats = new() { _exceptionStat };
        private static readonly List<ExceptionEntity> _exceptionEntities = new() { _exceptionEntity };
        private static readonly List<ExceptionEntityReadViewModel> _exceptionEntityReadViewModels = new() { _exceptionEntityReadViewModel };
        private static readonly PagedList<ExceptionEntity> _paggedListExceptions = PagedList<ExceptionEntity>.ToPagedList(_exceptionEntities.AsQueryable(), 1, 10);
        private static readonly PagedList<ExceptionEntityReadViewModel> _paggedListViewModelsExceptions = PagedList<ExceptionEntityReadViewModel>.ToPagedList(_exceptionEntityReadViewModels.AsQueryable(), 1, 10);
        [Fact]
        public async Task GetAsync_Exceptions_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetAsync(new PaggingParameters()))
                .ReturnsAsync(_paggedListExceptions);

            _exceptionControllerFixture.MockMapperException
                .Setup(m => m.Map(It.IsAny<IEnumerable<ExceptionEntity>>()))
                .Returns(_paggedListViewModelsExceptions);

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetAsync(new PaggingParameters());

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _paggedListViewModelsExceptions);
        }

        [Fact]
        public async Task GetAsync_Exception_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetAsync(_id))
                .ReturnsAsync(_exceptionEntity);

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetAsync(_id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _exceptionEntity);
        }

        [Fact]
        public async Task GetStatsAsync_ExceptionStats_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetStatsAsync())
                .ReturnsAsync(_exceptionStats);

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetStatsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _exceptionStats);
        }

        [Fact]
        public async Task GetTodayAsync_TodayExceptions_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetTodayAsync())
                .ReturnsAsync(_exceptionEntities);

            _exceptionControllerFixture.MockMapperException
                .Setup(m => m.Map(It.IsAny<IEnumerable<ExceptionEntity>>()))
                .Returns(_exceptionEntityReadViewModels);

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetTodayAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _exceptionEntityReadViewModels);
        }

        [Fact]
        public async Task GetTodayStatsAsync_TodayExceptionStats_ReturnsOkObjectResult()
        {
            // Arrange
            _exceptionControllerFixture.MockExceptionService
                .Setup(s => s.GetTodayStatsAsync())
                .ReturnsAsync(_exceptionStats);

            // Act
            var result = await _exceptionControllerFixture.MockExceptionController.GetTodayStatsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _exceptionStats);
        }
    }
}
