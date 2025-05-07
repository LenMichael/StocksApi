using Microsoft.IdentityModel.Tokens;
using StocksApi.Dtos.Stocks;
using System.Collections.Generic;

namespace StocksApi.Validators.Stocks
{
    public static class CustomStockValidator
    {
        public static List<string> Validate(CreateStockDto stockDto)
        {
            var errors = new List<string>();

            if (stockDto.Symbol.IsNullOrEmpty())
                errors.Add("The Symbol is required.");

            if (stockDto.Symbol.Length > 10)
                errors.Add("The Symbol must not exceed 10 characters.");

            if (stockDto.CompanyName.IsNullOrEmpty())
                errors.Add("The Company Name is required.");

            if (stockDto.CompanyName.Length > 100)
                errors.Add("The Company Name must not exceed 100 characters.");

            if (stockDto.Purchase <= 0)
                errors.Add("The Purchase value must be greater than 0.");

            if (stockDto.LastDiv < 0)
                errors.Add("The Last Dividend must be 0 or greater.");

            if (stockDto.Industry.IsNullOrEmpty())
                errors.Add("The Industry is required.");

            if (stockDto.MarketCap <= 0)
                errors.Add("The Market Cap must be greater than 0.");

            return errors;
        }
    }
}
