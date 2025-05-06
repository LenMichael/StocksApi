using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksApi.Data;
using StocksApi.Dtos.Stocks;
using StocksApi.Filters;
using StocksApi.Mappers;
using StocksApi.Repositories.Interfaces;
using StocksApi.Services.Interfaces;

namespace StocksApi.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        public StockController( IStockService stockService )
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query, CancellationToken cancellationToken)
        {
            var stocks = await _stockService.GetAllAsync(query, cancellationToken);
            var stockDtos = stocks.Select(x => x.ToStockDto());
            return Ok(stockDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var stock = await _stockService.GetByIdAsync(id, cancellationToken);
            if (stock == null)
                return NotFound();
         
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDto stockDto, CancellationToken cancellationToken)
        {
            if (stockDto == null)
                return BadRequest();
            var stock = stockDto.ToStockFromCreateDto();
            await _stockService.CreateAsync(stockDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto stockDto, CancellationToken cancellationToken)
        {
            if (stockDto == null)
                return BadRequest();

            var stock = await _stockService.UpdateAsync(id, stockDto, cancellationToken);
            if (stock == null)
                return NotFound();
         
            return Ok(stock.ToStockDto());
            //return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var stock = await _stockService.DeleteAsync(id, cancellationToken);
            if (stock == null)
                return NotFound();

            return NoContent();
        }
    }
}
