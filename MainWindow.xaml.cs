using InteractiveDataDisplay.WPF;
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

        private void Alphabet(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("goog");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Microsoft(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("msft");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Apple(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("aapl");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Uber(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("uber");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Intel(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("intc");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void IBM(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("ibm");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Facebook(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("fb");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void WD(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("wdc");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Nvidia(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("nvda");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Oracle(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("orcl");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Amazon(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("amzn");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void AMD(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("amd");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Dell(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("dell");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Adobe(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("adbe");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void QinetiQ(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("qq");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }

        private void Spotify(object sender, RoutedEventArgs e)
        {
            var values = ApiCommunicator.CollectData("spot");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values, Convert.ToInt32(nodatapointslider.Value));
        }
    }
}
