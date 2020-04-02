using System;
using System.IO;
using System.Threading.Tasks;
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
            
            Log.Information("This informational message will be written to SQLite database");
            

            string apiKey = File.ReadAllText(config["finnhubkeypath"]);
            var client = new FinnhubClient(apiKey);
            

            string symbol = "AAPL";
            var company = await client.Stock.GetCompany(symbol);
            

            Console.WriteLine($"Company: {symbol},{Environment.NewLine}Description: {company.Description}");
             
            CloseLogger();
        }

        static ILogger OpenLogger(IConfiguration config)
        {
            return Log.Logger = new LoggerConfiguration()
                .WriteTo.SQLite(config["logdbpath"])
                .CreateLogger();
        }

        static void CloseLogger()
        {
            Log.CloseAndFlush();
        }

        static IConfigurationRoot CreateConfiguration()
        {
            string path = Path.GetFullPath("../../../../");

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
