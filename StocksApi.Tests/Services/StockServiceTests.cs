using Xunit;
using Moq;
using StocksApi.Services.Implementations;
using StocksApi.Repositories.Interfaces;
using StocksApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StocksApi.Tests
{
    public class StockServiceTests
    {
        [Fact]
        public async Task GetStockByIdAsync_ReturnsStock_WhenStockExists()
        {
            // Arrange
            var mockRepo = new Mock<IStockRepository>();
            var stockId = 1;
            var expectedStock = new Stock { Id = stockId, Symbol = "Test Stock" };
            mockRepo.Setup(r => r.GetByIdAsync(stockId, It.IsAny<CancellationToken>())).ReturnsAsync(expectedStock);

            var service = new StockService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(stockId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedStock.Id, result.Id);
            Assert.Equal(expectedStock.Symbol, result.Symbol);
        }

        [Fact]
        public async Task GetStockByIdAsync_ReturnsNull_WhenStockDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IStockRepository>();
            var stockId = 2;
            mockRepo.Setup(r => r.GetByIdAsync(stockId, It.IsAny<CancellationToken>())).ReturnsAsync((Stock)null);

            var service = new StockService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(stockId, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
