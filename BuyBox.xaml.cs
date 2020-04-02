using System;
using System.Collections.Generic;
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
            var code = MainWindow.CurrentCode;
            var dataDictionary = Api.CollectDataSmall(code).ToObject<Dictionary<string, float>>();
            Current.Content = dataDictionary["c"];
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