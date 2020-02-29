using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class Trader
    {
        public void Buy(string company, int Shares,Portfolio userPortfolio)
        {
            float price = ApiCommunicator.CurrentPrice(company);
            JToken stuff = ApiCommunicator.CollectData(company,0);
            JObject info = stuff.ToObject<JObject>();
            StockStorage Buy = new StockStorage(company,Shares,price);
            userPortfolio.AvailableFunds -= price*Shares;
            userPortfolio.SharesDictionary.Add(company,Buy); //TODO potential problem here if the user has shares of that company
        }
    }

    public class StockStorage
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

    public class Portfolio
    {
        public float AvailableFunds;// how much money the account has available to spend on shares
        public Dictionary<string, StockStorage> SharesDictionary;// string is the companies codes and a StockStorage instance

        public Portfolio()
        {
            AvailableFunds = 50000;
            SharesDictionary = new Dictionary<string, StockStorage>();
        }

        public float CalculateTotalAccountValue()
        {
            float TotalAccountValue = 0;
            string[] keys = SharesDictionary.Keys.ToArray(); //Gets a array of the company's codes that the user has invested in
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
