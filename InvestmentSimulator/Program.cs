using System.IO;
using System.Threading.Tasks;
using InvestmentSimulator.DBFiles;
using InvestmentSimulator.Connector;
using Microsoft.Extensions.Configuration;
using Serilog;
using ThreeFourteen.Finnhub.Client;
using ThreeFourteen.Finnhub.Client.Model;
using System.Linq;
using System;
using System.Collections.Generic;
using InvestmentSimulator.Connector.FileConnector;

namespace InvestmentSimulator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = CreateConfiguration();
            OpenLogger(config);

            string apiKey = File.ReadAllText(config["finnhubkeypath"]);
            var client = new FinnhubClient(apiKey);

            var dbContext = new StockContext();

            var exchangeConnector = new ExchangeConnector(client, dbContext);
            //await exchangeConnector.GetExchanges();

            var stockConnector = new StockConnector(client, dbContext);
            //await stockConnector.GetSymbols("AX");

            var fileConnector = new FileConnector(config["filepath"] + "watchlist.csv", "Watchlist", dbContext);

            fileConnector.Import();



            CloseLogger();
        }

        static ILogger OpenLogger(IConfiguration config)
        {
            Log.Information("Logger Oppening.");
            return Log.Logger = new LoggerConfiguration()
                .WriteTo.SQLite(config["logdbpath"])
                .CreateLogger();
        }

        static void CloseLogger()
        {
            Log.Information("Logger Closing.");
            Log.CloseAndFlush();
        }

        static IConfigurationRoot CreateConfiguration()
        {
            //string path = Path.GetFullPath("../../../../");
            string path = Path.GetFullPath("../");
            //string path = Directory.GetCurrentDirectory();

            return new ConfigurationBuilder()
                //.AddInMemoryCollection()
                //.AddEnvironmentVariables()
                .SetBasePath(path)
                .AddJsonFile("settings.json", false)
                //.AddCommandLine(args)
                .Build();
        }

        private async void SeedDividends(StockConnector stockConnector, StockContext dbContext)
        {

            var dividends = dbContext.Dividends
                            .Where(b => b.Yield == 0 && b.Date.Year > 1998)
                            .OrderBy(c => c.Date);

            foreach (var dividend in dividends)
            {
                var priceQuery = dbContext.StockPrices
                    .Where(b => b.AssetFK == dividend.AssetFK && b.TimeStamp.Date == dividend.Date.Date && b.Adjusted == false);

                string symbol = dbContext.StockProperties
                    .Where(b => b.AssetId == dividend.AssetFK)
                    .Select(c => c.DisplaySymbol).First();
                Console.WriteLine(symbol);

                if (priceQuery.Count() == 0)
                {
                    await stockConnector.GetCandles(symbol, Resolution.Day, dividend.Date.AddMonths(-12), DateTime.Now, false);
                }

                priceQuery = dbContext.StockPrices
                    .Where(b => b.AssetFK == dividend.AssetFK && b.TimeStamp.Date == dividend.Date.Date && b.Adjusted == false);

                if (priceQuery.Count() == 0)
                {
                    priceQuery = dbContext.StockPrices
                        .Where(b => b.AssetFK == dividend.AssetFK
                        && b.Adjusted == false
                        && b.TimeStamp.Date.Month == (dividend.Date.AddMonths(-1).Month)
                        && b.TimeStamp.Date.AddMonths(1).Year == dividend.Date.Year);
                }

                decimal price;
                if (priceQuery.Count() != 0)
                {
                    price = priceQuery.First().Amount;
                    dividend.Yield = dividend.Amount / price;
                }
            }

            dbContext.SaveChanges();

            var records = dbContext.Dividends
                .Select(b => b.AssetFK)
                .Distinct();

            foreach (var record in records)
            {
                string symbol = dbContext.StockProperties
                    .Where(b => b.AssetId == record)
                    .FirstOrDefault().DisplaySymbol;

                DateTime minDividendDate = dbContext.Dividends
                    .Where(b => b.AssetFK == record)
                    .OrderBy(c => c.Date)
                    .First().Date;

                dividends = dbContext.Dividends
                    .Where(b => b.AssetFK == record)
                    .OrderBy(c => c.Date);

                decimal yeildAvg = 0;
                decimal yeildSum = 0;
                int yeildCount = 0;
                foreach (var dividend in dividends)
                {
                    yeildCount++;
                    yeildSum += dividend.Yield;
                }
                yeildAvg = yeildSum / yeildCount;

                int dividendCount = dbContext.Dividends
                    .Where(b => b.AssetFK == record)
                    .Count();

                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span = DateTime.Now - minDividendDate;

                float years = (zeroTime + span).Year;

                if (years > 0 && dividendCount / years > 1)
                {
                    Console.WriteLine($"{symbol} has had {dividendCount} dividends in {years} years at a ratio of {dividendCount / years} with an average yeild of {yeildAvg}");
                }
            }
        }
    }
}
