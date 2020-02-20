using System;
using System.Windows;

namespace StockTrader_.NET_Framework_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonHandler(object sender, RoutedEventArgs e)
        {
            switch (sender.ToString())
            {
                case "System.Windows.Controls.Button: Apple": FindData("aapl"); break;
                case "System.Windows.Controls.Button: Microsoft": FindData("msft"); break;
                case "System.Windows.Controls.Button: Alphabet": FindData("goog"); break;
                case "System.Windows.Controls.Button: Uber": FindData("uber"); break;
                case "System.Windows.Controls.Button: Intel": FindData("intc"); break;
                case "System.Windows.Controls.Button: IBM": FindData("ibm"); break;
                case "System.Windows.Controls.Button: Facebook": FindData("fb"); break;
                case "System.Windows.Controls.Button: WD": FindData("wdc"); break;
                case "System.Windows.Controls.Button: Nvidia": FindData("nvda"); break;
                case "System.Windows.Controls.Button: Oracle": FindData("orcl"); break;
                case "System.Windows.Controls.Button: Amazon": FindData("amzn"); break;
                case "System.Windows.Controls.Button: AMD": FindData("amd"); break;
                case "System.Windows.Controls.Button: Dell": FindData("dell"); break;
                case "System.Windows.Controls.Button: Adobe": FindData("adbe"); break;
                case "System.Windows.Controls.Button: QinetiQ": FindData("qq"); break;
                case "System.Windows.Controls.Button: Spotify": FindData("spot"); break;
            }
        }

        private void FindData(string code) { 
            var values = ApiCommunicator.CollectData(code);
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value)); }

        private void BuyButton(object sender, RoutedEventArgs e)
        {
            BuyBox win2 = new BuyBox();
            win2.Show();
        }

        private void SellButton(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
