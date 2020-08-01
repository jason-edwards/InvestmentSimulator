using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvestmentSimulator.DBFiles;

namespace InvestmentSimulator.Simulation.Strategies
{
    class ModifiedBollingerBandsDirectional : IStrategy
    {
        public Task BuildStrategy()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable> GetBuyIndicators()
        {
            throw new NotImplementedException();
        }

        public Task<OrderType> GetIndicatorAtTimeForSymbol(string symbol, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Task<List<(string, OrderType)>> GetIndicatorsAtTime(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable> GetSellInidicators()
        {
            throw new NotImplementedException();
        }
    }
}
