using InvestmentSimulator.DBFiles;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThreeFourteen.Finnhub.Client;
using ThreeFourteen.Finnhub.Client.Model;

namespace InvestmentSimulator.Connector
{
    class ExchangeConnector
    {
        private readonly FinnhubClient _finnhubClient;
        private readonly StockContext _dbContext;
        internal ExchangeConnector(FinnhubClient finnhubClient, StockContext dbContext)
        {
            _finnhubClient = finnhubClient;
            _dbContext = dbContext;
        }

        public async Task GetExchanges()
        {
            StockExchange[] exchanges = await _finnhubClient.Stock.GetExchanges();

            foreach (var exchange in exchanges)
            {
                var query = _dbContext.Exchanges
                        .Where(b => b.Code == exchange.Code);

                if (!(query.Count() > 0))
                {
                    _dbContext.Add(new Exchange { Code = exchange.Code, Name = exchange.Name });
                }
                else
                {
                    Log.Information($"Duplicate Record, Table[Exchange]: {exchange.Code} already exists in db");
                }
            }
            _dbContext.SaveChanges();
        }
    }
}
