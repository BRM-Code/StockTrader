using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace StockTrader_.NET_Framework_
{
    public partial class Startup
    {
        public Portfolio UserPortfolio;
        public static Settings Settings;
        private readonly string _path = Directory.GetCurrentDirectory();
        private readonly DatabaseHandler _database;

        public Startup()
        {
            //Start this class
            InitializeComponent();
            this.Show();

            //Fetching Settings from file
            try
            {
                var text = File.ReadAllText(_path + @"\Settings.txt");
                Settings = JsonConvert.DeserializeObject<Settings>(text);
                Debug.WriteLine("Settings file loaded");
            }
            catch
            {
                Debug.WriteLine("Couldn't find the Settings file");
                Settings = new Settings {AutoTradeRules = new Dictionary<string, float>()};
            }

            //Fetching Portfolio from Database
            _database = new DatabaseHandler();
            UserPortfolio = _database.RetrievePortfolio(this);

            //Give the user a indication of whats happening
            StartLabel.Visibility = Visibility.Visible;

            //Pass the user to the main window
            var mainWindow = new MainWindow(UserPortfolio,this);
            mainWindow.Show();
            this.Hide();
        }

        public void Shutdown(Portfolio userPortfolio, MainWindow currentMainWindow)
        {
            //Save Portfolio to Database
            _database.SavePortfolio(userPortfolio);

            //Save Settings to File
            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(_path + @"\Settings.txt"))
            {
                using var writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, Settings);
            }

            //Close everything
            currentMainWindow.Close();
            Close();
        }
    }
}