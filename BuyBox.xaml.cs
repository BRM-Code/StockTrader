using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    public partial class BuyBox : Window
    {
        public int noshares = 0;
        public bool complete = false;// used as a indicator to tell the main program when the user has entered the amount of shares

        public BuyBox(JToken datapoints)
        {
            InitializeComponent();
            // updates the window with the latest prices of the selected stock
            current.Content = Convert.ToSingle(datapoints[datapoints.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]]["1. open"]);
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            noshares = Convert.ToInt32((SharesAmount.Text));
            complete = true;
            this.Close();
        }
    }
}
