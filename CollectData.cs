using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StockTrader_.NET_Framework_
{
    class CollectData
    {
        public CollectData(double values, string company)
        {
            //https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=MSFT&interval=5min&apikey=demo

            using var wb = new WebClient();
                //string APItoken;
                string APItoken = "Q4AZUW80DHGKUS3A"; // The API token
                UriBuilder APIuribuild = new UriBuilder();//Setting up the UriBuilder
                APIuribuild.Scheme = "https";
                APIuribuild.Host = "www.alphavantage.co";
                APIuribuild.Path = $"/query";
                APIuribuild.Query = $"functions=TIME_SERIES_INTRADAY&symbol={company}&interval=5min&apikey={APItoken}";
                Uri APIuri = APIuribuild.Uri;//Tells UriBuilder that all the URL parts are there
                try
                {
                    var APIresponse = wb.DownloadString(APIuri);
                }
                catch
                { Console.WriteLine("The API isn't calling me back :("); }
        }
    }
}
