using Microsoft.EntityFrameworkCore;
using StocksApi.Data;
using StocksApi.Models;
using StocksApi.Repositories.Interfaces;

namespace StocksApi.Repositories.Implementations
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio, CancellationToken cancellationToken)
        {
            await _context.Portfolios.AddAsync(portfolio, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return portfolio;
        }

        public async Task<Portfolio> DeleteAsync(User user, string symbol, CancellationToken cancellationToken)
        {
            var portfolioModel = await _context.Portfolios
                .Include(p => p.Stock)
                .FirstOrDefaultAsync(p => p.UserId == user.Id && p.Stock.Symbol.ToLower() == symbol.ToLower(), cancellationToken);
            if (portfolioModel == null)
                return null;

            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync(cancellationToken);
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(User user, CancellationToken cancellationToken)
        {
            return await _context.Portfolios
                .Where(p => p.UserId == user.Id)
                .Select(stock => new Stock
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap
                }).ToListAsync(cancellationToken);
        }
    }
}
