using System.Windows;

namespace StockTrader_.NET_Framework_
{
    public partial class AutoTrader
    {
        public AutoTrader()
        {
            InitializeComponent();
            for (var i = 0; i < MainWindow.codes.Length;)
            {
                ComboBox.Items.Add(MainWindow.codes[i]);
                i++;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            float minimumPrice = 0;
            try
            {
                minimumPrice = float.Parse(value.Text);//input checking
            }
            catch
            {
                MessageBox.Show("That's not a number!", "Error");
            }
            var code = ComboBox.Text;
            var dictionary = Startup.Settings.AutoTradeRules;
            dictionary.Add(code,minimumPrice);
            Close();
        }
    }
}
