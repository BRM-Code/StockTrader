using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            BoxTitle.Content = _isBuyBox switch
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
            var regex = new Regex(@"[^0-9]");
            if (regex.IsMatch(SharesAmount.Text))
            {
                MessageBox.Show("That's not a valid amount.", "Error");
                return;
            }
            var noShares = Convert.ToInt32(SharesAmount.Text);
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

        private void SharesAmount_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var input = e.Text;
            var pattern = "[^0-9]+";
            e.Handled = Regex.IsMatch(input,pattern);
        }
    }
}