using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace StockTrader_.NET_Framework_
{
    public partial class BuyBox
    {
        private readonly bool _isBuyBox;

        public BuyBox(bool isBuyBox)
        {
            _isBuyBox = isBuyBox;
            InitializeComponent();
            Title.Content = _isBuyBox switch
            {
                true => "Buy",
                false => "Sell"
            };
            var currentCompanyJToken = MainWindow.CurrentCompanyJToken;
            current.Content = Convert.ToSingle(currentCompanyJToken[currentCompanyJToken.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]]["1. open"]);
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            var noShares = Convert.ToInt32((SharesAmount.Text));
            switch (_isBuyBox)
            {
                case true:
                    Trader.Buy(MainWindow.CurrentCompany, Convert.ToInt32(noShares));
                    break;
                case false:
                    Trader.Sell(MainWindow.CurrentCompany, Convert.ToInt32(noShares));
                    break;
            }
            Close();
        }
    }
}
