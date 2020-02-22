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
        public void Buy(string company, int Shares,Portfolio userPortfolio)
        {
            float price = ApiCommunicator.CurrentPrice(company);
            JToken stuff = ApiCommunicator.CollectData(company);
            JObject info = stuff.ToObject<JObject>();
            StockStorage Buy = new StockStorage(company,Shares,price);
            userPortfolio.AvalibleFunds -= price*Shares;
            userPortfolio.ShareStockStorages.Add(company,Buy); //TODO potential problem here if the user has shares of that company
        }
    }

    class StockStorage
    {
        public string company;
        public int shares;
        public float priceBought;//used to calculate if the user has made a loss or not on the share

        public StockStorage(string Company, int Shares, float Price)
        {
            company = Company;
            shares = Shares;
            priceBought = Price;
        }
    }

    class Portfolio
    {
        public float AvalibleFunds;// how much money the account has available to spend on shares
        public Dictionary<string, StockStorage> ShareStockStorages;// string is the companies codes and a StockStorage instance

        public Portfolio()
        {
            AvalibleFunds = 50000;
        }

        public float CalculateTotalAccountValue()
        {
            float TotalAccountValue = 0;
            string[] keys = ShareStockStorages.Keys.ToArray(); //Gets a array of the company's codes that the user has invested in
            for (int i = 0; i < keys.Length;)
            {
                string company = keys[i]; //gets the next company's code
                TotalAccountValue = ApiCommunicator.CurrentPrice(company) + TotalAccountValue;
                i++;
            }
            return TotalAccountValue;
        }
    }
}
