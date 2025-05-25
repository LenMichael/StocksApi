using Xunit;
using StocksApi.Validators.Stocks;
using StocksApi.Dtos.Stocks;
using System.Collections.Generic;

namespace StocksApi.Tests.Validators.Stocks
{
    public class CustomStockValidatorTests
    {
        [Fact]
        public void Validate_ReturnsNoErrors_ForValidDto()
        {
            var dto = new CreateStockDto
            {
                Symbol = "TEST",
                CompanyName = "Test Company",
                Purchase = 100,
                LastDiv = 1,
                Industry = "Tech",
                MarketCap = 1000000
            };

            var errors = CustomStockValidator.Validate(dto);

            Assert.Empty(errors);
        }

        [Fact]
        public void Validate_ReturnsError_WhenSymbolIsEmpty()
        {
            var dto = new CreateStockDto
            {
                Symbol = "",
                CompanyName = "Test Company",
                Purchase = 100,
                LastDiv = 1,
                Industry = "Tech",
                MarketCap = 1000000
            };

            var errors = CustomStockValidator.Validate(dto);

            Assert.Contains("The Symbol is required.", errors);
        }

        [Fact]
        public void Validate_ReturnsError_WhenCompanyNameIsTooLong()
        {
            var dto = new CreateStockDto
            {
                Symbol = "TEST",
                CompanyName = new string('A', 101),
                Purchase = 100,
                LastDiv = 1,
                Industry = "Tech",
                MarketCap = 1000000
            };

            var errors = CustomStockValidator.Validate(dto);

            Assert.Contains("The Company Name must not exceed 100 characters.", errors);
        }

        [Fact]
        public void Validate_ReturnsMultipleErrors_ForInvalidDto()
        {
            var dto = new CreateStockDto
            {
                Symbol = "",
                CompanyName = "",
                Purchase = -1,
                LastDiv = -5,
                Industry = "",
                MarketCap = 0
            };

            var errors = CustomStockValidator.Validate(dto);

            Assert.Contains("The Symbol is required.", errors);
            Assert.Contains("The Company Name is required.", errors);
            Assert.Contains("The Purchase value must be greater than 0.", errors);
            Assert.Contains("The Last Dividend must be 0 or greater.", errors);
            Assert.Contains("The Industry is required.", errors);
            Assert.Contains("The Market Cap must be greater than 0.", errors);
        }
    }
}
