using System;
using System.Collections.Generic;
using System.Linq;
using InteractiveDataDisplay.WPF;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    class GraphHandler
    {
        private readonly LineGraph LineGraph;

        public GraphHandler(LineGraph linegraph)
        {
            LineGraph = linegraph;
        }

        public void Draw(JToken datapoints, int nodatapoints)
        {
            if (datapoints == null || nodatapoints == 0)
            {
                return;
            }

            float[] y = new float[nodatapoints];
            for (int i = 0; i < nodatapoints; i++)
            {
                y[i] = Convert.ToSingle(datapoints[datapoints.ToObject<Dictionary<string, object>>().Keys.ToArray()[nodatapoints - i - 1]]["1. open"]);
            }

            List<int> x = new List<int>();
            for (int i = 0; i < nodatapoints;)
            {
                x.Add(i);
                i++;
            }
            LineGraph.Plot(x, y);
            return;
        }
    }
}
