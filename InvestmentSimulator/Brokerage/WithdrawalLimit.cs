using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentSimulator.Brokerage
{
    public class WithdrawalLimit : ITradingLimit
    {
        public decimal Limit { get; set; }
        public TradingRate TradingRate { get; set; }
    }
}
