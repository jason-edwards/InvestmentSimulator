using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace InvestmentSimulator.Brokerage
{
    public class TradingRateInteractions
    {
        public static TradingRate TradingRateFromString (string tradingRate)
        {
            switch (tradingRate)
            {
                case "Daily": return TradingRate.Daily;
                case "Weekly": return TradingRate.Weekly;
                case "Monthly": return TradingRate.Monthly;
                default:
                    break;
            }
            throw new InvalidOperationException("Not a valid trading rate");
        }
    }
}
