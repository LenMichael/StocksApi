using Xunit;
using Microsoft.EntityFrameworkCore;
using StocksApi.Repositories.Implementations;
using StocksApi.Data;
using StocksApi.Models;
using StocksApi.Filters;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StocksApi.Tests.Repositories
{
    public class StockRepositoryTests
    {
        private ApplicationDBContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "StocksDb_" + System.Guid.NewGuid())
                .Options;
            return new ApplicationDBContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsStockToDatabase()
        {
            var dbContext = GetDbContext();
            var repo = new StockRepository(dbContext);
            var stock = new Stock { Symbol = "TEST", CompanyName = "Test Company", Purchase = 10, LastDiv = 1, Industry = "Tech", MarketCap = 1000 };

            var result = await repo.CreateAsync(stock, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("TEST", result.Symbol);
            Assert.Single(dbContext.Stocks);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsStock_WhenExists()
        {
            var dbContext = GetDbContext();
            var stock = new Stock { Symbol = "TEST", CompanyName = "Test Company", Purchase = 10, LastDiv = 1, Industry = "Tech", MarketCap = 1000 };
            dbContext.Stocks.Add(stock);
            dbContext.SaveChanges();

            var repo = new StockRepository(dbContext);

            var result = await repo.GetByIdAsync(stock.Id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(stock.Symbol, result.Symbol);
        }

        [Fact]
        public async Task DeleteAsync_RemovesStock_WhenExists()
        {
            var dbContext = GetDbContext();
            var stock = new Stock { Symbol = "TEST", CompanyName = "Test Company", Purchase = 10, LastDiv = 1, Industry = "Tech", MarketCap = 1000 };
            dbContext.Stocks.Add(stock);
            dbContext.SaveChanges();

            var repo = new StockRepository(dbContext);

            var deleted = await repo.DeleteAsync(stock.Id, CancellationToken.None);

            Assert.NotNull(deleted);
            Assert.Empty(dbContext.Stocks);
        }

        [Fact]
        public async Task StockExists_ReturnsTrue_WhenStockExists()
        {
            var dbContext = GetDbContext();
            var stock = new Stock { Symbol = "TEST", CompanyName = "Test Company", Purchase = 10, LastDiv = 1, Industry = "Tech", MarketCap = 1000 };
            dbContext.Stocks.Add(stock);
            dbContext.SaveChanges();

            var repo = new StockRepository(dbContext);

            var exists = await repo.StockExists(stock.Id);

            Assert.True(exists);
        }
    }
}
