using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using InteractiveDataDisplay.WPF;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class GraphHandler
    {
        public LineGraph LineGraph;

        public GraphHandler(LineGraph linegraph)
        {
            LineGraph = linegraph;
        }

        public void Draw(JToken datapoints, int nodatapoints)
        {
            if (datapoints == null)
            {
                Console.WriteLine("The API returned null (probably because you ran out of calls)");
                return;
            }
            float[] y = new float[nodatapoints];
            for (int i = 0; i < nodatapoints; )
            {
                y[i] = Convert.ToSingle(datapoints[datapoints.ToObject<Dictionary<string, object>>().Keys.ToArray()[i]]["1. open"]);
                i++;
            }
          
            List<int> x = new List<int>();
            for (int i = 0; i < nodatapoints;)
            {
                x.Add(i);
                i++;
            }
            LineGraph.Plot(x, y); // x and y are IEnumerable<double>
            return;
        }
    }
}
