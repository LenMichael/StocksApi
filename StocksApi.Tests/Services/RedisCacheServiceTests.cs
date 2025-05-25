using Xunit;
using Moq;
using StackExchange.Redis;
using StocksApi.Services.Implementations;
using System;
using System.Threading.Tasks;

namespace StocksApi.Tests.Services
{
    public class RedisCacheServiceTests
    {
        [Fact]
        public async Task GetAsync_ReturnsValue_WhenKeyExists()
        {
            // Arrange
            var key = "test-key";
            var expectedValue = "test-value";

            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(db => db.StringGetAsync(key, It.IsAny<CommandFlags>()))
                  .ReturnsAsync(expectedValue);

            var mockConn = new Mock<IConnectionMultiplexer>();
            mockConn.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(mockDb.Object);

            var service = new RedisCacheService(mockConn.Object);

            // Act
            var result = await service.GetAsync(key);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public async Task SetAsync_CallsStringSetAsync_WithCorrectParameters()
        {
            // Arrange
            var key = "test-key";
            var value = "test-value";
            TimeSpan? expiration = TimeSpan.FromMinutes(5);

            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(db => db.StringSetAsync(key, value, expiration, It.IsAny<When>(), It.IsAny<CommandFlags>()))
                  .ReturnsAsync(true);

            var mockConn = new Mock<IConnectionMultiplexer>();
            mockConn.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(mockDb.Object);

            var service = new RedisCacheService(mockConn.Object);

            // Act
            await service.SetAsync(key, value, expiration);

            // Assert
            mockDb.Verify(db => db.StringSetAsync(key, value, expiration, It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
        }
    }
}
