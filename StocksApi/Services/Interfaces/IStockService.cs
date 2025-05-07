using StocksApi.Dtos.Stocks;
using StocksApi.Filters;
using StocksApi.Models;

namespace StocksApi.Services.Interfaces
{
    public interface IStockService
    {
        Task<List<Stock>> GetAllAsync(QueryObject query, CancellationToken cancellationToken);
        Task<Stock?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Stock?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken);
        Task<Stock> CreateAsync(CreateStockDto stockDto, CancellationToken cancellationToken);
        Task<Stock?> UpdateAsync(int id, UpdateStockDto stockDto, CancellationToken cancellationToken);
        Task<Stock?> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> StockExists(int id);
    }
}
