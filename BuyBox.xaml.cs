using System;
using System.Windows;

namespace StockTrader_.NET_Framework_
{
    public partial class BuyBox
    {
        private readonly bool _IsBuyBox;

        public BuyBox(bool isBuyBox)
        {
            _IsBuyBox = isBuyBox;
            InitializeComponent();
            switch (_IsBuyBox)
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
            switch (_IsBuyBox)
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
