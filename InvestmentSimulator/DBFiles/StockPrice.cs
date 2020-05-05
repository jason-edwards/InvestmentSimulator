using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentSimulator.DBFiles
{
    public class StockPrice
    {
        /// Primary key for table - maps with unique Symbol + Echange
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PriceId { get; set; }

        /// AssetId
        public long AssetFK { get; set; }

        /// Timestamp of price
        public DateTime TimeStamp { get; set; }

        /// Current Price
        public decimal Amount { get; set; }

        public bool Adjusted { get; set; }
    }
}
