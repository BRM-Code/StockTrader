using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Linq.Expressions;

namespace StockTrader_.NET_Framework_
{
    public class DatabaseHandler
    {
        private MySqlConnection _connection;
        private string _cnnString;

        public DatabaseHandler(string cnnString)
        {
            _cnnString = cnnString;
            _connection = new MySqlConnection(cnnString);
        }

        public void UpdateDatabase(string symbol)
        {
            var wb = new WebClient();
            UriBuilder APIuribuild = new UriBuilder();
            APIuribuild.Scheme = "https";
            APIuribuild.Host = "cloud.iexapis.com";
            APIuribuild.Path = $"/stable/stock/{symbol}/quote";
            APIuribuild.Query = "token=pk_c08b953efdfb4f8dbae8afec9bb81fe0";
            Uri APIuri = APIuribuild.Uri;
            stock APIresponseObject = null;
            try
            {
                var APIresponse = wb.DownloadString(APIuri);
                try
                {
                    APIresponseObject = JsonConvert.DeserializeObject<stock>(APIresponse);
                }
                catch
                {
                    Console.WriteLine("Couldn't convert the json into a object, probably because a value was null");
                    return;
                }
            }
            catch
            {
                Console.WriteLine("The API isn't calling me back :'(");
            }

            string query = "INSERT INTO stocks (symbol, companyName, primaryExchange, calculationPrice, open, openTime, close, closeTime, high, low, latestPrice, latestSource, latestTime, latestUpdate, latestVolume, iexRealtimePrice, iexRealtimeSize, iexLastUpdated, delayedPrice, delayedPriceTime, extendedPrice, extendedChange, extendedChangePercent, extendedPriceTime, previousClose, previousVolume, change, changePercent, volume, iexMarketPercent, iexVolume, avgTotalVolume, iexBidPrice, iexBidSize, iexAskPrice, iexAskSize, marketCap, peRatio, week52High, week52Low, ytdChange, lastTradeTime, isUSMarketOpen )";

            {
                // create the query
                query += " VALUES (";
                query += $"'{APIresponseObject.symbol}', ";
                query += $"'{APIresponseObject.companyName}', ";
                query += $"'{APIresponseObject.primaryExchange}', ";
                query += $"'{APIresponseObject.calculationPrice}', ";
                query += $"'{APIresponseObject.open}', ";
                query += $"'{APIresponseObject.openTime}', ";
                query += $"'{APIresponseObject.close}', ";
                query += $"'{APIresponseObject.closeTime}', ";
                query += $"'{APIresponseObject.high}', ";
                query += $"'{APIresponseObject.low}', ";
                query += $"'{APIresponseObject.latestPrice}', ";
                query += $"'{APIresponseObject.latestSource}', ";
                query += $"'{APIresponseObject.latestTime}', ";
                query += $"'{APIresponseObject.latestUpdate}', ";
                query += $"'{APIresponseObject.latestVolume}', ";
                query += $"'{APIresponseObject.iexRealtimePrice}', ";
                query += $"'{APIresponseObject.iexRealtimeSize}', ";
                query += $"'{APIresponseObject.iexLastUpdated}', ";
               // query += $"'{APIresponseObject.delayedPrice}', ";
               // query += $"'{APIresponseObject.delayedPriceTime}', ";
                query += $"'{APIresponseObject.extendedPrice}', ";
                query += $"'{APIresponseObject.extendedChange}', ";
                query += $"'{APIresponseObject.extendedChangePercent}', ";
                query += $"'{APIresponseObject.extendedPriceTime}', ";
                query += $"'{APIresponseObject.previousClose}', ";
                query += $"'{APIresponseObject.previousVolume}', ";
                query += $"'{APIresponseObject.change}', ";
                query += $"'{APIresponseObject.changePercent}', ";
                query += $"'{APIresponseObject.volume}', ";
                query += $"'{APIresponseObject.iexMarketPercent}', ";
                query += $"'{APIresponseObject.iexVolume}', ";
                query += $"'{APIresponseObject.avgTotalVolume}', ";
                query += $"'{APIresponseObject.iexBidPrice}', ";
                query += $"'{APIresponseObject.iexBidSize}', ";
                query += $"'{APIresponseObject.iexAskPrice}', ";
                query += $"'{APIresponseObject.iexAskSize}', ";
                query += $"'{APIresponseObject.marketCap}', ";
                query += $"'{APIresponseObject.peRatio}', ";
                query += $"'{APIresponseObject.week52High}', ";
                query += $"'{APIresponseObject.week52Low}', ";
                query += $"'{APIresponseObject.ytdChange}', ";
                query += $"'{APIresponseObject.lastTradeTime}', ";
                query += $"'{APIresponseObject.isUSMarketOpen}'";

                query += ");";
            }

            // const string query = "INSERT INTO 'stocks' (symbol, companyName, primaryExchange, calculationPrice, open, openTime, close, closeTime, high, low, latestPrice, latestSource, latestTime, latestUpdate, latestVolume, iexRealtimePrice, iexRealtimeSize, iexLastUpdated, delayedPrice, delayedPriceTime, extendedPrice, extendedChange, extendedChangePercent, extendedPriceTime, previousClose, previousVolume, change, changePercent, volume, iexMarketPercent, iexVolume, avgTotalVolume, iexBidPrice, iexBidSize, iexAskPrice, iexAskSize, marketCap, peRatio, week52High, week52Low, ytdChange, lastTradeTime, isUSMarketOpen ) VALUES ( @symbol,  @companyName,  @primaryExchange,  @calculationPrice,  @open, @openTime, @close, @closeTime, @high, @low, @latestPrice, @latestSource, @latestTime, @latestUpdate, @latestVolume, @iexRealtimePrice, @iexRealtimeSize, @iexLastUpdated, @delayedPrice, @delayedPriceTime, @extendedPrice, @extendedChange, @extendedChangePercent, @extendedPriceTime, @previousClose, @previousVolume, @change, @changePercent, @volume, @iexMarketPercent, @iexVolume, @avgTotalVolume, @iexBidPrice, @iexBidSize, @iexAskPrice, @iexAskSize, @marketCap, @peRatio, @week52High, @week52Low, @ytdChange, @lastTradeTime, @isUSMarketOpen);";
            // const string query = "SELECT * FROM stocks";
            
            _connection.Open();
            var command = new MySqlCommand(query, _connection);

            command.Parameters.Add("@symbol",MySqlDbType.TinyText);
            command.Parameters.Add("@companyName", MySqlDbType.TinyText);
            command.Parameters.Add("@primaryExchange", MySqlDbType.TinyText);
            command.Parameters.Add("@calculationPrice", MySqlDbType.Float);
            command.Parameters.Add("@open",MySqlDbType.Float);
            command.Parameters.Add("@openTime",MySqlDbType.Int64);
            command.Parameters.Add("@close",MySqlDbType.Float);
            command.Parameters.Add("@closeTime", MySqlDbType.Int64);
            command.Parameters.Add("@high", MySqlDbType.Float);
            command.Parameters.Add("@low", MySqlDbType.Float);
            command.Parameters.Add("@latestPrice", MySqlDbType.Float);
            command.Parameters.Add("@latestSource", MySqlDbType.TinyText);
            command.Parameters.Add("@latestTime", MySqlDbType.TinyText);
            command.Parameters.Add("@latestUpdate", MySqlDbType.Int64);
            command.Parameters.Add("@latestVolume", MySqlDbType.Int64);
            command.Parameters.Add("@iexRealtimePrice", MySqlDbType.Float);
            command.Parameters.Add("@iexRealtimeSize", MySqlDbType.Int64);
            command.Parameters.Add("@iexLastUpdated", MySqlDbType.Int64);
            command.Parameters.Add("@delayedPrice", MySqlDbType.Float);
            command.Parameters.Add("@delayedPriceTime", MySqlDbType.Int64);
            command.Parameters.Add("@extendedPrice", MySqlDbType.Float);
            command.Parameters.Add("@extendedChange", MySqlDbType.Float);
            command.Parameters.Add("@extendedChangePercent", MySqlDbType.Float);
            command.Parameters.Add("@extendedPriceTime", MySqlDbType.Int64);
            command.Parameters.Add("@previousClose", MySqlDbType.Float);
            command.Parameters.Add("@previousVolume", MySqlDbType.Int64);
            command.Parameters.Add("@change", MySqlDbType.Float);
            command.Parameters.Add("@changePercent", MySqlDbType.Float);
            command.Parameters.Add("@volume", MySqlDbType.Int64);
            command.Parameters.Add("@iexMarketPercent", MySqlDbType.Float);
            command.Parameters.Add("@iexVolume", MySqlDbType.Int64);
            command.Parameters.Add("@avgTotalVolume", MySqlDbType.Int64);
            command.Parameters.Add("@iexBidPrice", MySqlDbType.Float);
            command.Parameters.Add("@iexBidSize", MySqlDbType.Int64);
            command.Parameters.Add("@iexAskPrice", MySqlDbType.Float);
            command.Parameters.Add("@iexAskSize", MySqlDbType.Float);
            command.Parameters.Add("@marketCap", MySqlDbType.Int64);
            command.Parameters.Add("@peRatio", MySqlDbType.Float);
            command.Parameters.Add("@week52High", MySqlDbType.Float);
            command.Parameters.Add("@week52Low", MySqlDbType.Float);
            command.Parameters.Add("@ytdChange", MySqlDbType.Float);
            command.Parameters.Add("@lastTradeTime", MySqlDbType.Int64);
            command.Parameters.Add("@isUSMarketOpen",MySqlDbType.Bit);


            command.Parameters["@symbol"].Value = APIresponseObject.symbol ;
            command.Parameters["@companyName"].Value = APIresponseObject.companyName ;
            command.Parameters["@primaryExchange"].Value = APIresponseObject.primaryExchange ;
            command.Parameters["@calculationPrice"].Value = APIresponseObject.calculationPrice ;
            command.Parameters["@open"].Value = APIresponseObject.open ;
            command.Parameters["@openTime"].Value = APIresponseObject.openTime ;
            command.Parameters["@close"].Value = APIresponseObject.close ;
            command.Parameters["@closeTime"].Value = APIresponseObject.closeTime ;
            command.Parameters["@high"].Value = APIresponseObject.high ;
            command.Parameters["@low"].Value = APIresponseObject.low ;
            command.Parameters["@latestPrice"].Value = APIresponseObject.latestPrice ;
            command.Parameters["@latestSource"].Value = APIresponseObject.latestSource ;
            command.Parameters["@latestTime"].Value = APIresponseObject.latestTime ;
            command.Parameters["@latestUpdate"].Value = APIresponseObject.latestUpdate ;
            command.Parameters["@latestVolume"].Value = APIresponseObject.latestVolume ;
            command.Parameters["@iexRealtimePrice"].Value = APIresponseObject.iexRealtimePrice ;
            command.Parameters["@iexRealtimeSize"].Value = APIresponseObject.iexRealtimeSize ;
            command.Parameters["@iexLastUpdated"].Value = APIresponseObject.iexLastUpdated ;
            command.Parameters["@delayedPrice"].Value = APIresponseObject.delayedPrice ;
            command.Parameters["@delayedPriceTime"].Value = APIresponseObject.delayedPriceTime ;
            command.Parameters["@extendedPrice"].Value = APIresponseObject.extendedPrice ;
            command.Parameters["@extendedChange"].Value = APIresponseObject.extendedChange ;
            command.Parameters["@extendedChangePercent"].Value = APIresponseObject.extendedChangePercent ;
            command.Parameters["@extendedPriceTime"].Value = APIresponseObject.extendedPriceTime ;
            command.Parameters["@previousClose"].Value = APIresponseObject.previousClose ;
            command.Parameters["@previousVolume"].Value = APIresponseObject.previousVolume ;
            command.Parameters["@change"].Value = APIresponseObject.change ;
            command.Parameters["@changePercent"].Value = APIresponseObject.changePercent ;
            command.Parameters["@volume"].Value = APIresponseObject.volume ;
            command.Parameters["@iexMarketPercent"].Value = APIresponseObject.iexMarketPercent ;
            command.Parameters["@iexVolume"].Value = APIresponseObject.iexVolume ;
            command.Parameters["@avgTotalVolume"].Value = APIresponseObject.avgTotalVolume ;
            command.Parameters["@iexBidPrice"].Value = APIresponseObject.iexBidPrice ;
            command.Parameters["@iexBidSize"].Value = APIresponseObject.iexBidSize ;
            command.Parameters["@iexAskPrice"].Value = APIresponseObject.iexAskPrice ;
            command.Parameters["@iexAskSize"].Value = APIresponseObject.iexAskSize ;
            command.Parameters["@marketCap"].Value = APIresponseObject.marketCap ;
            command.Parameters["@peRatio"].Value = APIresponseObject.peRatio ;
            command.Parameters["@week52High"].Value = APIresponseObject.week52High ;
            command.Parameters["@week52Low"].Value = APIresponseObject.week52Low ;
            command.Parameters["@ytdChange"].Value = APIresponseObject.ytdChange ;
            command.Parameters["@lastTradeTime"].Value = APIresponseObject.lastTradeTime ;
            command.Parameters["@isUSMarketOpen"].Value = APIresponseObject.isUSMarketOpen ;

            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
