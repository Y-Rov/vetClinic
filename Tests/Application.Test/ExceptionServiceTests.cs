﻿using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using Core.Pagginator;
using Core.Pagginator.Parameters;
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


        [Fact]
        public async Task GetExceptionsAsync_Exceptions_ReturnsIEnumerableOfException()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(ExceptionServiceFixture._pagedListExceptions);

            // Act
            var result = await _exceptionServiceFixture.MockExceptionEntityService.GetAsync(ExceptionServiceFixture.pagingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, ExceptionServiceFixture._pagedListExceptions);
        }

        [Fact]
        public async Task GetExceptionAsync_Exception_ReturnException()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(ExceptionServiceFixture._exceptionEntity);

            // Act
            var result = await _exceptionServiceFixture.MockExceptionEntityService.GetAsync(ExceptionServiceFixture._id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, ExceptionServiceFixture._exceptionEntity);
        }

        [Fact]
        public async Task GetExceptionAsync_Exception_ThrowsNotFoundException()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetAsync(It.IsAny<int>()))
                .Throws<NotFoundException>();

            // Act
            var result = _exceptionServiceFixture.MockExceptionEntityService.GetAsync(ExceptionServiceFixture._id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetTodayExceptionsAsync_TodayExceptions_ReturnsIEnumerableOfException()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetTodayAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(ExceptionServiceFixture._pagedListExceptions);

            // Act
            var result = await _exceptionServiceFixture.MockExceptionEntityService.GetTodayAsync(ExceptionServiceFixture.pagingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, ExceptionServiceFixture._pagedListExceptions);

            var resultAndExpect = result!.Zip(ExceptionServiceFixture._exceptionEntities, (result, expect) => new { Result = result, Expect = expect });
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
                .Setup(r => r.GetStatsAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(ExceptionServiceFixture._pagedListExceptionStats);

            // Act
            var result = await _exceptionServiceFixture.MockExceptionEntityService.GetStatsAsync(ExceptionServiceFixture.pagingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, ExceptionServiceFixture._pagedListExceptionStats);
        }

        [Fact]
        public async Task GetExceptionTodayStatsAsync_ExceptionTodayStats_ReturnsIEnumerableOfExceptionStats()
        {
            // Arrange
            _exceptionServiceFixture.MockExceptionRepository
                .Setup(r => r.GetTodayStatsAsync(It.IsAny<ExceptionParameters>()))
                .ReturnsAsync(ExceptionServiceFixture._pagedListExceptionStats);

            // Act
            var result = await _exceptionServiceFixture.MockExceptionEntityService.GetTodayStatsAsync(ExceptionServiceFixture.pagingParameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, ExceptionServiceFixture._pagedListExceptionStats);
        }
    }
}