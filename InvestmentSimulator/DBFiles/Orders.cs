using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentSimulator.DBFiles
{
    class Orders
    {
        /// Primary key for table
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderId { get; set; }

        /// AssetId
        public long AssetFK { get; set; }

        /// Exchange FK
        public long ExchangeFK { get; set; }

        /// Timestamp of price
        public DateTime TimeStamp { get; set; }

        /// Price (equivalent after trasaction fees)
        public decimal OrderPrice { get; set; }

        /// Quantity
        public decimal OrderQuantity { get; set; }

        // Order Type
        public OrderType TypeOfOrder { get; set; }

        // Order Status
        public OrderStatus StatusOfOrder { get; set; }

        // OrderId Pair
        public long OrderPairId { get; set; }
    }
}
