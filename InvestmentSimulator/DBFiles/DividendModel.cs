using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentSimulator.DBFiles
{
    public class DividendModel
    {
        /// Unique for combination of DateTime + Symbol
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DividendId { get; set; }
        
        /// Dividend Date
        public DateTime Date { get; set; }

        /// AssetId
        public long AssetFK { get; set; }

        /// Amount in currency
        public decimal Amount { get; set; }

        /// Percent yeild. Amount / Shareprice at open.
        public decimal Yield { get; set; }
    }
}
