using StocksApi.Models;

namespace StocksApi.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<List<Stock>> GetUserPortfolio(User user, CancellationToken cancellationToken);
        Task<Portfolio> CreateAsync(Portfolio portfolio, CancellationToken cancellationToken);
        Task<Portfolio> DeleteAsync(User user, string symbol, CancellationToken cancellationToken);
    }
}
