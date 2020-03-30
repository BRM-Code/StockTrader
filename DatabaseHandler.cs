using System;
using System.Windows;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    public class DatabaseHandler
    {
        //Since we are using only one database, we can declare the MySqlConnection Object as a class wide variable
        private readonly MySqlConnection _sqlConnection = new MySqlConnection($"server={Startup.Settings.SqlServer};database={Startup.Settings.SqlDatabase};uid={Startup.Settings.SqlUser};password={Startup.Settings.SqlPassword}");
        private MySqlDataReader _dataReader;
        private Portfolio _portfolio;

        public Portfolio RetrievePortfolio(Startup startup)//Connects to the Database and gets the entry with the highest Id which is the latest
        {
            startup.ConnLabel.Visibility = Visibility.Visible;
            try
            {
                _sqlConnection.Open();
            }
            catch
            {
                MessageBox.Show("Could not connect to Database", "Error");
            }
            try
            {
                var sqlCommand = new MySqlCommand("SELECT * FROM portfoliodb ORDER BY Id DESC LIMIT 1", _sqlConnection);
                _dataReader = sqlCommand.ExecuteReader();
                var portfolioJObject = (JObject)JToken.FromObject(_dataReader.GetValue(1));
                _portfolio = portfolioJObject.ToObject<Portfolio>();//casts the JObject to the portfolio class
            }
            catch
            {
                startup.ConnVerfLabel.Content = "Database empty, creating new Portfolio";
                startup.ConnVerfLabel.Visibility = Visibility.Visible;
                _portfolio = new Portfolio();
            }
            _sqlConnection.Close();
            _sqlConnection.Dispose();
            return _portfolio;
        }

        public void SavePortfolio(Portfolio userPortfolio)
        {
            var currentDateTime = DateTime.Now;//This may get used in the future
            var portfolioJObject = (JObject)JToken.FromObject(userPortfolio);
            var query = "INSERT INTO portfoliodb (EntryDate, PortfolioObject) values('" + currentDateTime + "','" + portfolioJObject + "')";
            var command = new MySqlCommand(query, _sqlConnection);
            _sqlConnection.Open();
            command.ExecuteNonQuery();
            _sqlConnection.Close();
            _sqlConnection.Dispose();
        }
    }
}