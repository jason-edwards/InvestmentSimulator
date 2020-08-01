using System.Transactions;

namespace InvestmentSimulator.Brokerage
{
    public class DepositLimit : ITradingLimit
    {
        public decimal Limit { get; set; }
        public TradingRate TradingRate {get; set; }
    }
}
