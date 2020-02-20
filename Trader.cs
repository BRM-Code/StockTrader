using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteractiveDataDisplay.WPF;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class Trader
    {
        public void Buy(string company, int Shares)
        {
            float price = ApiCommunicator.CurrentPrice(company);
            JToken stuff = ApiCommunicator.CollectData(company);
            JObject info = stuff.ToObject<JObject>();
            DateTime lastRefresh = (DateTime)info["Meta Data"]["3. Last Refreshed"];
            StockStorage Buy = new StockStorage(lastRefresh,company,Shares,price);
        }
    }

    class StockStorage
    {
        public DateTime time;
        public string company;
        public int shares;
        public float price;

        public StockStorage(DateTime Time, string Company, int Shares, float Price)
        {
            time = Time;
            company = Company;
            shares = Shares;
            price = Price;
        }
    }
}
