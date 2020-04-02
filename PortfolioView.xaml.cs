using System;
using System.Collections.Generic;
using System.Linq;

namespace StockTrader_.NET_Framework_
{
    public partial class PortfolioView
    {
        private readonly Startup _currentStartup;

        public PortfolioView(Startup currentStartup)
        {
            InitializeComponent();
            _currentStartup = currentStartup;
            var portfolio = currentStartup.UserPortfolio;
            AccountValueLabel.Content = portfolio.CalculateTotalAccountValue();
            AvailableFundsLabel.Content = portfolio.AvailableFunds;
            var keys = portfolio.SharesDictionary.Keys.ToArray();
            var values = portfolio.SharesDictionary.Values.ToArray();
            for (var i = 0; i < keys.Length;)
            {
                var code = keys[i];
                var shares = values[i].Shares;
                var priceBoughtFor = values[i].PriceBought * values[i].Shares;
                var data = Api.CollectDataSmall(keys[i]);
                var dataDictionary = data.ToObject<Dictionary<string, object>>();
                var currentValue = Convert.ToSingle(dataDictionary["c"]) * shares;
                Codes.Items.Add(code.ToUpper());
                Shares.Items.Add(shares);
                OgValue.Items.Add(priceBoughtFor);
                CurrentValue.Items.Add(currentValue);
                i++;
            }
        }

        private void OnClosing(object sender, EventArgs e)
        {
            var mainWindow = new MainWindow(_currentStartup.UserPortfolio, _currentStartup);
            mainWindow.Show();
        }
    }
}
