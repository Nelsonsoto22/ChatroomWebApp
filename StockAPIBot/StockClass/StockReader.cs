

using CsvHelper;
using CsvHelper.Configuration;
using StockAPIBot.Models;
using System.Globalization;
using System.Net.Http.Headers;

namespace StockAPIBot.StockClass;

public interface IStockReader
{
    Task<string> getStock(string stock_code);
}

public class StockReader : IStockReader
{


    public async Task<string> getStock(string stock_code)
    {
        var stockURL = $"https://stooq.com/q/l/?s={stock_code}&f=sd2t2ohlcv&h&e=csv";

        var _client = new HttpClient();
        List<Stock> lsStock = new List<Stock>();

        using (var msg = new HttpRequestMessage(HttpMethod.Get, new Uri(stockURL)))
        {
            msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/csv"));
            using (var resp = await _client.SendAsync(msg))
            {
                try
                {

                    if (resp.IsSuccessStatusCode)
                    {
                        resp.EnsureSuccessStatusCode();

                        using (var s = await resp.Content.ReadAsStreamAsync())
                        using (var sr = new StreamReader(s))
                        using (var futureoptionsreader = new CsvReader(sr, CultureInfo.CurrentCulture))
                        {

                            //futureoptionsreader.Configuration.RegisterClassMap<MappingNSEIndexes>();
                            lsStock = futureoptionsreader.GetRecords<Stock>().ToList();
                        }
                    }
                    else
                    {
                        return $"Error reading {stock_code} quote";
                    }

                }
                catch (Exception)
                {
                    return $"Error reading {stock_code} quote";
                }
            }
        }

        if (lsStock.Count > 0)
        {
            return $"{stock_code} quote is ${lsStock.First().Close.ToString("00.00")} per share";
        }
        else
        {
            return $"Invalid {stock_code}";
        }
    }
}
