using CsvHelper.Configuration.Attributes;
using System;

namespace InvestmentSimulator.Connector.FileConnector
{
    public class SplitFile
    {
        [Index(0)]
        public string Symbol { get; set; }

        [Index(1)]
        public int Year { get; set; }

        [Index(2)]
        public int Month { get; set; }

        [Index(3)]
        public int Day { get; set; }

        [Index(4)]
        public decimal ToFactor { get; set; }

        [Index(5)]
        public decimal FromFactor { get; set; }

    }
}
