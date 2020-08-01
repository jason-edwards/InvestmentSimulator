using CsvHelper;
using InvestmentSimulator.Brokerage;
using InvestmentSimulator.DBFiles;
using InvestmentSimulator.Simulation;
using Serilog;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace InvestmentSimulator.Connector.FileConnector
{
    /// <summary>
    /// FileConnector is used to import/export data to/from the database. Does not connect files to the API or vice versa.
    /// Only reads CSV. Default file system is Market Capitalisation.
    /// </summary>
    public class FileConnector
    {
        private readonly string _fileName;
        private readonly FileSystem _fileSystem;
        private StockContext _dbStockContext;
        private ITradeConditions _tradeConditions;

        internal FileConnector(string fileName, string system)
        {
            _fileName = fileName;

            switch(system)
            {
                case "Candle":
                    _fileSystem = FileSystem.Candles;
                    break;
                case "Company":
                    _fileSystem = FileSystem.CompanyList;
                    break;
                case "Dividend":
                    _fileSystem = FileSystem.Dividends;
                    break;
                case "MarketCap":
                    _fileSystem = FileSystem.MarketCap;
                    break;
                case "Prices":
                    _fileSystem = FileSystem.Prices;
                    break;
                case "Profiles":
                    _fileSystem = FileSystem.Profiles;
                    break;
                case "Properties":
                    _fileSystem = FileSystem.Properties;
                    break;
                case "Splits":
                    _fileSystem = FileSystem.Splits;
                    break;
                case "Watchlist":
                    _fileSystem = FileSystem.Watchlist;
                    break;
                case "TradeConditions":
                    _fileSystem = FileSystem.TradeConditions;
                    break;
            }
        }

        /// <summary>
        /// Imports data from a csv file to the database based off of the input file system. Only actions information for stocks on the watchlist.
        /// </summary>
        /// <returns>
        /// bool success : 
        /// true if all lines in the file were correctly read. 
        /// returns false if there were any problems adding reading from file or writing to db.
        /// </returns>
        public bool ImportToStockDB(StockContext context, bool overrider = true)
        {
            bool success = true;
            _dbStockContext = context;

            switch (_fileSystem)
            {
                case FileSystem.Candles:
                    break;
                case FileSystem.CompanyList:
                    break;
                case FileSystem.Dividends:
                    break;
                case FileSystem.MarketCap:
                    break;
                case FileSystem.Prices:
                    break;
                case FileSystem.Profiles:
                    break;
                case FileSystem.Properties:
                    break;
                case FileSystem.Splits:
                    success = ImportSplits(overrider);
                    break;
                case FileSystem.Watchlist:
                    success = ImportWatchlist();
                    break;
                default:
                    Log.Error($"File {_fileName} not imported, Unknown filesystem: {_fileSystem}");
                    success = false;
                    break;
            }
            return success;
        }

        public (bool, ITradeConditions) ImportBaseTradeConditions()
        {
            bool success = true;
            _tradeConditions = new BaseTradeConditions();

            using (var reader = new StreamReader(_fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Read();
                csv.ReadHeader();

                var record = csv.GetRecords<BaseTradeConditionsFile>().ToList().FirstOrDefault();

                _tradeConditions.InitialCapital = record.InitialCapital;
                _tradeConditions.Brokerage = BrokerageInteractions.BrokerageFromString(record.Brokerage);
                _tradeConditions.OrderSize.OrderSizeCurrency = record.OrderSizeCurrency;
                _tradeConditions.OrderSize.OrderSizePercentage = record.OrderSizePercentage;
                _tradeConditions.OrderSize.CurrencyCutoverToPercentageAmount = record.CurrencyCutoverToPercentageAmount;
                _tradeConditions.MaxContractHeld = record.MaxContractsHeld;
                _tradeConditions.BuyCooldown = (record.BuyCooldown, TradingRateInteractions.TradingRateFromString(record.BuyCooldownRate));
            }

            return (success, _tradeConditions);
        }

        /// <summary>
        /// Reads csv with 2 columns: symbol. (e.g. CBA.AX), Exchange (e.g. AX)
        /// Without heading row.
        /// All symbols in the list will be have Watching set to True in StockProperty table.
        /// </summary>
        public bool ImportWatchlist()
        {
            bool success = true;

            using (var reader = new StreamReader(_fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = false;
                csv.Read();

                var records = csv.GetRecords<WatchlistFile>().ToList();

                foreach (var r in records)
                {
                    var exchangeId = _dbStockContext.Exchanges
                        .Where(b => b.Code == r.Exchange).FirstOrDefault().ExchangeId;

                    if(exchangeId > 0)
                    {
                        var query = _dbStockContext.StockProperties
                            .Where(b => (b.Symbol == r.Symbol) && (b.ExchangeFK == exchangeId));

                        if(query.Count() > 0)
                        {
                            query.FirstOrDefault().Watching = true;
                        }
                        else
                        {
                            Log.Error($"Symbol {r.Symbol} not found in table StockProperties, Entry not added from: {_fileName}");
                            success = false;
                        }
                    } 
                    else
                    {
                        Log.Error($"Exchange {r.Exchange} not found in table Exchanges, Entry not added from: {_fileName}");
                        success = false;
                    }
                }
                _dbStockContext.SaveChanges();

            }

            return success;
        }

        /// <summary>
        /// Reads csv with 6 columns: symbol. (e.g. CBA.AX), year (yyyy), month (m), day (d), ToFactor, FromFactor
        /// With heading row.
        /// Adds new split data in StockSplit table.
        /// If overrider is set, then the existing splits data will overridden (ToFactor and FromFactor only).
        /// If overrider is not set, then if exisiting symbol + date combination is found the item will be skipped.
        /// </summary>
        public bool ImportSplits(bool overrider)
        {
            bool success = true;

            using (var reader = new StreamReader(_fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Read();
                csv.ReadHeader();

                var records = csv.GetRecords<SplitFile>().ToList();

                foreach (var r in records)
                {
                    var watchlingQuery = _dbStockContext.StockProperties
                        .Where(b => b.Symbol == r.Symbol && b.Watching == true);

                    if (watchlingQuery.Count() == 0) 
                    {
                        Log.Information($"Record not added/ updated. {r.Symbol} either does not exist in database or is not on the watchlist");
                        continue; 
                    }

                    DateTime date = new DateTime(r.Year, r.Month, r.Day);
                    long assetId = watchlingQuery.FirstOrDefault().AssetId;

                    var splitQuery = _dbStockContext.StockSplits
                        .Where(b => (b.AssetFK == assetId) && (b.Date.Date == date.Date));

                    if (splitQuery.Count() > 0 && overrider == true)
                    {
                        splitQuery.FirstOrDefault().FromFactor = r.FromFactor;
                        splitQuery.FirstOrDefault().ToFactor = r.ToFactor;
                    }
                    else if (splitQuery.Count() == 0)
                    {
                        _dbStockContext.StockSplits.Add(new StockSplit
                        {
                            AssetFK = assetId,
                            Date = date,
                            FromFactor = r.FromFactor,
                            ToFactor = r.ToFactor
                        });
                    }
                    else if (splitQuery.Count() > 0 && overrider == false)
                    {
                        Log.Information($"Record not updated, {r.Symbol} exists in database but override set to {overrider}");
                    }
                    else
                    {
                        Log.Error($"Record not added/ updated. Unknown problem with {r.Symbol}");
                        success = false;
                    }
                }
            }

            _dbStockContext.SaveChanges();
            return success;
        }


    }
}
