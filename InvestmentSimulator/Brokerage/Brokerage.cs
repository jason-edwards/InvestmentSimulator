using InvestmentSimulator.Simulation;
using System;

namespace InvestmentSimulator.Brokerage
{
    public class Brokerage : IBrokerage
    {
        public decimal Fee { get ; set; }
        public FeeType FeeType { get; set; }
        public TradingLimits TradingLimits { get; set; }

        public decimal FeeAsCurrency(decimal transactionAmount)
        {
            return FeeType switch
            {
                FeeType.Currency => Fee,
                FeeType.Percentage => Fee * transactionAmount,
                _ => throw new Exception(),
            };
        }

        public decimal FeeAsPercentage(decimal transactionAmount)
        {
            return FeeType switch
            {
                FeeType.Currency => Fee / transactionAmount,
                FeeType.Percentage => Fee,
                _ => throw new Exception(),
            };
        }
    }
}
