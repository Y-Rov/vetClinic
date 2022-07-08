using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class PortfolioControllerTest : IClassFixture<PortfolioControllerFixture>, IDisposable
    {
        private readonly PortfolioControllerFixture _fixture;
        private bool _disposed;

        public PortfolioControllerTest(PortfolioControllerFixture fixture)
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
                _fixture.MockPortfolioService.ResetCalls();
            }

            _disposed = true;
        }

        [Fact]
        public async Task GetAsync_WhenListOfPortfoliosIsNotEmpty_ThenListOfPortfolioCreateReadViewModelsIsReturned()
        {
            // Arrange
            _fixture.MockPortfolioService
                .Setup(service => service.GetAllPortfoliosAsync())
                .ReturnsAsync(_fixture.Portfolios);

            _fixture.MockPortfolioReadEnumerableViewModelMapper
                .Setup(mapper => mapper.Map(It.Is<IEnumerable<Portfolio>>(match => match.Any())))
                .Returns(_fixture.PortfolioCreateReadViewModels)
                .Verifiable();

            // Act
            var result = await _fixture.MockPortfolioController.GetAsync();

            // Assert
            _fixture.MockPortfolioService
                .Verify(service => service.GetAllPortfoliosAsync(), Times.Once);

            _fixture.MockPortfolioReadEnumerableViewModelMapper
                .Verify();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IEnumerable<PortfolioCreateReadViewModel>>(result);

            _fixture.MockPortfolioService.ResetCalls();
        }

        [Fact]
        public async Task GetAsync_WhenListOfPortfoliosIsEmpty_ThenEmptyListOfPortfolioCreateReadViewModelsIsReturned()
        {
            // Arrange
            _fixture.MockPortfolioService
                .Setup(service => service.GetAllPortfoliosAsync())
                .ReturnsAsync(new List<Portfolio>());

            _fixture.MockPortfolioReadEnumerableViewModelMapper
                .Setup(mapper => mapper.Map(It.Is<IEnumerable<Portfolio>>(match => !match.Any())))
                .Returns(new List<PortfolioCreateReadViewModel>())
                .Verifiable();

            // Act
            var result = await _fixture.MockPortfolioController.GetAsync();

            // Assert
            _fixture.MockPortfolioService
                .Verify(service => service.GetAllPortfoliosAsync(), Times.Once);

            _fixture.MockPortfolioReadEnumerableViewModelMapper
                .Verify();

            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<PortfolioCreateReadViewModel>>(result);

            _fixture.MockPortfolioService.ResetCalls();
        }

        [Fact]
        public async Task GetAsync_WhenUserIdIsCorrect_ThenPortfolioBaseViewModelIsReturned()
        {
            // Arrange
            _fixture.MockPortfolioService
                .Setup(service => service.GetPortfolioByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)))
                .ReturnsAsync(_fixture.ReadyPortfolio);

            _fixture.MockPortfolioReadViewModelMapper
                .Setup(mapper => mapper.Map(It.IsAny<Portfolio>()))
                .Returns(_fixture.ReadyPortfolioBaseViewModel)
                .Verifiable();

            // Act
            var result = await _fixture.MockPortfolioController.GetAsync(_fixture.UserId);

            // Assert
            _fixture.MockPortfolioService
                .Verify(service => service.GetPortfolioByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)), Times.Once);

            _fixture.MockPortfolioReadViewModelMapper
                .Verify();

            Assert.NotNull(result);
            Assert.IsType<PortfolioBaseViewModel>(result);

            _fixture.MockPortfolioService.ResetCalls();
        }

        [Fact]
        public async Task CreateAsync_WhenPortfolioCreateReadViewModelIsCorrect_ThenNoContentResultIsReturned()
        {
            // Arrange
            _fixture.MockPortfolioCreateMapper
                .Setup(mapper => mapper.Map(It.IsAny<PortfolioCreateReadViewModel>()))
                .Returns(_fixture.ReadyPortfolio)
                .Verifiable();

            _fixture.MockPortfolioService
                .Setup(service => service.CreatePortfolioAsync(It.IsAny<Portfolio>()))
                .Returns(Task.FromResult<object?>(null));

            // Act
            var result = await _fixture.MockPortfolioController.CreateAsync(_fixture.ReadyPortfolioCreateReadViewModel);

            // Assert
            _fixture.MockPortfolioService
                .Verify(service => service.CreatePortfolioAsync(It.IsAny<Portfolio>()), Times.Once);

            _fixture.MockPortfolioCreateMapper
                .Verify();

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAsync_WhenPortfolioBaseViewModelIsCorrect_ThenNoContentResultIsReturned()
        {
            // Arrange
            _fixture.MockPortfolioService
                .Setup(service => service.GetPortfolioByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)))
                .ReturnsAsync(_fixture.ReadyPortfolio);

            _fixture.MockPortfolioUpdateMapper
                .Setup(mapper => mapper.Map(
                    It.IsAny<PortfolioBaseViewModel>(),
                    It.IsAny<Portfolio>()));

            _fixture.MockPortfolioService
                .Setup(service => service.UpdatePortfolioAsync(It.IsAny<Portfolio>()))
                .Returns(Task.FromResult<object?>(null));

            // Act
            var result = await _fixture.MockPortfolioController.UpdateAsync(_fixture.UserId, _fixture.ReadyPortfolioBaseViewModel);

            // Assert
            _fixture.MockPortfolioService
                .Verify(service => service.GetPortfolioByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)), Times.Once);

            _fixture.MockPortfolioService
                .Verify(service => service.UpdatePortfolioAsync(It.IsAny<Portfolio>()), Times.Once);

            _fixture.MockPortfolioCreateMapper
                .Verify();

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);

            _fixture.MockPortfolioService.ResetCalls();
        }

        [Fact]
        public async Task DeleteAsync_WhenUserIdIsCorrect_ThenNoContentResultIsReturned()
        {
            // Arrange
            _fixture.MockPortfolioService
                .Setup(service => service.DeletePortfolioByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)))
                .Returns(Task.FromResult<object?>(null));

            // Act
            var result = await _fixture.MockPortfolioController.DeleteAsync(_fixture.UserId);

            // Assert
            _fixture.MockPortfolioService
                .Verify(service => service.DeletePortfolioByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)), Times.Once);

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
