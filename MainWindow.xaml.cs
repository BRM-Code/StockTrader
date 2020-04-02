using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Debug = System.Diagnostics.Debug;

namespace StockTrader_.NET_Framework_
{
    public partial class MainWindow
    {
        public static string CurrentCode = "";
        public string CurrentName = "";
        public static float CurrentCompanyPrice;
        public string CurrentTimeFrame;
        public JToken CurrentCompanyJToken;
        public readonly Portfolio UserPortfolio;

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

        private async void FindData(string code,bool refresh)
        {
            Debug.WriteLine($"Request to Draw the Graph with {code} and refresh is {refresh}");
            //This checks that the user has given the required data 
            if ((Convert.ToInt32(Nodatapointslider.Value) == 0) || code == "") return;

            //Setup Variables For the Method
            var timeFrame = TimeframeComboBox.SelectionBoxItem.ToString();
            var noDataPoints = Convert.ToInt32(Nodatapointslider.Value);
            if (CurrentTimeFrame == null)
            {
                Debug.WriteLine("Timeframe was null");
                CurrentTimeFrame = timeFrame;
            }
            if (CurrentCode != code || !(refresh) || CurrentTimeFrame != timeFrame)
            {
                Debug.WriteLine("Fetching Data...");
                CurrentCompanyJToken = Api.CollectData(code, timeFrame);
                Debug.WriteLine("Data Received");
            }

            //Setup Variables that rely on The Collected data
            var keysArray = CurrentCompanyJToken.ToObject<Dictionary<string, object>>().Keys.ToArray();

            //Cleans up the input before drawing the graph
            //This makes sure the user hasn't asked for more data than the JToken can provide
            if (Convert.ToInt32(keysArray.Length) < Convert.ToInt32(Nodatapointslider.Value))
            {
                Debug.WriteLine("Lowering number of data points");
                Nodatapointslider.Value = keysArray.Length;
                noDataPoints = keysArray.Length;
            }
            //This stops the user asking for more data than the API can provide for Weekly and Monthly
            if (noDataPoints >= 240 & (timeFrame == "Weekly" || timeFrame == "Monthly"))
            {
                noDataPoints = 240;
            }
            //This draws the graphs
            await GraphHandler.Draw(CurrentCompanyJToken, noDataPoints, this);
            if (PredictionMethodComboBox.Text != "Off")
            {
                await GraphHandler.PredictionDraw(CurrentCompanyJToken, noDataPoints, this);
            }

            if ((timeFrame == "Weekly" || timeFrame == "Monthly") && (Startup.Settings.ExtremeData))
            {
                ExtremeDataWarning.Content = $"Warning:\n ExtremeData Mode doesn't work with {timeFrame}";
            }
            else {ExtremeDataWarning.Content = "Extreme data mode enabled";}

            //Adds values the labels to the right of the window
            plotter.BottomTitle = timeFrame;
            CurrentCode = code;
            CurrentTimeFrame = timeFrame;
            currentCompany.Content = CurrentName;
            CurrentCompanyPrice = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["1. open"]);
            CurrentPrice.Content = CurrentCompanyPrice;
            HighLabel.Content = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["2. high"]);
            LowLabel.Content = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["3. low"]);
            Volume.Content = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["5. volume"]);
        }

        private void TraderButtonHandler(bool isBuyBox)
        {
            if (CurrentCode == "")
            {
                System.Windows.MessageBox.Show("No Company Selected!", "Error");
                return;
            }
            var newBuyBox = new BuyBox(isBuyBox, this);
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
            CurrentName = ((System.Windows.Controls.Button)sender).Content.ToString();
            var code = (string) ((System.Windows.Controls.Button) sender).Tag; 
            FindData(code,false);
        }

        private void ValueUpdater(object sender, EventArgs e)
        {
            Debug.WriteLine("Updating Values...");
            AvailableFunds.Content = $"£{UserPortfolio.AvailableFunds}";
            AccountValue.Content = $"£{UserPortfolio.CalculateTotalAccountValue()}";
        }

        private void OnClosing(object sender, EventArgs e)
        {
            _currentStartup.UserPortfolio = UserPortfolio;
            _updateTimer.Dispose();
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Opening Settings...");
            var settingsWindow = new SettingsWindow(_currentStartup);
            settingsWindow.Show();
            Close();
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

        private void Refresh(object sender, RoutedEventArgs e)
        {
            FindData(CurrentCode, true);
        }

        private void TimeframeComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            if (TimeframeComboBox.Text == "Monthly" || TimeframeComboBox.Text == "Weekly")
            {
                Nodatapointslider.Maximum = 240;
            }
            else
            {
                Nodatapointslider.Maximum = 100;
            }
        }

        private void OpenPortfolioView(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Opening Portfolio View...");
            var window = new PortfolioView(_currentStartup);
            window.Show();
            Close();
        }
    }
}