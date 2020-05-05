using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentSimulator.DBFiles
{
    public class StockProperty
    {
        /// Primary key for table - maps with unique Symbol + Echange
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AssetId { get; set; }

        /// Symbol of the company
        [MaxLength(8), Required]
        public string Symbol { get; set; }

        /// ExchangeId
        public long ExchangeFK { get; set; }

        /// Currency used in company filings.
        [MaxLength(3)]
        public string Currency { get; set; }

        ///CUSIP number
        [MaxLength(9)]
        public string Cusip { get; set; }

        /// Sedol Number
        [MaxLength(7)]
        public string Sedol { get; set; }

        /// ISIN number
        [MaxLength(12)]
        public string Isin { get; set; }

        /// Company symbol/ticker as used on the listed exchange
        [MaxLength(8)]
        public string Ticker { get; set; }

        /// IPO date
        public DateTime IpoDate { get; set; }

        /// Display Symbol of the company
        [MaxLength(8)]
        public string DisplaySymbol { get; set; }

        [ForeignKey("AssetFK")]
        public ICollection<DividendModel> Dividends { get; set; }

        [ForeignKey("AssetFK")]
        public ICollection<StockProfile> StockProfiles { get; set; }

        [ForeignKey("AssetFK")]
        public ICollection<StockCandle> StockCandles { get; set; }

        [ForeignKey("AssetFK")]
        public ICollection<StockPrice> StockPrice { get; set; }

        [ForeignKey("AssetFK")]
        public ICollection<StockSplit> StockSplits { get; set; }

        public bool Watching { get; set; }
    }
}
