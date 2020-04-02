using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InvestmentSimulator.DBFiles
{
    public class AssetContext : DbContext
    {
        public DbSet<AssetProperties> AssetProperties { get; set; }
        public DbSet<Dividend> Dividends { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=Investment.db");
    }
    
    
    public class AssetProperties
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
        [MaxLength(3), Required]
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
        public ICollection<Dividend> Dividends { get; set; }
    }

    public class Dividend
    {
        /// Unique for combination of DateTime + Symbol
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DividendId { get; set; }

        /// Dividend Date
        public DateTime Date { get; set; }

        /// Ammount in currency per share
        public decimal Amount { get; set; }

        /// AssetId
        public long AssetFK { get; set; }
    }

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
        public ICollection<AssetProperties> Assets { get; set; }
    }
}
