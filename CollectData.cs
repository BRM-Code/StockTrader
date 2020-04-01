using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Debug.WriteLine("Started Collecting Data...");
            var url = CreateUrl(company,timeFrame);
            var valuesJToken = GetResponse(url,timeFrame);
            if (valuesJToken == null)
            {
                Debug.WriteLine("Got a null response, switching proxy...");
                ProxykeyCooldown(_activePairs[0]);
                _activePairs.Remove(_activePairs[0]);
                CollectData(company,timeFrame);
            }
            Debug.WriteLine("Data Collected");
            return valuesJToken;
        }

        private static JToken GetResponse(Uri uri,string timeFrame)
        {
            Debug.WriteLine("Fetching Response...");
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
            var wb = new WebClient();
            myProxy.Address = newUri;
            myProxy.Credentials = new NetworkCredential("proxy", "c4yDXnYsbD");
            wb.Proxy = myProxy;
            string apiResponse;
            try
            {
                Debug.WriteLine("Sending Request...");
                apiResponse = wb.DownloadString(uri);
                wb.Dispose();
            }
            catch
            {
                MessageBox.Show("No response from API, check connection", "Error");
                return null;
            }
            Debug.WriteLine("Request Received");
            var responseJObject = JObject.Parse(apiResponse);
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
            try
            {
                var valuesLength = values.ToObject<Dictionary<string, object>>().Keys.ToArray();
                Debug.WriteLine($"Returned JToken with {valuesLength.Length} values");
            }
            catch
            {
                Debug.WriteLine("Received null values");
            }
            return values;
        }
        
        private static Uri CreateUrl(string company,string timeFrame)
        {
            var build = new UriBuilder {Host = "www.alphavantage.co/query"}; //Setting up the UriBuilder
            var extraDataParameter = "";
            timeFrame = timeFrame switch
            {
                "IntraDay" => "TIME_SERIES_INTRADAY",
                "Daliy" => "TIME_SERIES_DAILY",
                "Weekly" => "TIME_SERIES_WEEKLY",
                "Monthly" => "TIME_SERIES_MONTHLY",
                _ => timeFrame
            };
            if (Startup.Settings.ExtremeData)
            {
                extraDataParameter = "&outputsize=full";
            }
            try { build.Query = $"function={timeFrame}&symbol={company}&interval=5min{extraDataParameter}&apikey={ProxyKeyPairDictionary.Keys.ToArray()[_activePairs[0]]}"; }
            catch
            {
                MessageBox.Show("Used available requests", "Error");
                return null;
            }
            var uri = build.Uri;//Tells UriBuilder that all the URL parts are there
            Debug.WriteLine(uri);
            return uri;
        }
        
        private static void ProxykeyCooldown(int removedPair)
        {
            Debug.WriteLine("Started Cooldown Timer...");
            _cooldownTimer = new Timer();
            _cooldownTimer.Tick += Refresh(removedPair);
            _cooldownTimer.Interval = 60000;
            _cooldownTimer.Start();
        }
        
        private static EventHandler Refresh(int removedPair)
        {
            Debug.WriteLine("Cooldown Timer Finished");
            _cooldownTimer.Stop();
            _cooldownTimer.Dispose();
            _activePairs.Add(removedPair);
            return null;
        }
    }
}