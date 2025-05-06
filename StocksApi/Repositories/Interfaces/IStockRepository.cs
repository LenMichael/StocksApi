using StocksApi.Dtos.Stocks;
using StocksApi.Filters;
using StocksApi.Models;

namespace StocksApi.Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(Stock stock);
        Task<Stock> DeleteAsync(int id);
        Task<bool> StockExists(int id);
    }
}
