using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentSimulator.DBFiles
{
    public static class OrderTypeExtensions
    {
        public static string GetFieldValue(this OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.Buy: return "Buy";
                case OrderType.Sell: return "Sell";
                case OrderType.None: return "None";
                default:
                    break;
            }
            throw new InvalidOperationException("Not a valid order type");
        }
    }
}
