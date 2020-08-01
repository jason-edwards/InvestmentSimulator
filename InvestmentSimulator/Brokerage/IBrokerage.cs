using InvestmentSimulator.Simulation;

namespace InvestmentSimulator.Brokerage
{
    /// <summary>
    /// Assumes same currency as base exchange.
    /// </summary>
    public interface IBrokerage
    {
        public decimal Fee { get; set; }
        public FeeType FeeType { get; set; }
        public TradingLimits TradingLimits { get; set; }
        public decimal FeeAsCurrency(decimal transactionAmount);
        public decimal FeeAsPercentage(decimal transactionAmount);

    }
}
