using System;
using System.Windows;
using System.Windows.Forms;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace StockTrader_.NET_Framework_
{
    public partial class MainWindow
    {
        public static string CurrentCompany = "";
        public static Portfolio UserPortfolio;
        private Timer _updateTimer;

        public MainWindow(Portfolio userPortfolio)
        {
            UserPortfolio = userPortfolio;
            StartTimer();
            InitializeComponent();
            if (!Startup.Settings.ExtremeData) return;
            extremeDataWarning.Visibility = Visibility.Visible;
            nodatapointslider.Maximum = 1160;
            nodatapointslider.TickFrequency = 100;
        }

        private void ButtonHandler(object sender, RoutedEventArgs e)
        {
            FindData((string)((Button)sender).Tag);
        }

        private void FindData(string code)
        {
            CurrentCompany = code; 
            string timeframe = TimeframeComboBox.SelectionBoxItem.ToString();
            var values = Api.CollectData(code,timeframe);
            currentCompany.Content = code;
            currentPrice.Content = Api.FetchData(code,"1. open", timeframe);
            highLabel.Content = Api.FetchData(code, "2. high", timeframe);
            lowLabel.Content = Api.FetchData(code, "3. low", timeframe);
            volume.Content = Api.FetchData(code, "5. volume", timeframe);
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

        private void TraderButtonHandler(bool isBuyBox)
        {
            if (CurrentCompany == "")
            {
                MessageBox.Show("No Company Selected!", "Error");
                return;
            }
            BuyBox newBuyBox = new BuyBox(isBuyBox);
            newBuyBox.Show();
        }

        private void StartTimer()
        {
            _updateTimer = new Timer();
            _updateTimer.Tick += ValueUpdater;
            _updateTimer.Interval = 60000;
            _updateTimer.Start();
        }

        private void ValueUpdater(object sender, EventArgs e)
        {
            avalibleFunds.Content = $"£{UserPortfolio.AvailableFunds}";
            accountValue.Content = $"£{UserPortfolio.CalculateTotalAccountValue()}";
        }

        private void OnClosing(object sender, EventArgs e)
        {
            Startup.UserPortfolio = UserPortfolio;
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
            this.Close();
        }

        private void ChangeGraphResolution(object sender, RoutedEventArgs e)
        {
            FindData(CurrentCompany);
        }
    }
}
