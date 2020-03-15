using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
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
        private static List<int> _activePairs = new List<int>(3){0,1,2};
        private static Timer _cooldownTimer;

        public static JToken CollectData(string company,string timeFrame)
        {
            Uri url = Api.CreateUrl(company,timeFrame);
            JToken valuesJToken = Api.GetResponse(url,timeFrame);
            if (valuesJToken == null)
            {
                Console.WriteLine(@"Switching Proxy");
                ProxykeyCooldown(_activePairs[0]);
                _activePairs.Remove(_activePairs[0]);
                Api.CollectData(company,timeFrame);
            }
            return valuesJToken;
        }

        public static float FetchData(string company,string value,string timeFrame)
        {
            JToken data = CollectData(company,timeFrame);
            var dataitem = Convert.ToSingle(data[data.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]][value]);
            return dataitem;
        }

        private static JToken GetResponse(Uri uri,string timeFrame)
        {
            WebProxy myProxy = new WebProxy();
            Uri newUri = null;
            try
            {
                newUri = new Uri($"http://{ProxyKeyPairDictionary.Values.ToArray()[_activePairs[0]]}");
            }
            catch
            {
                MessageBox.Show("Used the 15 requests per minute", "Error");
            }
            using var wb = new WebClient();
            myProxy.Address = newUri;
            myProxy.Credentials = new NetworkCredential("proxy", "c4yDXnYsbD");
            wb.Proxy = myProxy;
            string apIresponse;
            try
            { apIresponse = wb.DownloadString(uri); }
            catch
            {
                MessageBox.Show("No response from API, check connection", "Error");
                return null;
            }
            JObject responseJObject = JObject.Parse(apIresponse);
            if (responseJObject.ContainsKey("Note"))
            {
                return null;
            }
            string jObjectName = "";
            switch (timeFrame)
            {
                case "IntraDay":
                    jObjectName = "Time Series (5min)";
                    break;
                case "Daliy":
                    jObjectName = "Time Series (Daily)";
                    break;
                case "Weekly":
                    jObjectName = "Weekly Time Series";
                    break;
                case "Monthly":
                    jObjectName = "Monthly Time Series";
                    break;
            }
            var values = responseJObject[jObjectName];
            return values;
        }
        
        private static Uri CreateUrl(string company,string timeFrame)
        {
            using var wb = new WebClient();
            UriBuilder uribuild = new UriBuilder();//Setting up the UriBuilder
            uribuild.Host = "www.alphavantage.co/query";
            string extraDataParameter = "";
            string apiFunction = "";
            switch(timeFrame)
            {
                case "IntraDay":
                    apiFunction = "TIME_SERIES_INTRADAY";
                    break;
                case "Daliy":
                    apiFunction = "TIME_SERIES_DAILY";
                    break;
                case "Weekly":
                    apiFunction = "TIME_SERIES_WEEKLY";
                    break;
                case "Monthly":
                    apiFunction = "TIME_SERIES_MONTHLY";
                    break;
            }
            if (Startup.Settings.ExtremeData)
            {
                extraDataParameter = "&outputsize=full";
            }
            try { uribuild.Query = $"function={apiFunction}&symbol={company}&interval=5min{extraDataParameter}&apikey={ProxyKeyPairDictionary.Keys.ToArray()[_activePairs[0]]}"; }
            catch
            {
                MessageBox.Show("Ran out of API keys that work", "Error");
                return null;
            }
            Uri apIuri = uribuild.Uri;//Tells UriBuilder that all the URL parts are there
            Console.WriteLine(apIuri);
            return apIuri;
        }
        
        private static void ProxykeyCooldown(int removedPair)
        {
            _cooldownTimer = new Timer();
            _cooldownTimer.Tick += Refresh(removedPair);
            _cooldownTimer.Interval = 60000;
            _cooldownTimer.Start();
        }
        
        private static EventHandler Refresh(int removedPair)
        {
            _cooldownTimer.Stop();
            _activePairs.Add(removedPair);
            return null;
        }
    }
}