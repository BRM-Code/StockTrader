using System;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Button = System.Windows.Controls.Button;

namespace StockTrader_.NET_Framework_
{
    public partial class MainWindow : Window
    {
        private string currentCompany = "";
        private Portfolio userPortfolio;    //TODO Create a system to sort out users portfolio/ Sort out Database
        private Timer updateTimer;

        public MainWindow()
        {
            userPortfolio = new Portfolio();
            StartTimer();
            InitializeComponent();
            ApiCommunicator.RotateProxy();
        }

        private void ButtonHandler(object sender, RoutedEventArgs e)
        {
            FindData((string)((Button)sender).Tag);
        }

        private void FindData(string code)
        {
            currentCompany = code;
            var values = ApiCommunicator.CollectData(code,0);
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void BuyButton(object sender, RoutedEventArgs e)
        {
            if (currentCompany == "")
            {
                return;
            }

            JToken token = ApiCommunicator.CollectData(currentCompany,0);
            BuyBox newBuyBox = new BuyBox(token);
            newBuyBox.Show();
            while (newBuyBox.complete == false) {}
            Trader newTrader = new Trader();
            newTrader.Buy(currentCompany, Convert.ToInt32(newBuyBox.SharesAmount), userPortfolio);
        }

        private void SellButton(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StartTimer()
        {
            updateTimer = new Timer();
            updateTimer.Tick += new EventHandler(updateTimer_Tick);
            updateTimer.Interval = 2000;
            updateTimer.Start();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            avalibleFunds.Content = $"£{userPortfolio.AvailableFunds}";
            accountValue.Content = $"£{userPortfolio.CalculateTotalAccountValue()}";
        }
    }
}
