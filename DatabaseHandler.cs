using System;
//using System.Data.MySqlClient;
using MySql.Data.MySqlClient;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class DatabaseHandler
    {
        //Since we are using only one database, we can declare the MySqlConnection Object as a class wide variable
        private readonly MySqlConnection _sqlConnection = new MySqlConnection("server=db.jakewalker.xyz;database=benrm1;uid=benrm;password=tiWuSIMo4IBo");
        private MySqlDataReader _dataReader;
        private Portfolio portfolio;

        public Portfolio RetrievePortfolio()//Connects to the Database and gets the entry with the highest Id which is the latest
        {
            MessageBox.Show("Connecting to Database...", "Waiting");
            _sqlConnection.Open();
            try
            {
                MySqlCommand sqlCommand = new MySqlCommand("SELECT * FROM portfoliodb ORDER BY Id DESC LIMIT 1", _sqlConnection);
                _dataReader = sqlCommand.ExecuteReader();
                _sqlConnection.Close();
                JObject portfolioJObject = (JObject)JToken.FromObject(_dataReader.GetValue(1));
                portfolio = portfolioJObject.ToObject<Portfolio>();//casts the JObject to the portfolio class
            }
            catch
            {
                _sqlConnection.Close();
                MessageBox.Show("Database empty, creating new Portfolio", "");
                portfolio = new Portfolio();
            }
            return portfolio;
        }

        public void SavePortfolio(Portfolio userPortfolio)
        {
            DateTime currentDateTime = DateTime.Now;//This may get used in the future
            JObject portfolioJObject = (JObject)JToken.FromObject(userPortfolio);
            string query = "INSERT INTO portfoliodb (EntryDate, PortfolioObject) values('" + currentDateTime + "','" + portfolioJObject + "')";
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            _sqlConnection.Open();
            adapter.InsertCommand = new MySqlCommand(query, _sqlConnection);
            _sqlConnection.Close();
        }
    }
}