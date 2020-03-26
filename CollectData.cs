using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace StockTrader_.NET_Framework_
{
    internal static class Api
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
            var url = CreateUrl(company,timeFrame);
            var valuesJToken = GetResponse(url,timeFrame);
            if (valuesJToken == null)
            {
                ProxykeyCooldown(_activePairs[0]);
                _activePairs.Remove(_activePairs[0]);
                CollectData(company,timeFrame);
            }
            return valuesJToken;
        }

        private static JToken GetResponse(Uri uri,string timeFrame)
        {
            var myProxy = new WebProxy();
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
            try {apIresponse = wb.DownloadString(uri);}
            catch
            {
                MessageBox.Show("No response from API, check connection", "Error");
                return null;
            }
            var responseJObject = JObject.Parse(apIresponse);
            if (responseJObject.ContainsKey("Note"))
            {
                return null;
            }

            var jObjectName = timeFrame switch
            {
                "IntraDay" => "Time Series (5min)",
                "Daliy" => "Time Series (Daily)",
                "Weekly" => "Weekly Time Series",
                "Monthly" => "Monthly Time Series",
                _ => "Error"
            };
            var values = responseJObject[jObjectName];
            return values;
        }
        
        private static Uri CreateUrl(string company,string timeFrame)
        {
            using var wb = new WebClient();
            var build = new UriBuilder {Host = "www.alphavantage.co/query"}; //Setting up the UriBuilder
            var extraDataParameter = "";
            switch(timeFrame)
            {
                case "IntraDay":
                    timeFrame = "TIME_SERIES_INTRADAY";
                    break;
                case "Daliy":
                    timeFrame = "TIME_SERIES_DAILY";
                    break;
                case "Weekly":
                    timeFrame = "TIME_SERIES_WEEKLY";
                    break;
                case "Monthly":
                    timeFrame = "TIME_SERIES_MONTHLY";
                    break;
            }
            if (Startup.Settings.ExtremeData)
            {
                extraDataParameter = "&outputsize=full";
            }
            try { build.Query = $"function={timeFrame}&symbol={company}&interval=5min{extraDataParameter}&apikey={ProxyKeyPairDictionary.Keys.ToArray()[_activePairs[0]]}"; }
            catch
            {
                MessageBox.Show("Ran out of API keys that work", "Error");
                return null;
            }
            var uri = build.Uri;//Tells UriBuilder that all the URL parts are there
            Console.WriteLine(uri);
            return uri;
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
            _cooldownTimer.Dispose();
            _activePairs.Add(removedPair);
            return null;
        }
    }
}