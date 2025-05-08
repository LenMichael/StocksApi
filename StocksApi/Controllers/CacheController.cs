using Microsoft.AspNetCore.Mvc;
using StocksApi.Services.Implementations;

[Route("api/cache")]
[ApiController]
public class CacheController : ControllerBase
{
    private readonly RedisCacheService _cacheService;

    public CacheController(RedisCacheService cacheService)
    {
        _cacheService = cacheService;
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> Get(string key)
    {
        var value = await _cacheService.GetAsync(key);

        if (value == null)
        {
            return NotFound();
        }
        return Ok(value);
    }

    [HttpPost]
    public async Task<IActionResult> Set([FromBody] CacheItem item)
    {
        Console.WriteLine($"ITEM.KEY: {item.Key}, ITEM.VALUE: {item.Value}");
        await _cacheService.SetAsync(item.Key, item.Value, TimeSpan.FromMinutes(10));
        return Ok();
    }
}

public class CacheItem
{
    public string Key { get; set; }
    public string Value { get; set; }
}
