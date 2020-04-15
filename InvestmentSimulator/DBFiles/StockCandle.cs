using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InvestmentSimulator.DBFiles
{
    public class StockCandle
    {
        /// Primary key for table - maps with unique Symbol + Echange
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CandleId { get; set; }

        /// AssetId
        public long AssetFK { get; set; }

        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public long Volume { get; set; }
        
        // Starting time of candle
        public DateTime TimeStamp { get; set; }

        [MaxLength(2)]
        public string Resolution { get; set; }

        public bool Adjusted { get; set; }
    }
}
