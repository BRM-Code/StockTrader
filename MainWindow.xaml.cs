﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using InteractiveDataDisplay.WPF;
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
        public LineGraph LineGraph;

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
            CurrentCompany = code;
            currentCompany.Content = code;
            CurrentCompanyJToken = Api.CollectData(code, timeFrame);
            var keysArray = CurrentCompanyJToken.ToObject<Dictionary<string, object>>().Keys.ToArray();
            CurrentCompanyPrice = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["1. open"]);
            CurrentPrice.Content = CurrentCompanyPrice;
            HighLabel.Content = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["2. high"]);
            LowLabel.Content = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["3. low"]);
            Volume.Content = Convert.ToSingle(CurrentCompanyJToken[keysArray[0]]["5. volume"]);
            if (Convert.ToInt32(keysArray.Length) != Convert.ToInt32(Nodatapointslider.Value))
            {
                Nodatapointslider.Value = keysArray.Length;
            }
            var lineGraph = new GraphHandler(Linegraph);
            var noDataPoints = Convert.ToInt32(Nodatapointslider.Value);
            if (noDataPoints >= 100 && (timeFrame == "Weekly" || timeFrame == "Monthly"))noDataPoints = 100;
            await lineGraph.Draw(CurrentCompanyJToken, noDataPoints);
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
            var database = new DatabaseHandler();
            database.SavePortfolio(UserPortfolio);
        }
    }
}