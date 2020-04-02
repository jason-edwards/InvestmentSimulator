using System;
using System.IO;
using System.Threading.Tasks;
using InvestmentSimulator.DBFiles;
using InvestmentSimulator.Connector;
using Microsoft.Extensions.Configuration;
using Serilog;
using ThreeFourteen.Finnhub.Client;


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

            var db = new AssetContext();

            var exchangeInterface = new AssetConnector(client, db);
            await exchangeInterface.GetExchanges();



            string symbol = "AAPL";
            var company = await client.Stock.GetCompany(symbol);
            

            Console.WriteLine($"Company: {symbol},{Environment.NewLine}Description: {company.Description}");
             
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
