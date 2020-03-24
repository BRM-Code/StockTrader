﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace StockTrader_.NET_Framework_
{
    public static class Trader
    {
        public static void Buy(string company, int shares)
        {
            if (MainWindow.UserPortfolio.SharesDictionary.ContainsKey(company))//Checks if the user has any shares of that company
            {
                MainWindow.UserPortfolio.SharesDictionary[company].Shares += shares;
                MainWindow.UserPortfolio.AvailableFunds -= MainWindow.CurrentCompanyPrice * shares;
                return;
            }
            var buy = new StockStorage(company,shares, MainWindow.CurrentCompanyPrice);
            MainWindow.UserPortfolio.AvailableFunds -= MainWindow.CurrentCompanyPrice * shares;
            MainWindow.UserPortfolio.SharesDictionary.Add(company,buy);
        }

        public static void Sell(string company, int shares)
        {
            if (!MainWindow.UserPortfolio.SharesDictionary.ContainsKey(company))//Checks if the user has any shares of that company
            {
                MessageBox.Show("You don't own any stocks from that company", "Error");
                return;
            }

            if (MainWindow.UserPortfolio.SharesDictionary[company].Shares < shares)//Checks that the user isn't trying to sell more stocks than they own
            {
                MessageBox.Show("Trying to sell more shares than in your possession", "Error");
                return;
            }
            MainWindow.UserPortfolio.SharesDictionary[company].Shares -= shares;//removes the number of shares from that companies in the dictionary
            MainWindow.UserPortfolio.AvailableFunds += MainWindow.CurrentCompanyPrice * shares;//add the value of the sold stocks to the users available funds
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
                totalAccountValue = MainWindow.CurrentCompanyPrice * shares[i].Shares + totalAccountValue;
                i++;
            }
            return totalAccountValue;
        }
    }
}
