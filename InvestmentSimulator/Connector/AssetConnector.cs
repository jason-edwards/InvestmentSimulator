using InvestmentSimulator.DBFiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThreeFourteen.Finnhub.Client;
using ThreeFourteen.Finnhub.Client.Model;

namespace InvestmentSimulator.Connector
{
    class AssetConnector
    {
        private readonly FinnhubClient _finnhubClient;
        private readonly AssetContext _db;
        internal AssetConnector(FinnhubClient finnhubClient, AssetContext db)
        {
            _finnhubClient = finnhubClient;
            _db = db;
        }

        public async Task GetExchanges()
        {
            StockExchange[] exchanges = await _finnhubClient.Stock.GetExchanges();

            foreach (var exchange in exchanges)
            {
                _db.Add(new Exchange { Code = exchange.Code, Name = exchange.Name });
            }
            _db.SaveChanges();

        }


    }
}
