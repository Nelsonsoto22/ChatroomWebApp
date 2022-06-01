using Microsoft.AspNetCore.Mvc;
using StockAPIBot.StockClass;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockAPIBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockBotController : ControllerBase
    {
        private IStockReader _stockReader;

        public StockBotController(IStockReader stockReader)
        {
            _stockReader = stockReader;
        }

        
        // GET api/<StockBotController>/5
        [HttpGet("{stock_code}")]
        public async Task<ActionResult<string>> Get(string stock_code)
        {
            string result = await _stockReader.getStock(stock_code);
            if (result.Contains("Error"))
                return BadRequest(result);
            return Ok(result);
        }

    }
}
