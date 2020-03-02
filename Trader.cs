using System.Collections.Generic;
using System.Linq;

namespace StockTrader_.NET_Framework_
{
    class Trader
    {
        public void Buy(string company, int shares,Portfolio userPortfolio)
        {
            float price = Api.CurrentPrice(company);
            StockStorage buy = new StockStorage(company,shares,price);
            userPortfolio.AvailableFunds -= price*shares;
            userPortfolio.SharesDictionary.Add(company,buy); //TODO potential problem here if the user has shares of that company
        }
    }

    public class StockStorage
    {
        public string Company;
        public int Shares;
        public float PriceBought;//used to calculate if the user has made a loss or not on the share

        public StockStorage(string company, int shares, float price)
        {
            this.Company = company;
            this.Shares = shares;
            PriceBought = price;
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
            float totalAccountValue = 0;
            string[] keys = SharesDictionary.Keys.ToArray();
            StockStorage[] shares = SharesDictionary.Values.ToArray();
            for (int i = 0; i < keys.Length;)
            {
                totalAccountValue = Api.CurrentPrice(keys[i]) *shares[i].Shares + totalAccountValue;
                i++;
            }
            return totalAccountValue;
        }
    }
}
