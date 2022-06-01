using StockAPIBot.Redis;
using StockAPIBot.StockClass;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockAPIBot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockBotController : ControllerBase
{
    private IStockReader _stockReader;
    private IRedisManager _redisManager;

    public StockBotController(IStockReader stockReader, IRedisManager redisManager)
    {
        _stockReader = stockReader;
        _redisManager = redisManager;
    }

    
    // GET api/<StockBotController>/5
    [HttpGet("{stock_code}")]
    public async Task<IActionResult> Get(string stock_code)
    {
        string result = await _stockReader.getStock(stock_code);

        bool messagesent = await _redisManager.sendMessage("stockChannel", result);

        if(!messagesent)
            return StatusCode(500, "Redis Server Not Found");
        else if (result.Contains("Error"))
            return BadRequest(result);
        else 
            return Ok(result);
    }

}
