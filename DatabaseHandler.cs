using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Net;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace StockTrader_.NET_Framework_
{
    class DatabaseHandler
    {
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
                    Console.WriteLine("Couldn't convert the json into a object");
                    return;
                }
            }
            catch
            {
                Console.WriteLine("The API isn't calling me back :'(");
            }

            using (var connection = new SqlConnection("Server = db.jakewalker.xyz; Database = benrm1; Username = benrm; Password = tiWuSIMo4IBo£"))
            {
                connection.Open();
                var affectedRows = connection.Execute("Insert into stocks (symbol, companyName, primaryExchange, calculationPrice, open, openTime, close, closeTime, high, low, latestPrice, latestSource, latestTime, latestUpdate, latestVolume, iexRealtimePrice, iexRealtimeSize, iexLastUpdated, delayedPrice, delayedPriceTime, extendedPrice, extendedChange, extendedChangePercent, extendedPriceTime, previousClose, previousVolume, change, changePercent, volume, iexMarketPercent, iexVolume, avgTotalVolume, iexBidPrice, iexBidSize, iexAskPrice, iexAskSize, marketCap, peRatio, week52High, week52Low, ytdChange, lastTradeTime, isUSMarketOpen ) values (@symbol,@companyName,@primaryExchange,@calculationPrice,@open,@openTime,@close,@closeTime,@high,@low,@latestPrice,@latestSource,@latestTime,@latestUpdate,@latestVolume,@iexRealtimePrice,@iexRealtimeSize,@iexLastUpdated,@delayedPrice,@delayedPriceTime,@extendedPrice,@extendedChange,@extendedChangePercent,@extendedPriceTime,@previousClose,@previousVolume,@change,@changePercent,@volume,@iexMarketPercent,@iexVolume,@avgTotalVolume,@iexBidPrice,@iexBidSize,@iexAskPrice,@iexAskSize,@marketCap,@peRatio,@week52High,@week52Low,@ytdChange,@lastTradeTime,@isUSMarketOpen)",
                    new { symbol = APIresponseObject.symbol, companyName = APIresponseObject.companyName, primaryExchange = APIresponseObject.primaryExchange, calculationPrice = APIresponseObject.calculationPrice, open = APIresponseObject.open, openTime = APIresponseObject.openTime, close = APIresponseObject.close, closeTime = APIresponseObject.closeTime, high = APIresponseObject.high, low = APIresponseObject.low, symlatestPricebol = APIresponseObject.latestPrice, latestSource = APIresponseObject.latestSource, latestTime = APIresponseObject.latestTime, latestUpdate = APIresponseObject.latestUpdate, latestVolume = APIresponseObject.latestVolume, iexRealtimePrice = APIresponseObject.iexRealtimePrice, iexRealtimeSize = APIresponseObject.iexRealtimeSize, iexLastUpdated = APIresponseObject.iexLastUpdated, delayedPrice = APIresponseObject.delayedPrice, delayedPriceTime = APIresponseObject.delayedPriceTime, extendedPrice = APIresponseObject.extendedPrice, extendedChange = APIresponseObject.extendedChange, extendedChangePercent = APIresponseObject.extendedChangePercent, extendedPriceTime = APIresponseObject.extendedPriceTime, previousClose = APIresponseObject.previousClose, previousVolume = APIresponseObject.previousVolume, change = APIresponseObject.change, changePercent = APIresponseObject.changePercent, volume = APIresponseObject.volume, iexMarketPercent = APIresponseObject.iexMarketPercent, iexVolume = APIresponseObject.iexVolume, avgTotalVolume = APIresponseObject.avgTotalVolume, iexBidPrice = APIresponseObject.iexBidPrice, iexBidSize = APIresponseObject.iexBidSize, iexAskPrice = APIresponseObject.iexAskPrice, iexAskSize = APIresponseObject.iexAskSize, marketCap = APIresponseObject.marketCap, peRatio = APIresponseObject.peRatio, week52High = APIresponseObject.week52High, week52Low = APIresponseObject.week52Low, ytdChange = APIresponseObject.ytdChange, lastTradeTime = APIresponseObject.lastTradeTime, isUSMarketOpen = APIresponseObject.isUSMarketOpen });
                connection.Close();
                Console.WriteLine($"{affectedRows} row(s) have been affected");
            }
        }
    }
}
