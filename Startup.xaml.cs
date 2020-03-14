namespace StockTrader_.NET_Framework_
{
    public partial class Startup
    {
        public static Portfolio UserPortfolio;
        public DatabaseHandler Database = new DatabaseHandler();
        public static Settings Settings = new Settings();
        public Startup()
        {
            InitializeComponent();
            UserPortfolio = Database.RetrievePortfolio();
            MainWindow mainWindow = new MainWindow(UserPortfolio);
            mainWindow.Show();
            this.Hide();
        }
    }
}