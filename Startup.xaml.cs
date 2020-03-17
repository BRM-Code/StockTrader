namespace StockTrader_.NET_Framework_
{
    public partial class Startup
    {
        public static Portfolio UserPortfolio;
        private readonly DatabaseHandler _database = new DatabaseHandler();
        public static readonly Settings Settings = new Settings();
        public Startup()
        {
            InitializeComponent();
            UserPortfolio = _database.RetrievePortfolio();
            MainWindow mainWindow = new MainWindow(UserPortfolio,this);
            mainWindow.Show();
            this.Hide();
        }

        public void Shutdown(Portfolio userPortfolio, MainWindow currentMainWindow)
        {
            _database.SavePortfolio(userPortfolio);
            currentMainWindow.Close();
            Close();
        }
    }
}