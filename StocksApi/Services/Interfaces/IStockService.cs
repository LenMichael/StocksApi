using StocksApi.Dtos.Stocks;
using StocksApi.Models;

namespace StocksApi.Services.Interfaces
{
    public interface IStockService
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(CreateStockDto stockDto);
        Task<Stock?> UpdateAsync(int id, UpdateStockDto stockDto);
        Task<Stock> DeleteAsync(int id);
        Task<bool> StockExists(int id);
    }
}
