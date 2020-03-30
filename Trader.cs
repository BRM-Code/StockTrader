using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace StockTrader_.NET_Framework_
{
    public class StockStorage
    {
        public string Company;
        public int Shares;
        public float PriceBought;//used to calculate if the user has made a loss or not on the share

        public StockStorage(string company, int shares, float price)
        {
            Company = company;
            Shares = shares;
            PriceBought = price;
        }
    }

    public class Portfolio
    {
        public float AvailableFunds;// how much money the account has available to spend on shares
        private Dictionary<string, StockStorage> SharesDictionary;// string is the companies codes and a StockStorage instance

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
            for (var i = 0; i < keys.Length;)
            {
                totalAccountValue = MainWindow.CurrentCompanyPrice * shares[i].Shares + totalAccountValue;
                i++;
            }
            return totalAccountValue;
        }

        public void Buy(string company, int shares)
        {
            if (SharesDictionary.ContainsKey(company))//Checks if the user has any shares of that company
            {
                SharesDictionary[company].Shares += shares;
                AvailableFunds -= MainWindow.CurrentCompanyPrice * shares;
                return;
            }
            var buy = new StockStorage(company, shares, MainWindow.CurrentCompanyPrice);
            AvailableFunds -= MainWindow.CurrentCompanyPrice * shares;
            SharesDictionary.Add(company, buy);
        }

        public void Sell(string company, int shares)
        {
            if (!SharesDictionary.ContainsKey(company))//Checks if the user has any shares of that company
            {
                MessageBox.Show("You don't own any stocks from that company", "Error");
                return;
            }

            if (SharesDictionary[company].Shares < shares)//Checks that the user isn't trying to sell more stocks than they own
            {
                MessageBox.Show("Trying to sell more shares than in your possession", "Error");
                return;
            }
            SharesDictionary[company].Shares -= shares;//removes the number of shares from that companies in the dictionary
            AvailableFunds += MainWindow.CurrentCompanyPrice * shares;//add the value of the sold stocks to the users available funds
        }
    }
}