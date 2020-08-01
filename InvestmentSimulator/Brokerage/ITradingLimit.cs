using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentSimulator.Brokerage
{
    interface ITradingLimit
    {
        public decimal Limit { get; set; }
        public TradingRate TradingRate { get; set; }
    }
}
