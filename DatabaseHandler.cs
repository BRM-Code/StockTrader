using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
        public Portfolio RetrievePortfolio(Startup startup)//Connects to the Database and gets the entry with the highest Id which is the latest
        {
            startup.ConnLabel.Visibility = Visibility.Visible;
            try{_sqlConnection.Open();}
            catch{MessageBox.Show("Could not connect to Database", "Error");}
            JToken data;
            try
            {
                Debug.WriteLine("Attempting SQL Connection...");
                var sqlCommand = new MySqlCommand("SELECT EntryDate,PortfolioObject FROM portfoliodb ORDER BY Id DESC LIMIT 1", _sqlConnection);
                _dataReader = sqlCommand.ExecuteReader();
                _dataReader.Read();
                try {data = JToken.Parse(_dataReader.GetValue(1).ToString());}
                catch
                {
                    Debug.WriteLine("Data Missing or Corrupt");
                    return NewPortfolio(startup);
                }
                Debug.WriteLine("SQL Connections Successful");
                startup.ConnVerfLabel.Content = "SQL Connections Successful";
            }
            catch
            {
                Debug.WriteLine("SQL Connection Failed");
                return NewPortfolio(startup);
            }
            float funds;
            Dictionary<string, StockStorage> stockDictionary;
            //Checking that Available Funds is just numbers
            if (Regex.IsMatch(data["AvailableFunds"].ToString(), "[0-9]+"))
            {
                funds = Convert.ToSingle(data["AvailableFunds"]);
            }
            else
            {
                Debug.WriteLine("Error with Available Funds");
                return NewPortfolio(startup);
            }
            //Checking that stockDictionary can conform to the Dictionary<string, StockStorage>
            try {stockDictionary = data["SharesDictionary"].ToObject<Dictionary<string, StockStorage>>();}
            catch
            {
                Debug.WriteLine("Error with Shares Dictionary");
                return NewPortfolio(startup);
            }
            //Create new portfolio with the data retrieved from the database
            var portfolio = new Portfolio(funds, stockDictionary);
            //Dealing with the SQL connection
            _sqlConnection.Close();
            _sqlConnection.Dispose();
            return portfolio;
        }

        private static Portfolio NewPortfolio(Startup startup)
        {
            startup.ConnVerfLabel.Content = "Database empty, creating new Portfolio";
            startup.ConnVerfLabel.Visibility = Visibility.Visible;
            var portfolio = new Portfolio(0, null);
            return portfolio;
        }

        public void SavePortfolio(Portfolio userPortfolio)
        {
            var currentDateTime = DateTime.Now;//This may get used in the future
            var portfolio = JToken.FromObject(userPortfolio);
            var query = "INSERT INTO portfoliodb (EntryDate, PortfolioObject) values('" + currentDateTime + "','" + portfolio + "')";
            var command = new MySqlCommand(query, _sqlConnection);
            _sqlConnection.Open();
            command.ExecuteNonQuery();
            _sqlConnection.Close();
            _sqlConnection.Dispose();
        }
    }
}
