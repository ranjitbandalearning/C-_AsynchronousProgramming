using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using StockAnalyzer.Core.Domain;

namespace StockAnalyzer.Core
{
    public class DataStoreSync
    {
        public Dictionary<string, Company> Companies = new Dictionary<string, Company>();
        public static Dictionary<string, IEnumerable<StockPrice>> Stocks 
            = new Dictionary<string, IEnumerable<StockPrice>>();

        private string basePath { get; }

        public DataStoreSync(string basePath)
        {
            this.basePath = basePath;
        }

        public Dictionary<string, IEnumerable<StockPrice>> LoadStocks()
        {
            // THE BELOW LINE IS COMMENTED TO SEE TO THAT EVERY TIME THE FILE GETS READ
            //if (Stocks.Any()) return Stocks;

            var prices = GetStockPrices();

            Stocks = prices
                .GroupBy(x => x.Ticker)
                .ToDictionary(x => x.Key, x => x.AsEnumerable());

            return Stocks;
        }

        private IList<StockPrice> GetStockPrices()
        {
            //var lines = File.ReadAllLines(@"StockPrices_Small.csv");
            var lines = File.ReadAllLines(@"StockPrices_Big.csv");

            var data = new List<StockPrice>();

            foreach (var line in lines.Skip(1))
            {
                var segments = line.Split(',');

                for (var i = 0; i < segments.Length; i++) segments[i] = segments[i].Trim('\'', '"');
                var price = new StockPrice
                {
                    Ticker = segments[0],
                    TradeDate = DateTime.ParseExact(segments[1], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                    Volume = Convert.ToInt32(segments[6], CultureInfo.InvariantCulture),
                    Change = Convert.ToDecimal(segments[7], CultureInfo.InvariantCulture),
                    ChangePercent = Convert.ToDecimal(segments[8], CultureInfo.InvariantCulture),
                };
                data.Add(price);
            }

            return data;
        }
    }
}
