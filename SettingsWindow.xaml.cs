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
            currentServer.Content = Startup.Settings.SQLServer;
            currentDatabase.Content = Startup.Settings.SQLDatabase;
            currentUser.Content = Startup.Settings.SQLUser;
        }

        private void ApplyButton(object sender, RoutedEventArgs e)
        {
            if (Password.Password != "") Startup.Settings.SQLPassword = Password.Password;
            if (ServerAddressEntry.Text != "" && _isInvalidAddress == false) Startup.Settings.SQLServer = ServerAddressEntry.Text; 
            if (DatabaseEntry.Text != "") Startup.Settings.SQLDatabase = DatabaseEntry.Text; 
            if (Username.Text != "") Startup.Settings.SQLUser = Username.Text;
            Startup.Settings.ExtremeData = ExtremeDataCheckBox.IsChecked.Value;
            ApplyLabel.Visibility = Visibility.Visible;
            this.Close();
        }

        private void OnClosing(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow(Startup.UserPortfolio, _currentStartup);
            mainWindow.Show();
        }

        private void ServerAddressEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ServerAddressEntry.Text == "")return;
            Regex regex = new Regex(@"[-a - zA - Z0 - 9@:% _\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?");
            bool isMatch = regex.IsMatch(ServerAddressEntry.Text);
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
