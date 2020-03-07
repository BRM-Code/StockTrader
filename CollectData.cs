using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MessageBox = System.Windows.MessageBox;

namespace StockTrader_.NET_Framework_
{
    static class Api
    {
        private static readonly Dictionary<string, string> ProxyKeyPairDictionary = new Dictionary<string, string>
        {
            {"Q4AZUW80DHGKUS3A","alpha.proxy.vh7.uk"},
            {"7DHN0TYZEJN6K0AR","bravo.proxy.vh7.uk"},
            {"LRW6KJXN4UR6H5PE","charlie.proxy.vh7.uk"}
        };
        private static int _proxyKeyPairIndex;

        public static JToken CollectData(string company)
        {
            Uri url = Api.CreateUrl(company);
            JToken valuesJToken = Api.GetResponse(url);
            if (valuesJToken == null)
            {
                Console.WriteLine("Switching Proxy");
                _proxyKeyPairIndex++;
                Api.CollectData(company);
            }
            return valuesJToken;
        }

        public static float CurrentPrice(string company)
        {
            JToken data = CollectData(company);
            float value = Convert.ToSingle(data[data.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]]["1. open"]);
            return value;
        }

        public static JToken GetResponse(Uri uri)
        {
            WebProxy myProxy = new WebProxy();
            Uri newUri = new Uri($"http://{ProxyKeyPairDictionary.Values.ToArray()[_proxyKeyPairIndex]}");
            using var wb = new WebClient();
            myProxy.Address = newUri;
            myProxy.Credentials = new NetworkCredential("proxy", "c4yDXnYsbD");
            wb.Proxy = myProxy;
            string apIresponse;
            try
            { apIresponse = wb.DownloadString(uri); }
            catch
            {
                Console.WriteLine("The API isn't calling me back :(");
                return null;
            }
            JObject responseJObject = JObject.Parse(apIresponse);
            try
            {
                DateTime lastRefresh = (DateTime)responseJObject["Meta Data"]["3. Last Refreshed"];
            }
            catch
            {
                return null;
            }

            var values = responseJObject["Time Series (5min)"];
            return values;
        }

        private static Uri CreateUrl(string company)
        {
            using var wb = new WebClient();
            UriBuilder uribuild = new UriBuilder();//Setting up the UriBuilder
            uribuild.Host = "www.alphavantage.co/query";
            try { uribuild.Query = $"function=TIME_SERIES_INTRADAY&symbol={company}&interval=5min&apikey={ProxyKeyPairDictionary.Keys.ToArray()[_proxyKeyPairIndex]}"; }
            catch
            {
                MessageBox.Show("Ran out of API keys that work", "Error");
                return null;
            }
            Uri apIuri = uribuild.Uri;//Tells UriBuilder that all the URL parts are there
            Console.WriteLine(apIuri);
            return apIuri;
        }
    }
}