using InvestmentSimulator.Simulation;

namespace InvestmentSimulator.Brokerage
{
    public class CommSec : Brokerage
    {
        internal void CommsSec()
        {
            Fee = 19.950M;
            FeeType = FeeType.Currency;
            TradingLimits.DepositLimit.Limit = 0;
            TradingLimits.DepositLimit.TradingRate = TradingRate.Daily;
            TradingLimits.WithdrawalLimit.Limit = 0;
            TradingLimits.WithdrawalLimit.TradingRate = TradingRate.Daily;
        }
    }
}
