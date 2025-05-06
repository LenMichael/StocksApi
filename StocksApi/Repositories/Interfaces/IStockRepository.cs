using StocksApi.Dtos.Stocks;
using StocksApi.Filters;
using StocksApi.Models;

namespace StocksApi.Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query, CancellationToken cancellationToken);
        Task<Stock?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Stock> CreateAsync(Stock stock, CancellationToken cancellationToken);
        Task<Stock?> UpdateAsync(Stock stock, CancellationToken cancellationToken);
        Task<Stock> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> StockExists(int id);
    }
}
