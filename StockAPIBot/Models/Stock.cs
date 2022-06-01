namespace StockAPIBot.Models;

public class Stock
{
    public string Symbol { get; set; }
    public DateTime Date { get; set; }
    public float Open { get; set; }
    public float High { get; set; }
    public float Low { get; set; }
    public float Close { get; set; }
    public float Volume { get; set; }


}
