using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace StockTrader_.NET_Framework_
{
    public partial class SettingsWindow
    {
        private bool _isInvalidAddress;
        private readonly Startup _currentStartup;

        public SettingsWindow(Startup currentStartup)
        {
            _currentStartup = currentStartup;
            InitializeComponent();
            ExtremeDataCheckBox.IsChecked = Startup.Settings.ExtremeData;
            IndicatorCheckBox.IsChecked = Startup.Settings.Indicators;
            CurrentServer.Content = Startup.Settings.SqlServer;
            CurrentDatabase.Content = Startup.Settings.SqlDatabase;
            CurrentUser.Content = Startup.Settings.SqlUser;
        }

        private void ApplyButton(object sender, RoutedEventArgs e)
        {
            var settings = Startup.Settings;
            if (Password.Password != "") settings.SqlPassword = Password.Password;
            if (ServerAddressEntry.Text != "" && _isInvalidAddress == false) settings.SqlServer = ServerAddressEntry.Text; 
            if (DatabaseEntry.Text != "") settings.SqlDatabase = DatabaseEntry.Text; 
            if (Username.Text != "") settings.SqlUser = Username.Text;
            settings.ExtremeData = ExtremeDataCheckBox.IsChecked.Value;
            settings.Indicators = IndicatorCheckBox.IsChecked.Value;
            ApplyLabel.Visibility = Visibility.Visible;
            Close();
        }

        private void OnClosing(object sender, EventArgs e)
        {
            var mainWindow = new MainWindow(_currentStartup.UserPortfolio, _currentStartup);
            mainWindow.Show();
        }

        private void ServerAddressEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ServerAddressEntry.Text == "")return;
            var regex = new Regex(@"[-a - zA - Z0 - 9@:% _\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?");
            var isMatch = regex.IsMatch(ServerAddressEntry.Text);
            if (isMatch)
            {
                Invalidlabel.Visibility = Visibility.Hidden;
                _isInvalidAddress = false;
            }
            else
            {
                Invalidlabel.Visibility = Visibility.Visible;
                _isInvalidAddress = true;
            }
        }
    }
}
