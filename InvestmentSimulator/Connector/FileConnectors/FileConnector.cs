using CsvHelper;
using InvestmentSimulator.Connector.FileConnectors;
using InvestmentSimulator.DBFiles;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace InvestmentSimulator.Connector.FileConnector
{
    /// <summary>
    /// FileConnector is used to import/export data to/from the database. Does not connect files to the API or vice versa.
    /// Only reads CSV. Default file system is Market Capitalisation.
    /// </summary>
    public class FileConnector
    {
        private string _fileName;
        private FileSystem _fileSystem;
        private StockContext _dbContext;

        internal FileConnector(string fileName, string system, StockContext context)
        {
            _fileName = fileName;
            _dbContext = context;

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
                
            }
        }

        public bool Import()
        {
            bool success = true;

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

        /// Reads csv with 1 column: symbol. (e.g. CBA.AX) 
        /// No heading row.
        /// All symbols in the list will be have Watching set to True in StockProperty table.
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
                    var exchangeId = _dbContext.Exchanges
                        .Where(b => b.Code == r.Exchange).FirstOrDefault().ExchangeId;

                    if(exchangeId > 0)
                    {
                        var query = _dbContext.StockProperties
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
                _dbContext.SaveChanges();

            }

            return success;
        }
    }
}
