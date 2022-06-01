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
    // Get api/<StockBotController>/5
    [HttpGet("{stock_code}")]
    public async Task<IActionResult<string>> Get(string stock_code)
    {

        string result = await _stockReader.getStock(stock_code);

        bool messagesent = await _redisManager.sendMessage("stockChannel", result);


        return (IActionResult<string>)Ok(result);
    }

    // Post api/<StockBotController>/5
    [HttpPost("{stock_code}")]
    public IActionResult Post(string stock_code)
    {
        Task.Run(() =>
        {

            string result = _stockReader.getStock(stock_code).Result;

            bool messagesent = _redisManager.sendMessage("stockChannel", result).Result;

        });

        return Ok();
    }

}
