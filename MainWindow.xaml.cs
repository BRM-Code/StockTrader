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
        private readonly Startup _currentStartup;

        public MainWindow(Portfolio userPortfolio, Startup currentStartup)
        {
            _currentStartup = currentStartup;
            UserPortfolio = userPortfolio;
            StartTimer();
            InitializeComponent();
            if (!Startup.Settings.ExtremeData) return;
            ExtremeDataWarning.Visibility = Visibility.Visible;
            Nodatapointslider.Maximum = 1160;
            Nodatapointslider.TickFrequency = 100;
        }

        private void FindData(string code)
        {
            if (Nodatapointslider.Value == 0)return;
            var timeFrame = TimeframeComboBox.SelectionBoxItem.ToString();
            if ((timeFrame == "Weekly" || timeFrame == "Monthly") && (Startup.Settings.ExtremeData))
            {
                ExtremeDataWarning.Content = $"Warning:\n ExtremeData Mode doesn't work with {timeFrame}";
            }
            else
            {
                if (ExtremeDataWarning.Content != "Extreme data mode enabled")
                {
                    ExtremeDataWarning.Content = "Extreme data mode enabled";
                }
            }
            CurrentCompany = code;
            currentCompany.Content = code;
            currentPrice.Content = Api.FetchData(code,"1. open", timeFrame);
            highLabel.Content = Api.FetchData(code, "2. high", timeFrame);
            LowLabel.Content = Api.FetchData(code, "3. low", timeFrame);
            Volume.Content = Api.FetchData(code, "5. volume", timeFrame);
            GraphHandler lineGraph = new GraphHandler(linegraph);
            int nodatapoints = Convert.ToInt32(Nodatapointslider.Value);
            if (nodatapoints >= 100 && (timeFrame == "Weekly" || timeFrame == "Monthly"))
            {
                nodatapoints = 100;
            }
            var values = Api.CollectData(code, timeFrame);
            lineGraph.Draw(values,nodatapoints);
            DisplayGraph.BottomTitle = timeFrame;
        }

        private static void TraderButtonHandler(bool isBuyBox)
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
            FindData((string)((Button)sender).Tag);
        }

        private void ValueUpdater(object sender, EventArgs e)
        {
            avalibleFunds.Content = $"£{UserPortfolio.AvailableFunds}";
            accountValue.Content = $"£{UserPortfolio.CalculateTotalAccountValue()}";
        }

        private void OnClosing(object sender, EventArgs e)
        {
            Startup.UserPortfolio = UserPortfolio;
            _updateTimer.Dispose();
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(_currentStartup);
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
            DatabaseHandler database = new DatabaseHandler();
            database.SavePortfolio(UserPortfolio);
        }
    }
}