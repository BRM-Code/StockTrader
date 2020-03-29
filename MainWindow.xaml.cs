using System;
using System.Collections.Generic;
using System.Linq;
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
        public static JToken CurrentCompanyJToken;
        public static Portfolio UserPortfolio;
        public static float CurrentCompanyPrice;
        public string CurrentCompanyName = "";

        private Timer _updateTimer;
        private readonly Startup _currentStartup;

        public MainWindow(Portfolio userPortfolio, Startup currentStartup)
        {
            _currentStartup = currentStartup;
            UserPortfolio = userPortfolio;
            StartTimer();
            InitializeComponent();
            AvailableFunds.Content = $"£{UserPortfolio.AvailableFunds}";
            AccountValue.Content = $"£{UserPortfolio.CalculateTotalAccountValue()}";
            if (!Startup.Settings.ExtremeData) return;
            ExtremeDataWarning.Visibility = Visibility.Visible;
            Nodatapointslider.Maximum = 1160;
            Nodatapointslider.TickFrequency = 100;
        }

        private async void FindData(string code)
        {
            if (Convert.ToInt32(Nodatapointslider.Value) == 0)return;
            var timeFrame = TimeframeComboBox.SelectionBoxItem.ToString();
            if ((timeFrame == "Weekly" || timeFrame == "Monthly") && (Startup.Settings.ExtremeData))
            {
                ExtremeDataWarning.Content = $"Warning:\n ExtremeData Mode doesn't work with {timeFrame}";
            }
            else
            {
                if ((string) ExtremeDataWarning.Content != "Extreme data mode enabled")
                {
                    ExtremeDataWarning.Content = "Extreme data mode enabled";
                }
            }
            plotter.BottomTitle = timeFrame;
            CurrentCompany = code;
            currentCompany.Content = CurrentCompanyName;
            CurrentCompanyJToken = Api.CollectData(code, timeFrame);
            var keysArray = CurrentCompanyJToken.ToObject<Dictionary<string, object>>().Keys.ToArray();
            CurrentCompanyPrice = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["1. open"]);
            CurrentPrice.Content = CurrentCompanyPrice;
            HighLabel.Content = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["2. high"]);
            LowLabel.Content = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["3. low"]);
            Volume.Content = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["5. volume"]);
            if (Convert.ToInt32(keysArray.Length) <= Convert.ToInt32(Nodatapointslider.Value))
            {
                Nodatapointslider.Value = keysArray.Length;
            }
            var noDataPoints = Convert.ToInt32(Nodatapointslider.Value);
            if (noDataPoints >= 100 & (timeFrame == "Weekly" || timeFrame == "Monthly"))
            {
                 noDataPoints = 100;
            }
            var lineGraph = new GraphHandler();
            await lineGraph.Draw(CurrentCompanyJToken, noDataPoints, this);
            var predictionGraph = new GraphHandler();
            await predictionGraph.PredictionDraw(CurrentCompanyJToken, noDataPoints,this);
        }

        private static void TraderButtonHandler(bool isBuyBox)
        {
            if (CurrentCompany == "")
            {
                MessageBox.Show("No Company Selected!", "Error");
                return;
            }
            var newBuyBox = new BuyBox(isBuyBox);
            newBuyBox.Show();
        }

        private void StartTimer()
        {
            _updateTimer = new Timer();
            _updateTimer.Tick += ValueUpdater;
            _updateTimer.Interval = 60000;
            _updateTimer.Start();
        }

        private void BuyButton(object sender, RoutedEventArgs e)
        {
            TraderButtonHandler(true);
        }

        private void SellButton(object sender, RoutedEventArgs e)
        {
            TraderButtonHandler(false);
        }

        private void ButtonHandler(object sender, RoutedEventArgs e)
        {
            CurrentCompanyName = ((Button)sender).Content.ToString();
            FindData((string)((Button)sender).Tag);
        }

        private void ValueUpdater(object sender, EventArgs e)
        {
            AvailableFunds.Content = $"£{UserPortfolio.AvailableFunds}";
            AccountValue.Content = $"£{UserPortfolio.CalculateTotalAccountValue()}";
        }

        private void OnClosing(object sender, EventArgs e)
        {
            Startup.UserPortfolio = UserPortfolio;
            _updateTimer.Dispose();
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(_currentStartup);
            settingsWindow.Show();
            this.Close();
        }

        private void TimeframeComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            if (CurrentCompany == "")return;
            FindData(CurrentCompany);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            _currentStartup.Shutdown(UserPortfolio,this);
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            var database = new DatabaseHandler();
            database.SavePortfolio(UserPortfolio);
        }

    }
}