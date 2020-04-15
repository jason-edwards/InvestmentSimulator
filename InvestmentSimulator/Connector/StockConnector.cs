using InvestmentSimulator.DBFiles;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThreeFourteen.Finnhub.Client;
using ThreeFourteen.Finnhub.Client.Model;

namespace InvestmentSimulator.Connector
{
    class StockConnector : IAsset
    {
        private readonly FinnhubClient _finnhubClient;
        private readonly StockContext _dbContext;
        internal StockConnector(FinnhubClient finnhubClient, StockContext dbContext)
        {
            _finnhubClient = finnhubClient;
            _dbContext = dbContext;
        }

        public async Task GetSymbols(string exchange)
        {
            var result = _dbContext.Exchanges
                .Where(b => b.Code.Contains(exchange));

            if (result.Count() == 1)
            {
                Symbol[] symbols = await _finnhubClient.Stock.GetSymbols(exchange);

                foreach (var symbol in symbols)
                {
                    var query = _dbContext.StockProperties
                        .Where(b => b.Symbol == symbol.CandleSymbol);
                    
                    if (!(query.Count() > 0)) 
                    {
                        _dbContext.Add(new StockProperty { Symbol = symbol.CandleSymbol, DisplaySymbol = symbol.DisplaySymbol, ExchangeFK = result.First().ExchangeId });
                    } else
                    {
                        Log.Information($"Record not added, Table[AssetProperties]: {query.FirstOrDefault().Symbol} already exists in db and was not added.");
                    }
                }

                _dbContext.SaveChanges();
            } 
            else
            {
                var exchangeConnector = new ExchangeConnector(_finnhubClient, _dbContext);
                await exchangeConnector.GetExchanges();
            }
        }

        public async Task GetCandles(string symbol, Resolution resolution, DateTime? startDate=null, DateTime? endDate=null, bool adjusted=false)
        {
            DateTime to = endDate ?? DateTime.Now;
            DateTime from = startDate ?? DateTime.Now.AddDays(-7);

            var stockQuery = _dbContext.StockProperties
                .Where(b => b.Symbol == symbol);

            long symbolKey;

            if (stockQuery.Count() == 1)
            {
                symbolKey = stockQuery.FirstOrDefault().AssetId;

                var candlesQuery = _dbContext.Candles
                    .Where(b => b.AssetFK == symbolKey);

                Candle[] candles = await _finnhubClient.Stock.GetCandles(symbol, resolution, from, to, adjusted);

                foreach (var candle in candles)
                {
                    var candleRecord = candlesQuery
                        .Where(b => b.TimeStamp == candle.Timestamp && 
                        b.Resolution == resolution.ToString() &&
                        b.Adjusted == adjusted);

                    if (candleRecord.Count() == 0)
                    {
                        _dbContext.Candles.Add(new StockCandle
                        {
                            AssetFK = symbolKey,
                            TimeStamp = candle.Timestamp,
                            Open = candle.Open,
                            Close = candle.Close,
                            High = candle.High,
                            Low = candle.Low,
                            Volume = candle.Volume,
                            Resolution = resolution.ToString(),
                            Adjusted = adjusted
                        });
                    }
                    else
                    {
                        Log.Information($"Record not added, Table[StockCandle]: " +
                            $"Combination of symbol:{symbol} and time:{candle.Timestamp} already exists in db and was not added.");
                    }

                    AddPriceFromCandle(candle, symbolKey, resolution);


                }
                _dbContext.SaveChanges();
            } 
            else
            {
                Log.Information($"Unknown Symbol, InvestmentSimulator.Connector.StockConnector.GetCandles: " +
                    $"{symbol} does not exist within {_dbContext.StockProperties}");
            }

        }

