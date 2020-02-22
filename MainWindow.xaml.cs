using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    public partial class MainWindow : Window
    {
        private string currentCompany = "";
        private Portfolio userPortfolio = new Portfolio();//TODO Create a system to sort out users portfolio/ Sort out Database

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonHandler(object sender, RoutedEventArgs e)
        {
            FindData((string)((Button)sender).Tag);
        }

        private void FindData(string code)
        {
            currentCompany = code;
            var values = ApiCommunicator.CollectData(code);
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void BuyButton(object sender, RoutedEventArgs e)
        {
            JToken Token = ApiCommunicator.CollectData(currentCompany);
            BuyBox newBuyBox = new BuyBox(Token);
            newBuyBox.Show();
            while (newBuyBox.complete == false) {}
            Trader newTrader = new Trader();
            newTrader.Buy(currentCompany, Convert.ToInt32(newBuyBox.SharesAmount), userPortfolio);
        }

        private void SellButton(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
