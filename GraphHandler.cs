using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task Draw(JToken datapoints, int nodatapoints)
        {
            if (datapoints == null || nodatapoints == 0)return;
            var y = await DrawY(datapoints, nodatapoints);
            var x = await DrawX(nodatapoints);
            _lineGraph.Plot(x, y);
        }

        private static async Task<float[]> DrawY(JToken datapoints, int nodatapoints)
        {
            float[] y = new float[nodatapoints];
            var keysArray = datapoints.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (int i = 0; i < nodatapoints - 1;)
            {
                i++;
                var a = keysArray[nodatapoints - i - 1];
                y[i] = Convert.ToSingle(datapoints[a]["1. open"]);
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
