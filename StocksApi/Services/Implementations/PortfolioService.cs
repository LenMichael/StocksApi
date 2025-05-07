using StocksApi.Models;
using StocksApi.Repositories.Interfaces;
using StocksApi.Services.Interfaces;

namespace StocksApi.Services.Implementations
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioService(IPortfolioRepository portfolioRepo)
        {
            _portfolioRepo = portfolioRepo;
        }
        public async Task<Portfolio> CreateAsync(Portfolio portfolio, CancellationToken cancellationToken)
        {
            var createdPortfolio = await _portfolioRepo.CreateAsync(portfolio, cancellationToken);
            return createdPortfolio;
        }

        public async Task<Portfolio> DeleteAsync(User user, string symbol, CancellationToken cancellationToken)
        {
            var portfolio = await _portfolioRepo.DeleteAsync(user, symbol, cancellationToken);
            if (portfolio == null)
                return null;
            //throw new Exception("Portfolio not found");
            return portfolio;
        }

        public async Task<List<Stock>> GetUserPortfolio(User user, CancellationToken cancellationToken)
        {
            return await _portfolioRepo.GetUserPortfolio(user, cancellationToken);
        }
    }
}
