using StocksApi.Models;

namespace StocksApi.Repositories.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(User user, CancellationToken cancellationToken);
        Task<Portfolio> CreateAsync(Portfolio portfolio, CancellationToken cancellationToken);
        Task<Portfolio> DeleteAsync(User user, string symbol, CancellationToken cancellationToken);
    }
}
