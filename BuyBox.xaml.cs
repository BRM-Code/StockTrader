using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace StockTrader_.NET_Framework_
{
    public partial class BuyBox
    {
        private readonly bool _isBuyBox;
        private readonly MainWindow _currentMainWindow;

        public BuyBox(bool isBuyBox, MainWindow mainWindow)
        {
            _currentMainWindow = mainWindow;
            _isBuyBox = isBuyBox;
            InitializeComponent();
            Title.Content = _isBuyBox switch
            {
                true => "Buy",
                false => "Sell"
            };
            var currentCompanyJToken = mainWindow.CurrentCompanyJToken;
            Current.Content = Convert.ToSingle(currentCompanyJToken[currentCompanyJToken.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]]["1. open"]);
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            var noShares = Convert.ToInt32((SharesAmount.Text));
            switch (_isBuyBox)
            {
                case true:
                    _currentMainWindow.UserPortfolio.Buy(MainWindow.CurrentCode, Convert.ToInt32(noShares));
                    break;
                case false:
                    _currentMainWindow.UserPortfolio.Sell(MainWindow.CurrentCode, Convert.ToInt32(noShares));
                    break;
            }
            Close();
        }
    }
}