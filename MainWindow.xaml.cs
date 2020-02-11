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
            var values = ApiCommunicator.CollectData("aapl");
            GraphHandler LineGraph = new GraphHandler(linegraph);
            LineGraph.Draw(values);

        }

        private void Alphabet(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Microsoft(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Apple(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Uber(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Intel(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void IBM(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Facebook(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void WD(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Nvidia(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Oracle(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Amazon(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AMD(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Dell(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Adobe(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void QinetiQ(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Spotify(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
