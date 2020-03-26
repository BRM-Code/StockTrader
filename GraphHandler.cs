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

        public GraphHandler(LineGraph lineGraph)
        {
            _lineGraph = lineGraph;
        }

        public async Task Draw(JToken data, int noDataPoints)
        {
            if (data == null || noDataPoints == 0)return;
            var y = await DrawY(data, noDataPoints);
            var x = await DrawX(noDataPoints);
            _lineGraph.Plot(x, y);
        }

        public async Task QuickDraw(Dictionary<int,float> dataPointDictionary)
        {
            var x = dataPointDictionary.Keys.ToArray();
            var y = dataPointDictionary.Values.ToArray();

        }

        private static async Task<float[]> DrawY(JToken data, int noDataPoints)
        {
            var y = new float[noDataPoints];
            var keysArray = data.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (int i = 0; i < noDataPoints - 1;)
            {
                i++;
                var a = keysArray[noDataPoints - i - 1];
                y[i] = Convert.ToSingle(data[a]["1. open"]);
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
