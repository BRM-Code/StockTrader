using System;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class DatabaseHandler
    {
        private const string connectionString = "";

        public Portfolio RetrievePortfolio()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            string query = "SELECT * FROM portfoliodb ORDER BY id DESC LIMIT 1";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataReader dataReader = sqlCommand.ExecuteReader();
            string portfolio = dataReader.GetValue(1).ToString();

            sqlConnection.Close();
        }

        public void SavePortfolio(Portfolio userPortfolio)
        {
            DateTime currentDateTime = DateTime.Now;
            JObject portfolioJObject = (JObject)JToken.FromObject(userPortfolio);
            Console.WriteLine($"JSON about to be saved: ",portfolioJObject);
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            string query = "INSERT INTO portfoliodb (EntryDate, PortfolioObject) values('"+currentDateTime+"','"+portfolioJObject+"')";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
    }
}
