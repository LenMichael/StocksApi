using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksApi.Extensions;
using StocksApi.Models;
using StocksApi.Repositories.Interfaces;
using StocksApi.Services.Interfaces;

namespace StocksApi.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IStockService _stockService;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<User> userManager, IStockService stockService, IPortfolioRepository portfolioRepository, IStockRepository stockRepo)
        {
            _userManager = userManager;
            _stockService = stockService;
            _portfolioRepo = portfolioRepository;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPortfolio(CancellationToken cancellationToken)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser, cancellationToken);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortofolio(string symbol, CancellationToken cancellationToken)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol, cancellationToken);

            if (stock == null) return NotFound("Stock not found");

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser, cancellationToken);

            if (userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower())) 
                return BadRequest("Stock already in portfolio");

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                UserId = appUser.Id,
            };

            await _portfolioRepo.CreateAsync(portfolioModel, cancellationToken);

            if (portfolioModel == null)
                return BadRequest("Error adding stock to portfolio");
            else
                return Created();
                //return CreatedAtAction(nameof(GetPortfolio), new { id = portfolioModel.Id }, portfolioModel);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol, CancellationToken cancellationToken)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser, CancellationToken.None);
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count == 1)
                await _portfolioRepo.DeleteAsync(appUser, symbol, cancellationToken);
            else
                return BadRequest("Stock not in portfolio");

            return Ok();
        }
    }
}
