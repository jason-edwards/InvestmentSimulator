using System;
using System.Threading.Tasks;
using InvestmentSimulator.Finnhub.Client.Model;
using InvestmentSimulator.Finnhub.Client.Serialisation;

namespace InvestmentSimulator.Finnhub.Client
{
	public class StockClient
	{
        private readonly FinnhubClient _finnhubClient;

        internal StockClient(FinnhubClient finnhubClient)
        {
            _finnhubClient = finnhubClient;
        }

        public Task<Company> GetCompany(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol)) throw new ArgumentException(nameof(symbol));

            return _finnhubClient.SendAsync<Company>("stock/profile", JsonDeserialiser.Default,
                new Field(FieldKeys.Symbol, symbol));

        }

    } 
}
