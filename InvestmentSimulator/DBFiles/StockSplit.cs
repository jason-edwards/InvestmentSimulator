using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentSimulator.DBFiles
{
    public class StockSplit
    {
        /// Unique for combination of DateTime + Symbol
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SplitId { get; set; }

        /// Dividend Date
        public DateTime Date { get; set; }

        /// AssetId
        public long AssetFK { get; set; }

        /// Split Ratio = ToFactor/FromFactor. A Split Ratio of 5 means that after the stock split stock 
        /// holders will have 5 time the number of stocks and the stock price should drop by a factor of 5. 
        public decimal FromFactor { get; set; }

        /// Split Ratio = ToFactor/FromFactor. A Split Ratio of 5 means that after the stock split stock 
        /// holders will have 5 time the number of stocks and the stock price should drop by a factor of 5.  
        public decimal ToFactor { get; set; }
    }
}
