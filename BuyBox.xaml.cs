using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    /// <summary>
    /// Interaction logic for BuyBox.xaml
    /// </summary>
    public partial class BuyBox : Window
    {
        public int noshares = 0;
        public bool complete = false;

        public BuyBox(JToken datapoints)
        {
            InitializeComponent();
            current.Content = Convert.ToSingle(datapoints[datapoints.ToObject<Dictionary<string, object>>().Keys.ToArray()[0]]["1. open"]);
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            noshares = Convert.ToInt32((SharesAmount.Text));
            complete = true;
            this.Close();
        }

    }
}
