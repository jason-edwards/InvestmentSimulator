using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentSimulator.Simulation
{
    public class OrderSize
    {
        public decimal OrderSizeCurrency { get; set; }
        public decimal OrderSizePercentage { get; set; }
        public decimal CurrencyCutoverToPercentageAmount { get; set;  }

    }
}
