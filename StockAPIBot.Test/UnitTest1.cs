using StockAPIBot.StockClass;

namespace StockAPIBot.Test;

public class UnitTest1
{
    [Fact]
    public async void stockReader_ReturnValue()
    {
        StockReader stockReader = new StockReader();
        string stock_code = "eurusd";
        string val = await stockReader.getStock(stock_code);
        Assert.StartsWith(stock_code, val);
    }

    [Fact]
    public async void stockReader_ReturnError()
    {
        StockReader stockReader = new StockReader();
        string stock_code = "invalid";
        string val = await stockReader.getStock(stock_code);
        Assert.StartsWith("Error", val);
    }
}