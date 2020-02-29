using System;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class DatabaseHandler
    {
        //Since we are using only one database, we can declare the SqlConnection Object as a class wide variable
        private readonly SqlConnection _sqlConnection = new SqlConnection("server=db.jakewalker.xyz;database=benrm1;user=benrm;password=tiWuSIMo4IBo");

        public Portfolio RetrievePortfolio()//Connects to the Database and gets the entry with the highest Id which is the latest
        {
            _sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM portfoliodb ORDER BY Id DESC LIMIT 1", _sqlConnection);
            SqlDataReader dataReader = sqlCommand.ExecuteReader();
            _sqlConnection.Close();
            
            JObject portfolioJObject = (JObject)JToken.FromObject(dataReader.GetValue(1));
            Portfolio portfolio = portfolioJObject.ToObject<Portfolio>();//casts the JObject to the portfolio class
            return portfolio;
        }

        public void SavePortfolio(Portfolio userPortfolio)
        {
            DateTime currentDateTime = DateTime.Now;//This may get used in the future
            JObject portfolioJObject = (JObject)JToken.FromObject(userPortfolio);
            string query = "INSERT INTO portfoliodb (EntryDate, PortfolioObject) values('" + currentDateTime + "','" + portfolioJObject + "')";
            SqlDataAdapter adapter = new SqlDataAdapter();
            _sqlConnection.Open();
            adapter.InsertCommand = new SqlCommand(query, _sqlConnection);
            _sqlConnection.Close();
        }
    }
}