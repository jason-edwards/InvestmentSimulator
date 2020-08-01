using InvestmentSimulator.Brokerage;

namespace InvestmentSimulator.Simulation
{
    public interface ITradeConditions
    {
        public decimal InitialCapital { get; set; }
        public IBrokerage Brokerage { get; set; }
        public OrderSize OrderSize { get; set; }
        public int MaxContractHeld { get; set; }
        public (int, TradingRate) BuyCooldown { get; set; }
        public OrderType CheckConditions();

        public void LoadConditionsFromFile(string filePath);
    }
}
