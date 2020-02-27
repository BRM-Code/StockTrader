using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class ApiCommunicator
    {
        public static float CurrentPrice(string company)
        {
            JToken data = CollectData(company,0);
            float value = Convert.ToSingle(data[data.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]]["1. open"]);
            return value;
        }

        public static void RotateProxy()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://free-proxy-list.net/uk-proxy.html");
            var HeaderNames = doc.DocumentNode.SelectNodes("@class=").ToList();
            foreach (var item in HeaderNames)
            {
                Console.WriteLine(item.InnerText);
            }
        }

        //TODO use a rotating proxy 
        public static JToken CollectData(string company, int ApiKeyNo)
        { 
            //The url that we are constructing 
            //https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=MSFT&interval=5min&apikey=Q4AZUW80DHGKUS3A
            
            using var wb = new WebClient();
            string APIresponse; 
            string[] APItoken = {"Q4AZUW80DHGKUS3A","7DHN0TYZEJN6K0AR","LRW6KJXN4UR6H5PE","KW6I0UQRPAUIFLUC","3HVVHJ55F87ZDG0A"};
            UriBuilder APIuribuild = new UriBuilder();//Setting up the UriBuilder
            APIuribuild.Scheme = "https";
            APIuribuild.Host = "www.alphavantage.co";
            APIuribuild.Path = $"/query";

            // check if run out of api keys
            try
            {
                APIuribuild.Query = $"function=TIME_SERIES_INTRADAY&symbol={company}&interval=5min&apikey={APItoken[ApiKeyNo]}";
            }
            catch
            {
                MessageBox.Show("Ran out fo API keys that work", "Error");
                return null;
            }

            Uri APIuri = APIuribuild.Uri;//Tells UriBuilder that all the URL parts are there
            Console.WriteLine(APIuri);

            // check api is responding
            try
            {
                APIresponse = wb.DownloadString(APIuri);
            }
            catch
            {
                Console.WriteLine("The API isn't calling me back :("); 
                return null;
            }
            Console.WriteLine(APIresponse);
            JObject responseJObject = JObject.Parse(APIresponse);
            //
            try
            {
                DateTime lastRefresh = (DateTime)responseJObject["Meta Data"]["3. Last Refreshed"];
                var values = responseJObject["Time Series (5min)"];
                return values;
            }
            catch
            {
                ApiCommunicator.CollectData(company, ApiKeyNo+1);
                return null;
            }
        }
    }
}
