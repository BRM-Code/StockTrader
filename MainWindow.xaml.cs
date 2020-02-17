using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InteractiveDataDisplay;
using InteractiveDataDisplay.WPF;
using Newtonsoft.Json.Linq;

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

        private void Alphabet(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("goog");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Microsoft(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("msft");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Apple(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("aapl");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Uber(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("uber");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Intel(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("intc");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void IBM(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("ibm");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Facebook(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("fb");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void WD(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("wdc");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Nvidia(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("nvda");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Oracle(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("orcl");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Amazon(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("amzn");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void AMD(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("amd");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Dell(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("dell");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Adobe(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("adbe");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void QinetiQ(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("qq");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }

        private void Spotify(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("spot");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);
        }
    }
}
