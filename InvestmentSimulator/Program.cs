using System;
using System.IO;
using System.Threading.Tasks;
using InvestmentSimulator.DBFiles;
using InvestmentSimulator.Connector;
using Microsoft.Extensions.Configuration;
using Serilog;
using ThreeFourteen.Finnhub.Client;
using ThreeFourteen.Finnhub.Client.Model;
using ThreeFourteen.Finnhub.Client.Limits;

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
            await exchangeConnector.GetExchanges();

            var stockConnector = new StockConnector(client, dbContext);
            await stockConnector.GetSymbols("AX");

            await stockConnector.GetCandles("CBA.AX", Resolution.OneMinute);
            await stockConnector.GetCandles("CBA.AX", Resolution.OneMinute, null, null, true);
            await stockConnector.GetCandles("CBA.AX", Resolution.FiveMinutes);
            await stockConnector.GetCandles("CBA.AX", Resolution.FiveMinutes, null, null, true);
            await stockConnector.GetCandles("CBA.AX", Resolution.FifteenMinutes);
            await stockConnector.GetCandles("CBA.AX", Resolution.FifteenMinutes, null, null, true);
            await stockConnector.GetCandles("CBA.AX", Resolution.ThirtyMinutes);
            await stockConnector.GetCandles("CBA.AX", Resolution.ThirtyMinutes, null, null, true);
            await stockConnector.GetCandles("CBA.AX", Resolution.OneHour);
            await stockConnector.GetCandles("CBA.AX", Resolution.OneHour, null, null, true);
            await stockConnector.GetCandles("CBA.AX", Resolution.Day);
            await stockConnector.GetCandles("CBA.AX", Resolution.Day, null, null, true);

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
    }
}
