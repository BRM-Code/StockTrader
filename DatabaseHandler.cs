using System;
using MySql.Data.MySqlClient;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    public class DatabaseHandler
    {
        //Since we are using only one database, we can declare the MySqlConnection Object as a class wide variable
        private readonly MySqlConnection _sqlConnection = new MySqlConnection($"server={Startup.Settings.SQLServer};database={Startup.Settings.SQLDatabase};uid={Startup.Settings.SQLUser};password={Startup.Settings.SQLPassword}");
        private MySqlDataReader _dataReader;
        private Portfolio _portfolio;

        public Portfolio RetrievePortfolio()//Connects to the Database and gets the entry with the highest Id which is the latest
        {
            MessageBox.Show("Connecting to Database...", "Waiting");
            _sqlConnection.Open();
            try
            {
                MySqlCommand sqlCommand = new MySqlCommand("SELECT * FROM portfoliodb ORDER BY Id DESC LIMIT 1", _sqlConnection);
                _dataReader = sqlCommand.ExecuteReader();
                JObject portfolioJObject = (JObject)JToken.FromObject(_dataReader.GetValue(1));
                _portfolio = portfolioJObject.ToObject<Portfolio>();//casts the JObject to the portfolio class
            }
            catch
            {
                MessageBox.Show("Database empty, creating new Portfolio", "");
                _portfolio = new Portfolio();
            }
            _sqlConnection.Close();
            return _portfolio;
        }

        public void SavePortfolio(Portfolio userPortfolio)
        {
            DateTime currentDateTime = DateTime.Now;//This may get used in the future
            JObject portfolioJObject = (JObject)JToken.FromObject(userPortfolio);
            string query = "INSERT INTO portfoliodb (EntryDate, PortfolioObject) values('" + currentDateTime + "','" + portfolioJObject + "')";
            var command = new MySqlCommand(query, _sqlConnection);
            _sqlConnection.Open();
            command.ExecuteNonQuery();
            _sqlConnection.Close();
        }
    }
}