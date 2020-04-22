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
                var z = await DrawX(noDataPoints, 1);
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
            var x = await DrawX(noDataPoints, 1);
            var newline = new LineGraph();
            mainWindow.lines.Children.Add(newline);
            newline.Stroke = new SolidColorBrush(Colors.Blue);
            newline.Description = $"{mainWindow.CurrentName}'s Price";
            newline.StrokeThickness = 2;
            newline.Plot(x, y);
            Debug.WriteLine("Graph Drawn");
        }


        public static async void PredictionDraw(JToken data, int noDataPoints, MainWindow mainWindow)
        {
            Debug.WriteLine("Drawing Prediction Graph...");
            IEnumerable x = null;
            IEnumerable y = null;
            var algorithm = mainWindow.PredictionMethodComboBox.SelectionBoxItem.ToString();
            float[] a = null;
            float[] b = null;
            float[] c = null;
            float[] d = null;
            float[] e = null;
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
                        x = await DrawX(noDataPoints,10);
                        y = await Predictor.Sma(data, noDataPoints);
                        break;
                    }
                case "Exponential Moving Average":
                    {
                       x = await DrawX(noDataPoints, 1);
                       y = await Predictor.Ema(data, noDataPoints);
                       break;
                    }
                case "Tenkan-Sen (Conversion Line)":
                    {
                        x = await DrawX(noDataPoints, 9);
                        y = await Predictor.TenkanSen(data, noDataPoints);
                        break;
                    }
                case "Kijun-Sen (Base Line)":
                    {
                        x = await DrawX(noDataPoints, 26);
                        y = await Predictor.KijkunSen(data, noDataPoints);
                        break;
                    }
                case "Senkou Span A":
                    {
                        x = await DrawX(noDataPoints, 9);
                        var KijkunSen = await Predictor.KijkunSen(data, noDataPoints);
                        var TenkanSen = await Predictor.TenkanSen(data, noDataPoints);
                        y = await Predictor.SenkouA(TenkanSen, KijkunSen);
                        break;
                    }
                case "Senkou Span B":
                    {
                        x = await DrawX(noDataPoints, 52);
                        y = await Predictor.SenkouB(data, noDataPoints);
                        break;
                    }
                case "Chikou Span (Lagging Span)":
                    {
                        x = await DrawX(noDataPoints, 26);
                        y = await Predictor.Chikou(data, noDataPoints);
                        break;
                    }
                case "Ichimoku Cloud":
                    {
                        //Chikou
                        a = await Predictor.Chikou(data, noDataPoints);
                        var ax = await DrawX(noDataPoints, 26);
                        var lineA = new LineGraph();
                        mainWindow.lines.Children.Add(lineA);
                        lineA.Stroke = new SolidColorBrush(Colors.Orange);
                        lineA.Description = $"Chikou";
                        lineA.StrokeThickness = 2;
                        lineA.Plot(ax, a);

                        //Tenkan-San
                        b = await Predictor.TenkanSen(data, noDataPoints);
                        var bx = await DrawX(noDataPoints, 9);
                        var lineB = new LineGraph();
                        mainWindow.lines.Children.Add(lineB);
                        lineB.Stroke = new SolidColorBrush(Colors.Green);
                        lineB.Description = $"Tenkan-Sen";
                        lineB.StrokeThickness = 2;
                        lineB.Plot(bx, b);

                        //Kijkun-Sen
                        c = await Predictor.KijkunSen(data, noDataPoints);
                        var lineC = new LineGraph();
                        mainWindow.lines.Children.Add(lineC);
                        lineC.Stroke = new SolidColorBrush(Colors.Indigo);
                        lineC.Description = $"Kijun-Sen";
                        lineC.StrokeThickness = 2;
                        lineC.Plot(ax, c);

                        //Senkou Span A
                        d = await Predictor.SenkouA(b, c);
                        var lineD = new LineGraph();
                        mainWindow.lines.Children.Add(lineD);
                        lineD.Stroke = new SolidColorBrush(Colors.BlueViolet);
                        lineD.Description = $"Senkou Span A";
                        lineD.StrokeThickness = 2;
                        lineD.Plot(bx, d);

                        //Senkou Span B
                        e = await Predictor.SenkouB(data, noDataPoints);
                        var cx = await DrawX(noDataPoints, 52);
                        var lineE = new LineGraph();
                        mainWindow.lines.Children.Add(lineE);
                        lineE.Stroke = new SolidColorBrush(Colors.Salmon);
                        lineE.Description = $"Senkou Span B";
                        lineE.StrokeThickness = 2;
                        lineE.Plot(cx, e);
                        break;
                    }
            }

            if (algorithm != "Ichimoku Cloud")
            {
                var newline = new LineGraph();
                mainWindow.lines.Children.Add(newline);
                newline.Stroke = new SolidColorBrush(Colors.Green);
                newline.Description = $"{mainWindow.CurrentName}'s {algorithm}";
                newline.StrokeThickness = 2;
                newline.Plot(x, y);
            }
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

        private static async Task<List<int>> DrawX(int noDataPoints,int period)
        {
            Debug.WriteLine("Fetching Values for X...");
            var a = period;
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
