using Microsoft.EntityFrameworkCore;

namespace InvestmentSimulator.DBFiles
{
    public class StockContext : DbContext
    {
        public DbSet<StockProperty> StockProperties { get; set; }
        public DbSet<DividendModel> Dividends { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<StockProfile> StockProfiles { get; set; }
        public DbSet<StockCandle> Candles { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }
        public DbSet<StockSplit> StockSplits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=Investment.db");
    }
}
