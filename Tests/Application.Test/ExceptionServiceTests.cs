using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using Core.Pagginator;
using Moq;

namespace Application.Test
{
    public class ExceptionServiceTests : IClassFixture<ExceptionServiceFixture>
    {
        public ExceptionServiceTests(ExceptionServiceFixture exceptionServiceFixture)
        {
            _exceptionServiceFixture = exceptionServiceFixture;
        }

        private readonly ExceptionServiceFixture _exceptionServiceFixture;
        private static readonly int _id = 1;

        private static readonly ExceptionEntity _exceptionEntity = new()
        {
            Id = _id,
            Name = "NotFoundException",
            DateTime = DateTime.Now,
            StackTrace = @" at Application.Services.ExceptionEntityService.GetAsync(Int32 id)",
            Path = @"/api/exceptions/1"
        };

        private static readonly ExceptionStats _exceptionStat = new()
        {
            Count = 1,
            Name = "NotFoundException"
        };

        private static readonly List<ExceptionStats> _exceptionStats = new() { _exceptionStat };
        private static readonly List<ExceptionEntity> _exceptionEntities = new() { _exceptionEntity };
        private static readonly PagedList<ExceptionEntity> _paggedListExceptions = PagedList<ExceptionEntity>.ToPagedList(_exceptionEntities.AsQueryable(), 1, 10);
        private static readonly PaggingParameters paggingParameters = new()
        {
            PageNumber = 1,
            PageSize = 10,
        };

        [Fact]
        public async Task GetExceptionsAsync_Exceptions_ReturnsIEnumerableOfException()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetAsync(paggingParameters))
                .ReturnsAsync(_paggedListExceptions);

            // Act
            var result = await _exceptionServiceFixture.MockExceptionEntityService.GetAsync(paggingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _paggedListExceptions);
        }

        [Fact]
        public async Task GetExceptionAsync_Exception_ReturnException()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(_exceptionEntity);

            // Act
            var result = await _exceptionServiceFixture.MockExceptionEntityService.GetAsync(_id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _exceptionEntity);
        }

        [Fact]
        public async Task GetExceptionAsync_Exception_ThrowsNotFoundException()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetAsync(It.IsAny<int>()))
                .Throws<NotFoundException>();

            // Act
            var result = _exceptionServiceFixture.MockExceptionEntityService.GetAsync(_id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetTodayExceptionsAsync_TodayExceptions_ReturnsIEnumerableOfException()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetTodayAsync())
                .ReturnsAsync(_exceptionEntities);

            // Act
            var result = await _exceptionServiceFixture.MockExceptionEntityService.GetTodayAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _exceptionEntities);

            var resultAndExpect = result!.Zip(_exceptionEntities, (result, expect) => new { Result = result, Expect = expect });
            foreach (var item in resultAndExpect)
            {
                Assert.Equal(item.Result.DateTime.Date, item.Expect.DateTime.Date);
            }
        }

        [Fact]
        public async Task GetExceptionStatsAsync_ExceptionStats_ReturnsIEnumerableOfExceptionStats()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetStatsAsync())
                .ReturnsAsync(_exceptionStats);

            // Act
            var result = await _exceptionServiceFixture.MockExceptionEntityService.GetStatsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _exceptionStats);
        }

        [Fact]
        public async Task GetExceptionTodayStatsAsync_ExceptionTodayStats_ReturnsIEnumerableOfExceptionStats()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetTodayStatsAsync())
                .ReturnsAsync(_exceptionStats);

            // Act
            var result = await _exceptionServiceFixture.MockExceptionEntityService.GetTodayStatsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _exceptionStats);
        }
    }
}
