using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        public float priceBought;

        public StockStorage(DateTime Time, string Company, int Shares, float Price)
        {
            time = Time;
            company = Company;
            shares = Shares;
            priceBought = Price;
        }
    }

    class Portfolio
    {
        public float AvalibleFunds;
        public Dictionary<string, StockStorage> ShareStockStorages;

        public float CalculateTotalAccountValue()
        {
            float TotalAccountValue = 0;
            string[] keys = ShareStockStorages.Keys.ToArray();
            for (int i = 0; i < keys.Length;)
            {
                string company = keys[i];
                TotalAccountValue = ApiCommunicator.CurrentPrice(company) + TotalAccountValue;
                i++;
            }
            return TotalAccountValue;
        }
    }
}
