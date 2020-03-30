using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ThreeFourteen.Finnhub.Client;

namespace InvestmentSimulator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = CreateConfiguration();

            string apiKey = File.ReadAllText(config["finnhubkeypath"]);
            
            var client = new FinnhubClient(apiKey);
            string symbol = "AAPL";
            var company = await client.Stock.GetCompany(symbol);
            

            Console.WriteLine($"Company: {symbol},{Environment.NewLine}Description: {company.Description}");
        }

        static IConfigurationRoot CreateConfiguration()
        {
            string path = Path.GetFullPath("../../../../");
            Console.WriteLine(path);

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
