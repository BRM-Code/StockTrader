using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class ApiCommunicator
    {
        private Dictionary<string,string> proxyKeyPairDictionary = new Dictionary<string, string>(){{"Q4AZUW80DHGKUS3A","alpha.proxy.vh7.uk"},
            {"7DHN0TYZEJN6K0AR","bravo.proxy.vh7.uk"},{"LRW6KJXN4UR6H5PE", "charlie.proxy.vh7.uk" } };

        public static float CurrentPrice(string company)
        {
            JToken data = CollectData(company,0);
            float value = Convert.ToSingle(data[data.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]]["1. open"]);
            return value;
        }

        private static void GetResponse(Uri uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            WebProxy myproxy = new WebProxy("[your proxy address]", 443);
            myproxy.BypassProxyOnLocal = false;
            request.Proxy = myproxy;
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }

        private Uri CreateUrl(string company)
        {
            using var wb = new WebClient();
            string APIresponse;
            string[] APItoken = { "Q4AZUW80DHGKUS3A", "7DHN0TYZEJN6K0AR", "LRW6KJXN4UR6H5PE", "KW6I0UQRPAUIFLUC", "3HVVHJ55F87ZDG0A" };
            UriBuilder APIuribuild = new UriBuilder();//Setting up the UriBuilder
            APIuribuild.Host = "https://www.alphavantage.co/query";
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
            return APIuri;
        }

        public static JToken CollectData(string company)
        {
            ApiCommunicator newApiCommunicator = new ApiCommunicator();
            Uri url = newApiCommunicator.CreateUrl(company);
            
        }

        //TODO use a rotating proxy 
        public static JToken Somethingelse(string company, int ApiKeyNo)
        { 
            //The url that we are constructing 
            //https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=MSFT&interval=5min&apikey=Q4AZUW80DHGKUS3A
            
            using var wb = new WebClient();
            string APIresponse; 
            string[] APItoken = {"Q4AZUW80DHGKUS3A","7DHN0TYZEJN6K0AR","LRW6KJXN4UR6H5PE","KW6I0UQRPAUIFLUC","3HVVHJ55F87ZDG0A"};
            UriBuilder APIuribuild = new UriBuilder();//Setting up the UriBuilder
            APIuribuild.Host = "https://www.alphavantage.co/query";

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
                MessageBox.Show("We didn't get a response, please check your internet connection", "Error");
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