        public async Task GetDividends(string symbol, DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime to = endDate ?? DateTime.Now;
            DateTime from = startDate ?? DateTime.Now.AddYears(-1);

            var stockQuery = _dbContext.StockProperties
                .Where(b => b.Symbol == symbol);

            long symbolKey;

            if (stockQuery.Count() == 1)
            {
                symbolKey = stockQuery.FirstOrDefault().AssetId;

                var dividendQuery = _dbContext.Dividends
                    .Where(b => b.AssetFK == symbolKey);

                Dividend[] dividends = await _finnhubClient.Stock.GetDividends(symbol, from, to);

                foreach (var dividend in dividends)
                {
                    var record = dividendQuery
                        .Where(b => b.Date == dividend.Date);

                    if (record.Count() == 0)
                    {
                        _dbContext.Dividends.Add(new DividendModel { AssetFK = symbolKey, Date = dividend.Date, Amount = dividend.Amount });
                    }
                    else
                    {
                        Log.Information($"Record not added, Table[Dividend]: Combination of {symbol} and {dividend.Date} already exists in db and was not added.");
                    }
                }
                _dbContext.SaveChanges();
            }
            else if (stockQuery.Count() == 0)
            {
                Log.Information($"Unknown Symbol, InvestmentSimulator.Connector.StockConnector.GetDividends: " +
                    $"{symbol} does not exist within {_dbContext.StockProperties}");
            }
            else
            {
                Log.Information($"Duplicate Record, Table[AssetProperties]: Multiple entries of {symbol} exist in db.");
            }
        }

        public async Task GetAssetProperties(string symbol)
        {
            Log.Information("Premium Access, InvestmentSimulator.Connector.StockConnector.GetAssetProperties: This function is only for Premium Accounts");
            await Task.CompletedTask;
            /*
            DateTime date = DateTime.Now.Date;

            Company company = await _finnhubClient.Stock.GetCompany(symbol);

            var query = _dbContext.AssetProperties
                        .Where(b => b.Symbol == symbol)
                        .ToList();

            if (!(query.Count() > 0))
            {
                await GetSymbols(company.Exchange);
                await GetAssetProperties(symbol);
            } 
            else 
            {
                var propertyRecord = _dbContext.AssetProperties
                        .FirstOrDefault(b => b.Symbol == symbol);

                if (propertyRecord != null)
                {
                    propertyRecord.Currency = company.Currency;
                    propertyRecord.Cusip = company.Cusip;
                    propertyRecord.IpoDate = company.Ipo;
                    propertyRecord.Isin = company.Isin;
                    propertyRecord.Sedol = company.Sedol;
                    propertyRecord.Ticker = company.Ticker;
                    propertyRecord.AssetProfiles = new List<AssetProfile>();
                }

                var profileRecord = propertyRecord.AssetProfiles
                    .FirstOrDefault(b => b.Date == date);

                if (profileRecord != null)
                {
                    profileRecord.Address = company.Address;
                    profileRecord.City = company.City;
                    profileRecord.Country = company.Country;
                    profileRecord.Date = date;
                    profileRecord.Description = company.Description;
                    profileRecord.EmployeeTotal = int.Parse(company.EmployeeTotal);
                    profileRecord.GGroup = company.Ggroup;
                    profileRecord.GInd = company.Gind;
                    profileRecord.GSector = company.Gsector;
                    profileRecord.GSubInd = company.Gsubind;
                    profileRecord.MarketCap = company.MarketCap;
                    profileRecord.Name = company.Name;
                    profileRecord.NInd = company.Naics;
                    profileRecord.NNatInd = company.NaicsNatInd;
                    profileRecord.NSector = company.NaicsSector;
                    profileRecord.NSubsector = company.NaicsSubsector;
                    profileRecord.Phone = company.Phone;
                    profileRecord.ShareOutstanding = company.ShareOutstanding;
                    profileRecord.State = company.State;
                    profileRecord.WebURL = company.Weburl;
                } 
                else 
                {
                    propertyRecord.AssetProfiles.Add(new AssetProfile
                    {
                        Address = company.Address,
                        City = company.City,
                        Country = company.Country,
                        Date = date,
                        Description = company.Description,
                        EmployeeTotal = int.Parse(company.EmployeeTotal),
                        GGroup = company.Ggroup,
                        GInd = company.Gind,
                        GSector = company.Gsector,
                        GSubInd = company.Gsubind,
                        MarketCap = company.MarketCap,
                        Name = company.Name,
                        NInd = company.Naics,
                        NNatInd = company.NaicsNatInd,
                        NSector = company.NaicsSector,
                        NSubsector = company.NaicsSubsector,
                        Phone = company.Phone,
                        ShareOutstanding = company.ShareOutstanding,
                        State = company.State,
                        WebURL = company.Weburl
                    });
                }
                _dbContext.SaveChanges();
                
            }*/

        }

