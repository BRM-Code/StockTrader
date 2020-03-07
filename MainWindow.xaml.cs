using System;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace StockTrader_.NET_Framework_
{
    public partial class MainWindow
    {
        public static string CurrentCompany = "";
        public static Portfolio UserPortfolio; 
        private Timer _updateTimer;
        private readonly DatabaseHandler _database = new DatabaseHandler();

        public MainWindow()
        {
            UserPortfolio = _database.RetrievePortfolio();
            StartTimer();
            InitializeComponent();
        }

        private void ButtonHandler(object sender, RoutedEventArgs e)
        {
            FindData((string)((Button)sender).Tag);
        }

        private void FindData(string code)
        {
            CurrentCompany = code;
            var values = Api.CollectData(code);
            GraphHandler lineGraph = new GraphHandler(linegraph);
            lineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void BuyButton(object sender, RoutedEventArgs e)
        {
            TraderButtonHandler(true);
        }

        private void SellButton(object sender, RoutedEventArgs e)
        {
            TraderButtonHandler(false);
        }

        private void TraderButtonHandler(bool IsBuyBox)
        {
            if (CurrentCompany == "")
            {
                MessageBox.Show("No Company Selected!", "Error");
                return;
            }
            JToken token = Api.CollectData(CurrentCompany);
            BuyBox newBuyBox = new BuyBox(token, IsBuyBox);
            newBuyBox.Show();
        }

        private void StartTimer()
        {
            _updateTimer = new Timer();
            _updateTimer.Tick += ValueUpdater;
            _updateTimer.Interval = 12000;
            _updateTimer.Start();
        }

        private void ValueUpdater(object sender, EventArgs e)
        {
            avalibleFunds.Content = $"£{UserPortfolio.AvailableFunds}";
            accountValue.Content = $"£{UserPortfolio.CalculateTotalAccountValue()}";
        }

        private void OnClosing(object sender, EventArgs e)
        {
            _database.SavePortfolio(UserPortfolio);
            MessageBox.Show("User portfolio successfully uploaded!", "Success");
        }
    }
}
