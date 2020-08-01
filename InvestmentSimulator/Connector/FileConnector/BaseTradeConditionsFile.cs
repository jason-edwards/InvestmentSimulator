using CsvHelper.Configuration.Attributes;
using System;

namespace InvestmentSimulator.Connector.FileConnector
{
    public class BaseTradeConditionsFile
    {
        [Index(0)]
        public decimal InitialCapital { get; set; }

        [Index(1)]
        public string Brokerage { get; set; }

        [Index(2)]
        public decimal OrderSizeCurrency { get; set; }

        [Index(3)]
        public decimal OrderSizePercentage { get; set; }

        [Index(4)]
        public decimal CurrencyCutoverToPercentageAmount { get; set; }

        [Index(5)]
        public int MaxContractsHeld { get; set; }

        [Index(6)]
        public int BuyCooldown { get; set; }

        [Index(7)]
        public string BuyCooldownRate { get; set; }
    }
}
