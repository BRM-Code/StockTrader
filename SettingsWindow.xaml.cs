using System;
using System.Windows;

namespace StockTrader_.NET_Framework_
{
    public partial class SettingsWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
            ServerAddressEntry.Text = Startup.Settings.SQLServer;
            DatabaseEntry.Text = Startup.Settings.SQLDatabase;
            Username.Text = Startup.Settings.SQLUser;
        }

        private void ApplyButton(object sender, RoutedEventArgs e)
        {
            if (Password.Password != "")
            {
                Startup.Settings.SQLPassword = Password.Password;
            }
            Startup.Settings.SQLServer = ServerAddressEntry.Text;
            Startup.Settings.SQLDatabase = DatabaseEntry.Text;
            Startup.Settings.SQLUser = Username.Text;
            ApplyLabel.Visibility = Visibility.Visible;
        }

        private void OnClosing(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow(Startup.UserPortfolio);
            mainWindow.Show();
        }
    }
}
