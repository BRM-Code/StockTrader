using InteractiveDataDisplay.WPF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Debug = System.Diagnostics.Debug;

namespace StockTrader_.NET_Framework_
{
    internal static class GraphHandler
    {
        public static async Task Draw(JToken data, int noDataPoints, MainWindow mainWindow, float minimum)
        {
            if (minimum != 0)
            {
                var z = await DrawX(noDataPoints, false);
                var w = new float[noDataPoints];
                for (var i = 0; i < noDataPoints;)
                {
                    w[i] = minimum;
                    i++;
                }
                var minimumLine = new LineGraph();
                mainWindow.lines.Children.Add(minimumLine);
                minimumLine.Stroke = new SolidColorBrush(Colors.Red);
                minimumLine.Description = $"Auto Sale Trigger Value";
                minimumLine.StrokeThickness = 2;
                minimumLine.Plot(z, w);
                return;
            }
            Debug.WriteLine("Drawing Graph...");
            mainWindow.lines.Children.Clear();
            if (data == null || noDataPoints == 0) return;
            var y = await DrawY(data, noDataPoints);
            var x = await DrawX(noDataPoints, false);
            var newline = new LineGraph();
            mainWindow.lines.Children.Add(newline);
            newline.Stroke = new SolidColorBrush(Colors.Blue);
            newline.Description = $"{mainWindow.CurrentName}'s Price";
            newline.StrokeThickness = 2;
            newline.Plot(x, y);
            Debug.WriteLine("Graph Drawn");
        }


        public static async Task PredictionDraw(JToken data, int noDataPoints, MainWindow mainWindow)
        {
            Debug.WriteLine("Drawing Prediction Graph...");
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
                case "Simple Moving Average":
                    {
                        x = await DrawX(noDataPoints,true);
                        y = await Predictor.Sma(data, noDataPoints);
                        break;
                    }
                case "Exponential Moving Average":
                {
                    x = await DrawX(noDataPoints, false);
                    y = await Predictor.Ema(data, noDataPoints);
                    break;
                }
            }
            var newline = new LineGraph();
            mainWindow.lines.Children.Add(newline);
            newline.Stroke = new SolidColorBrush(Colors.Green);
            newline.Description = $"{mainWindow.CurrentName}'s Trend";
            newline.StrokeThickness = 2;
            newline.Plot(x, y);
            Debug.WriteLine("Graph Drawn");
        }

        private static async Task<float[]> DrawY(JToken data, int noDataPoints)
        {
            Debug.WriteLine("Fetching Values for Y...");
            var y = new float[noDataPoints];
            var keysArray = data.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (var i = 0; i < noDataPoints;)
            {
                var a = keysArray[noDataPoints - i - 1];
                y[i] = Convert.ToSingle(data[a]["1. open"]);
                i++;
            }
            Debug.WriteLine("Fetched Values for Y");
            return y;
        }

        private static async Task<List<int>> DrawX(int noDataPoints,bool isSMA)
        {
            Debug.WriteLine("Fetching Values for X...");
            var a = 1;
            if (isSMA)a = 10;
            var x = new List<int>();
            for (var i = 0; i < noDataPoints;)
            {
                x.Add(i);
                i += a;
            }
            Debug.WriteLine("Fetched Values for X");
            return x;
        }
    }
}
