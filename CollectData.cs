using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class ApiCommunicator
    {
        public static JToken CollectData(string company)
        {
            //The url that we are constructing 
            //https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=MSFT&interval=5min&apikey=Q4AZUW80DHGKUS3A

            using var wb = new WebClient();
                string APIresponse;
                string APItoken = "Q4AZUW80DHGKUS3A";
                UriBuilder APIuribuild = new UriBuilder();//Setting up the UriBuilder
                APIuribuild.Scheme = "https";
                APIuribuild.Host = "www.alphavantage.co";
                APIuribuild.Path = $"/query";
                APIuribuild.Query = $"function=TIME_SERIES_INTRADAY&symbol={company}&interval=5min&apikey={APItoken}";
                Uri APIuri = APIuribuild.Uri;//Tells UriBuilder that all the URL parts are there
                Console.WriteLine(APIuri);
                try
                {APIresponse = wb.DownloadString(APIuri);}
                catch
                {Console.WriteLine("The API isn't calling me back :(");
                    return null;}
                
                JObject responseJObject = JObject.Parse(APIresponse);
                DateTime lastRefresh = (DateTime)responseJObject["Meta Data"]["3. Last Refreshed"];
                // responseJObject.Property("Meta Data").Remove();
                //Makes a array called keys that is full of the latest datapoints
                var keys = responseJObject["Time Series (5min)"].ToObject<Dictionary<string, object>>().Keys.ToArray();
                var values = responseJObject["Time Series (5min)"];
                
                //var values = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(responseJObject.ToString());
                //converts the responseJObject into a string and then turns that string into a dictionary
                return values;
        }
    }
}