        public async Task GetStockSplit(string symbol, DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime to = endDate ?? DateTime.MaxValue;
            DateTime from = startDate ?? DateTime.MinValue;

            var stockQuery = _dbContext.StockProperties
                .Where(b => b.Symbol == symbol);

            if (stockQuery.Count() == 1)
            {
                var stockKey = stockQuery.First().AssetId;

                Split[] stockSplits = await _finnhubClient.Stock.GetSplits(symbol, from, to);

                foreach(var stockSplit in stockSplits)
                {
                    var record = _dbContext.StockSplits
                        .Where(b => b.Date == stockSplit.Date && b.AssetFK == stockKey);

                    if(record.Count() == 0)
                    {
                        _dbContext.StockSplits.Add(new StockSplit
                        {
                            AssetFK = stockKey,
                            Date = stockSplit.Date,
                            FromFactor = stockSplit.fromFactor,
                            ToFactor = stockSplit.toFactor
                        });
                    }
                    else
                    {
                        Log.Information($"Record not added, Table[Split]: " +
                            $"Combination of {symbol} and {stockSplit.Date} already exists in db and was not added.");
                    }
                }
                _dbContext.SaveChanges();
            }
            else if (stockQuery.Count() == 0)
            {
                Log.Information($"Unknown Symbol, InvestmentSimulator.Connector.StockConnector.GetStockSplit: " +
                    $"{symbol} does not exist within {_dbContext.StockProperties}");
            }
            else
            {
                Log.Information($"Duplicate Record, Table[AssetProperties]: Multiple entries of {symbol} exist in db.");
            }
        }

        private void AddPriceFromCandle(Candle candle, long symbolKey, Resolution resolution)
        {
            var record = _dbContext.StockPrices
                .Where(b => b.AssetFK == symbolKey && b.TimeStamp == candle.Timestamp);

            if (record.Count() == 0)
            {
                _dbContext.StockPrices.Add(new StockPrice
                {
                    Amount = candle.Open,
                    TimeStamp = candle.Timestamp,
                    AssetFK = symbolKey
                });
            }

            /// Only add closing price for smaller time resolutions. Certain days, weeks and months have altered time periods during the year.
            /// Not all exchanges close on the hour mark.
            DateTime? closingTime = null;
            switch (resolution)
            {
                case Resolution.OneMinute:
                    closingTime = candle.Timestamp.AddSeconds(59);
                    break;
                case Resolution.FiveMinutes:
                    closingTime = candle.Timestamp.AddSeconds(59).AddMinutes(4);
                    break;
                case Resolution.FifteenMinutes:
                    closingTime = candle.Timestamp.AddSeconds(59).AddMinutes(14);
                    break;
                case Resolution.ThirtyMinutes:
                    closingTime = candle.Timestamp.AddSeconds(59).AddMinutes(29);
                    break;
            }

            if (closingTime != null)
            {
                record = _dbContext.StockPrices
                    .Where(b => b.AssetFK == symbolKey && b.TimeStamp == closingTime);
                
                if(record.Count() == 0)
                {
                    _dbContext.StockPrices.Add(new StockPrice
                    {
                        Amount = candle.Close,
                        AssetFK = symbolKey,
                        TimeStamp = closingTime ?? new DateTime()
                    });
                }
            }
            _dbContext.SaveChanges();
        }
    }
}
