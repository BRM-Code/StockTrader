using System;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace StockTrader_.NET_Framework_
{
    public partial class MainWindow : Window
    {
        public static string currentCompany = "";
        public static Portfolio UserPortfolio; 
        private Timer updateTimer;
        private readonly DatabaseHandler database = new DatabaseHandler();

        public MainWindow()
        {
            UserPortfolio = database.RetrievePortfolio();
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
                MessageBox.Show("No Company Selected!", "Error");
                return;
            }

            JToken token = ApiCommunicator.CollectData(currentCompany,0);
            BuyBox newBuyBox = new BuyBox(token);
            newBuyBox.Show();
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
            avalibleFunds.Content = $"£{UserPortfolio.AvailableFunds}";
            accountValue.Content = $"£{UserPortfolio.CalculateTotalAccountValue()}";
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            database.SavePortfolio(UserPortfolio);
            MessageBox.Show("User portfolio successfully uploaded!", "Success");
        }
    }
}
