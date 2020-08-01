using CsvHelper.Configuration.Attributes;
using System;

namespace InvestmentSimulator.Connector.FileConnector
{
    public class WatchlistFile
    {
        [Index(0)]
        public string Symbol { get; set; }

        [Index(1)]
        public string Exchange { get; set; }
    }
}
