using InvestmentSimulator.Brokerage;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentSimulator.Simulation
{
    public class BaseTradeConditions : ITradeConditions
    {
        public decimal InitialCapital { get; set; }
        public IBrokerage Brokerage { get; set; }
        public OrderSize OrderSize { get; set; }
        public int MaxContractHeld { get; set; }
        public (int, TradingRate) BuyCooldown { get; set; }

        public OrderType CheckConditions()
        {
            throw new NotImplementedException();
        }

        public void LoadConditionsFromFile(string filePath)
        {
            
        }
    }
}
