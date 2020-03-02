using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    static class ApiCommunicator
    {
        private static readonly Dictionary<string,string> proxyKeyPairDictionary = new Dictionary<string, string>
        {
            {"Q4AZUW80DHGKUS3A","alpha.proxy.vh7.uk"},
            {"7DHN0TYZEJN6K0AR","bravo.proxy.vh7.uk"},
            {"LRW6KJXN4UR6H5PE","charlie.proxy.vh7.uk" }
        };
        private static int _proxyKeyPairIndex = 0;

        public static JToken CollectData(string company)
        {
            Uri url = ApiCommunicator.CreateUrl(company);
            JToken valuesJToken = ApiCommunicator.GetResponse(url);
            if (valuesJToken == null)
            {
                _proxyKeyPairIndex++;
                ApiCommunicator.CollectData(company);
            }
            return valuesJToken;
        }

        public static float CurrentPrice(string company)
        {
            JToken data = CollectData(company);
            float value = Convert.ToSingle(data[data.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]]["1. open"]);
            return value;
        }

        private static JToken GetResponse(Uri uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            WebProxy myproxy = new WebProxy(proxyKeyPairDictionary.Values.ToArray()[_proxyKeyPairIndex], 443);
            myproxy.BypassProxyOnLocal = false;
            request.Proxy = myproxy;
            request.Method = "GET";
            request.Credentials = new NetworkCredential("proxy", "c4yDXnYsbD");
            HttpWebResponse response;
            try 
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
                MessageBox.Show("We didn't get a response, please check your internet connection", "Error");
                return null;
            }
            JObject responseJObject = JObject.Parse(response.ToString());
            try
            {
                DateTime lastRefresh = (DateTime)responseJObject["Meta Data"]["3. Last Refreshed"];
                var values = responseJObject["Time Series (5min)"];
                return values;
            }
            catch
            {
                return null;
            }
        }

        private static Uri CreateUrl(string company)
        {
            using var wb = new WebClient();
            UriBuilder uribuild = new UriBuilder();//Setting up the UriBuilder
            uribuild.Host = "https://www.alphavantage.co/query";
            try {uribuild.Query = $"function=TIME_SERIES_INTRADAY&symbol={company}&interval=5min&apikey={proxyKeyPairDictionary.Keys.ToArray()[_proxyKeyPairIndex]}";}
            catch
            {
                MessageBox.Show("Ran out of API keys that work", "Error");
                return null;
            }
            Uri APIuri = uribuild.Uri;//Tells UriBuilder that all the URL parts are there
            Console.WriteLine(APIuri);
            return APIuri;
        }
    }
}