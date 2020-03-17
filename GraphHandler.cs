using System;
using System.Collections.Generic;
using System.Linq;
using InteractiveDataDisplay.WPF;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    internal class GraphHandler
    {
        private readonly LineGraph _lineGraph;

        public GraphHandler(LineGraph linegraph)
        {
            _lineGraph = linegraph;
        }

        public void Draw(JToken datapoints, int nodatapoints)
        {
            if (datapoints == null || nodatapoints == 0)
            {
                return;
            }

            float[] y = new float[nodatapoints];
            var keysArray = datapoints.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (int i = 0; i < nodatapoints - 1;)
            {
                i++;
                var a = keysArray[nodatapoints - i - 1];
                y[i] = Convert.ToSingle(datapoints[a]["1. open"]);
            }

            List<int> x = new List<int>();
            for (int i = 0; i < nodatapoints;)
            {
                x.Add(i);
                i++;
            }
            _lineGraph.Plot(x, y);
        }
    }
}
