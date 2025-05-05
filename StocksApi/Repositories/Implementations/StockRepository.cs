using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using StocksApi.Data;
using StocksApi.Dtos.Stocks;
using StocksApi.Models;
using StocksApi.Repositories.Interfaces;

namespace StocksApi.Repositories.Implementations
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock> DeleteAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
                return null;
            
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.Include(c=> c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            var stock = await _context.Stocks.Include(c=> c.Comments).FirstOrDefaultAsync(i=> i.Id == id);
            if (stock == null)
                return null;
            return stock;
        }

        public async Task<bool> StockExists(int id)
        {
            return await _context.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(Stock stock)
        {
            var existingStock = await _context.Stocks.FindAsync(stock.Id);
            if (existingStock == null)
                return null;

            _context.Entry(existingStock).CurrentValues.SetValues(stock);
            await _context.SaveChangesAsync();
            return existingStock;
        }
    }
}
