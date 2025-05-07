using StocksApi.Data;
using StocksApi.Dtos.Stocks;
using StocksApi.Filters;
using StocksApi.Mappers;
using StocksApi.Models;
using StocksApi.Repositories.Interfaces;
using StocksApi.Services.Interfaces;

namespace StocksApi.Services.Implementations
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepo;
        public StockService(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }
        public async Task<Stock> CreateAsync(CreateStockDto stockDto, CancellationToken cancellationToken)
        {
            var stock = stockDto.ToStockFromCreateDto();
            return await _stockRepo.CreateAsync(stock, cancellationToken);
        }

        public async Task<Stock?> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (!await _stockRepo.StockExists(id))
                return null;

            return await _stockRepo.DeleteAsync(id, cancellationToken);
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query, CancellationToken cancellationToken)
        {
            return await _stockRepo.GetAllAsync(query, cancellationToken);
        }

        public async Task<Stock?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _stockRepo.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken)
        {
            return await _stockRepo.GetBySymbolAsync(symbol, cancellationToken);
        }

        public async Task<bool> StockExists(int id)
        {
            return await _stockRepo.StockExists(id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockDto stockDto, CancellationToken cancellationToken)
        {
            var existingStock = await _stockRepo.GetByIdAsync(id, cancellationToken);
            if (existingStock == null)
                return null;

            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;

            return await _stockRepo.UpdateAsync(existingStock, cancellationToken);
        }
    }
}
