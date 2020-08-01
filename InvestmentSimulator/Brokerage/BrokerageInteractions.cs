using System;

namespace InvestmentSimulator.Brokerage
{
    public class BrokerageInteractions
    {
        public static IBrokerage BrokerageFromString (string brokerageName)
        {
            switch (brokerageName)
            {
                case "CommSec": return new CommSec();
                default:
                    break;
            }
            throw new InvalidOperationException("Not a valid order type");
        }

        public static string BrokerageTypeToString (IBrokerage brokerage)
        {
            return brokerage.GetType().ToString();
        }

    }
}
