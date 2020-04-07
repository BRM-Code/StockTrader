using System.Windows;

namespace StockTrader_.NET_Framework_
{
    public partial class AutoTrader
    {
        public AutoTrader()
        {
            InitializeComponent();

            //Populate the ComboBox with the company codes
            for (var i = 0; i < MainWindow.Codes.Length;)
            {
                ComboBox.Items.Add(MainWindow.Codes[i]);
                i++;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            float minimumPrice = 0;
            //Checking that the input is a floating point number
            try
            {
                minimumPrice = float.Parse(value.Text);
            }
            catch
            {
                MessageBox.Show("That's not a number!", "Error");
            }

            //Adds new rule to the Dictionary found in the Settings
            var code = ComboBox.Text;
            var dictionary = Startup.Settings.AutoTradeRules;
            dictionary.Add(code,minimumPrice);
            Close();
        }
    }
}