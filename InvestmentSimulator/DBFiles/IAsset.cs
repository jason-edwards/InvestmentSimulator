using System;
using System.Threading.Tasks;
using ThreeFourteen.Finnhub.Client.Model;

namespace InvestmentSimulator.DBFiles
{
    interface IAsset
    {
        Task GetSymbols(string exchange);
        Task GetCandles(string symbol, Resolution resolution, DateTime? from, DateTime? to, bool adjusted = false);
    }
}
