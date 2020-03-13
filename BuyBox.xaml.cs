using System;
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
            switch (_isBuyBox)
            {
                case true:
                    Title.Content = "Buy";
                    break;
                case false:
                    Title.Content = "Sell";
                    break;
            }
            current.Content = Api.CurrentPrice(MainWindow.CurrentCompany);
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            int noshares = Convert.ToInt32((SharesAmount.Text));
            Trader newTrader = new Trader();
            switch (_isBuyBox)
            {
                case true:
                    newTrader.Buy(MainWindow.CurrentCompany, Convert.ToInt32(noshares), MainWindow.UserPortfolio);
                    break;
                case false:
                    newTrader.Sell(MainWindow.CurrentCompany, Convert.ToInt32(noshares), MainWindow.UserPortfolio);
                    break;
            }
            this.Close();
        }
    }
}
