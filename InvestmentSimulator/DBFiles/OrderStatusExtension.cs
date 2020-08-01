using System;

namespace InvestmentSimulator.DBFiles
{
    public static class OrderStatusExtensions
    {
        public static string GetFieldValue(this OrderStatus orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatus.Closed: return "Closed";
                case OrderStatus.Open: return "Open";
                default:
                    break;
                
            }
            throw new InvalidOperationException("Not a valid order status");
        }
    }
}
