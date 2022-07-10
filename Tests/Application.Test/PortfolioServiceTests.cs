using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Moq;
using System.Linq.Expressions;

namespace Application.Test
{
    public class PortfolioServiceTests : IClassFixture<PortfolioServiceFixture>, IDisposable
    {
        private readonly PortfolioServiceFixture _fixture;
        private bool _disposed;

        public PortfolioServiceTests(PortfolioServiceFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _fixture.MockPortfolioRepository.ResetCalls();
            }

            _disposed = true;
        }

        [Fact]
        public async Task GetAllPortfoliosAsync_WhenListOfPortfoliosIsRequested_ThenListOfPortfoliosIsReturned()
        {
            // Arrange
            _fixture.MockPortfolioRepository
                .Setup(repository => repository.GetAsync(
                    It.IsAny<Expression<Func<Portfolio, bool>>>(),
                    It.IsAny<Func<IQueryable<Portfolio>, IOrderedQueryable<Portfolio>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(_fixture.TestListOfPortfolios);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = await _fixture.MockPortfolioService.GetAllPortfoliosAsync();

            // Assert
            _fixture.MockPortfolioRepository
                .Verify(repository => repository.GetAsync(
                    It.IsAny<Expression<Func<Portfolio, bool>>>(),
                    It.IsAny<Func<IQueryable<Portfolio>, IOrderedQueryable<Portfolio>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()), Times.Once);

            _fixture.MockLoggerManager
                .Verify();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Portfolio>>(result);
        }

        [Fact]
        public async Task GetPortfolioByUserIdAsync_WhenPortfolioExists_ThenPortfolioIsReturned()
        {
            // Arrange
            _fixture.MockPortfolioRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.TestPortfolio);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = await _fixture.MockPortfolioService.GetPortfolioByUserIdAsync(_fixture.TestPortfolio.UserId);

            // Assert
            _fixture.MockPortfolioRepository
                .Verify(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()), Times.Once);

            _fixture.MockLoggerManager
                .Verify();

            Assert.NotNull(result);
            Assert.IsType<Portfolio>(result);
        }

        [Fact]
        public async Task GetPortfolioByUserIdAsync_WhenPortfolioNotExists_ThenNotFoundExceptionIsThrown()
        {
            // Arrange
            _fixture.MockPortfolioRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(match => match != _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = _fixture.MockPortfolioService.GetPortfolioByUserIdAsync(_fixture.WrongUserId);

            // Assert
            _fixture.MockLoggerManager
                .Verify();

            await Assert.ThrowsAsync<NotFoundException>(() => result);
            Assert.True(result.IsFaulted);
        }

        [Fact]
        public async Task CreatePortfolioAsync_WhenUserPortfolioWasUndefinedBefore_ThenItIsSavedInDatabaseAndTaskIsReturned()
        {
            // Arrange
            _fixture.MockPortfolioRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _fixture.MockPortfolioRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Portfolio>()))
                .Returns(Task.FromResult<object?>(null));

            _fixture.MockPortfolioRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null));

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = _fixture.MockPortfolioService.CreatePortfolioAsync(_fixture.TestPortfolio);

            // Assert
            _fixture.MockPortfolioRepository
                .Verify(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()), Times.Once);

            _fixture.MockPortfolioRepository
                .Verify(repo => repo.InsertAsync(It.IsAny<Portfolio>()), Times.Once);

            _fixture.MockPortfolioRepository
                .Verify(repo => repo.SaveChangesAsync(), Times.Once);

            _fixture.MockLoggerManager
                .Verify();

            await result;
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task CreatePortfolioAsync_WhenUserPortfolioWasDefinedAlready_ThenBadRequestExceptionIsThrown()
        {
            _fixture.MockPortfolioRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.TestPortfolio);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = _fixture.MockPortfolioService.CreatePortfolioAsync(_fixture.TestPortfolio);

            // Assert
            _fixture.MockLoggerManager
                .Verify();

            await Assert.ThrowsAsync<BadRequestException>(() => result);
            Assert.True(result.IsFaulted);
        }

        [Fact]
        public async Task UpdatePortfolioAsync_WhenPortfolioIsCorrect_ThenItIsUpdatedSuccessfullyAndTaskIsReturned()
        {
            _fixture.MockPortfolioRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.TestPortfolio);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = _fixture.MockPortfolioService.UpdatePortfolioAsync(_fixture.TestPortfolio);

            // Assert
            _fixture.MockPortfolioRepository
                .Verify(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()), Times.Once);

            _fixture.MockLoggerManager
                .Verify();

            await result;
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task DeletePortfolioByUserIdAsync_WhenUserIdIsCorrect_ThenPortfolioIsDeletedSuccessfullyAndTaskIsReturned()
        {
            _fixture.MockPortfolioRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.TestPortfolio);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockPortfolioRepository
                .Setup(repo => repo.Delete(It.IsAny<Portfolio>()));

            _fixture.MockPortfolioRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null));

            // Act
            var result = _fixture.MockPortfolioService.DeletePortfolioByUserIdAsync(_fixture.TestPortfolio.UserId);

            // Assert
            _fixture.MockPortfolioRepository
                .Verify(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()), Times.Once);

            _fixture.MockPortfolioRepository
                .Verify(repo => repo.Delete(It.IsAny<Portfolio>()), Times.Once);

            _fixture.MockPortfolioRepository
                .Verify(repo => repo.SaveChangesAsync(), Times.Once);

            _fixture.MockLoggerManager
                .Verify();

            await result;
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task DeletePortfolioByUserIdAsync_WhenUserIdIsIncorrect_ThenNotFoundExceptionIsThrown()
        {
            _fixture.MockPortfolioRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId != _fixture.TestPortfolio.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = _fixture.MockPortfolioService.DeletePortfolioByUserIdAsync(_fixture.WrongUserId);

            // Assert
            _fixture.MockLoggerManager
                .Verify();

            await Assert.ThrowsAsync<NotFoundException>(() => result);
            Assert.True(result.IsFaulted);
        }
    }
}
