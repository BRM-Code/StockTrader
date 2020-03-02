using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    public partial class BuyBox
    {
        public BuyBox(JToken datapoints)
        {
            InitializeComponent();
            current.Content = Convert.ToSingle(datapoints[datapoints.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]]["1. open"]);
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            int noshares = Convert.ToInt32((SharesAmount.Text));
            Trader newTrader = new Trader();
            newTrader.Buy(MainWindow.CurrentCompany, Convert.ToInt32(noshares), MainWindow.UserPortfolio);
            this.Close();
        }
    }
}
