using InvestmentSimulator.DBFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThreeFourteen.Finnhub.Client.Model;

namespace InvestmentSimulator.Simulation.Strategies
{
    interface IStrategy
    {
        public Task BuildStrategy();

        public Task<IQueryable> GetBuyIndicators();

        public Task<IQueryable> GetSellInidicators();

        public Task<OrderType> GetIndicatorAtTimeForSymbol(string symbol, DateTime dateTime);

        public Task<List<(string, OrderType)>> GetIndicatorsAtTime(DateTime dateTime);

    }
}
