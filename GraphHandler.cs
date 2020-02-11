using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Draw(JToken datapoints)
        {
            if (datapoints == null)
            {
                Console.WriteLine("The API returned null (probably because you ran out of calls)");
                return;
            }
            float[] y = new float[40];
            for (int i = 0; i < 40; )
            {
                y[i] = Convert.ToSingle(datapoints[datapoints.ToObject<Dictionary<string, object>>().Keys.ToArray()[i]]["1. open"]);
                i++;
            }

            var x = new Int16[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39};//datapoints.ToObject<Dictionary<string, object>>().Keys.ToArray();
            LineGraph.Plot(x, y); // x and y are IEnumerable<double>
            return;
        }
    }
}
