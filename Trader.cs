using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class Trader
    {
        public void Buy(string company)
        {
            float price = ApiCommunicator.CurrentPrice(company);
            JToken stuff = ApiCommunicator.CollectData(company);
            JObject info = stuff.ToObject<JObject>();
            DateTime lastRefresh = (DateTime)info["Meta Data"]["3. Last Refreshed"];

        }
    }
}
