using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    public partial class BuyBox
    {
        private bool _IsBuyBox;

        public BuyBox(JToken Datapoints, bool IsBuyBox)
        {
            _IsBuyBox = IsBuyBox;
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
            current.Content = Convert.ToSingle(Datapoints[Datapoints.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]]["1. open"]);
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
