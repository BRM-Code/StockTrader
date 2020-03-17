using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

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
            ServerAddressEntry.Text = Startup.Settings.SQLServer;
            DatabaseEntry.Text = Startup.Settings.SQLDatabase;
            Username.Text = Startup.Settings.SQLUser;
        }

        private void ApplyButton(object sender, RoutedEventArgs e)
        {
            if (_isInvalidAddress)
            {
                return;
            }
            if (Password.Password != "")
            {
                Startup.Settings.SQLPassword = Password.Password;
            }
            Startup.Settings.SQLServer = ServerAddressEntry.Text;
            Startup.Settings.SQLDatabase = DatabaseEntry.Text;
            Startup.Settings.SQLUser = Username.Text;
            Startup.Settings.ExtremeData = (bool) ExtremeDataCheckBox.IsChecked;
            ApplyLabel.Visibility = Visibility.Visible;
        }

        private void OnClosing(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow(Startup.UserPortfolio, _currentStartup);
            mainWindow.Show();
        }

        private void ServerAddressEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(@"[-a - zA - Z0 - 9@:% _\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?");
            bool isMatch = regex.IsMatch(ServerAddressEntry.Text);
            if(isMatch)
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
