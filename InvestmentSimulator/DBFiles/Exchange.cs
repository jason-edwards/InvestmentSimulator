using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentSimulator.DBFiles
{
    public class Exchange
    {
        /// ExchangeId
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ExchangeId { get; set; }

        /// Name of the exchange
        [MaxLength(50)]
        public string Name { get; set; }

        ///Listed exchange
        [MaxLength(20), Required]
        public string Code { get; set; }

        /// Supported assests on the exchange 
        [ForeignKey("ExchangeFK")]
        public ICollection<StockProperty> Assets { get; set; }
    }
}
