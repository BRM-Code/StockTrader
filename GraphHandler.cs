using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using InteractiveDataDisplay.WPF;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    internal class GraphHandler
    {
        public async Task Draw(JToken data, int noDataPoints, MainWindow mainWindow)
        {
            mainWindow.lines.Children.Clear();
            if (data == null || noDataPoints == 0)return;
            var y = await DrawY(data, noDataPoints);
            var x = await DrawX(noDataPoints);
            var newline = new LineGraph();
            mainWindow.lines.Children.Add(newline);
            newline.Stroke = new SolidColorBrush(Colors.Blue);
            newline.Description = $"{mainWindow.CurrentCompanyName}'s Price";
            newline.StrokeThickness = 2;
            newline.Plot(x, y);
        }

        public async Task PredictionDraw(JToken data, int noDataPoints, MainWindow mainWindow)
        {
            IEnumerable x = null;
            IEnumerable y = null;
            var algorithm = mainWindow.PredictionMethodComboBox.SelectionBoxItem.ToString();
            switch (algorithm)
            {
                case "Linear Extrapolation":
                {
                    var predictedValues = await Predictor.LinearExtrapolation(data, noDataPoints);
                    x = predictedValues.Keys.ToArray();
                    y = predictedValues.Values.ToArray();
                    break;
                }
                case "Moving Average":
                {
                    var predictedValues = await Predictor.SMA(data, noDataPoints);
                    x = await DrawX(noDataPoints);
                    y = predictedValues;
                    break;
                }
            }
            var newline = new LineGraph();
            mainWindow.lines.Children.Add(newline);
            newline.Stroke = new SolidColorBrush(Colors.Orange);
            newline.Description = $"{mainWindow.CurrentCompanyName}'s Trend";
            newline.StrokeThickness = 2;
            newline.Plot(x, y);
        }

        private static async Task<float[]> DrawY(JToken data, int noDataPoints)
        {
            var y = new float[noDataPoints];
            var keysArray = data.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (int i = 0; i < noDataPoints;)
            {
                var a = keysArray[noDataPoints - i - 1];
                y[i] = Convert.ToSingle(data[a]["1. open"]);
                i++;
            }
            return y;
        }

        private static async Task<List<int>> DrawX(int noDataPoints)
        {
            List<int> x = new List<int>();
            for (int i = 0; i < noDataPoints;)
            {
                x.Add(i);
                i++;
            }
            return x;
        }
    }
}
