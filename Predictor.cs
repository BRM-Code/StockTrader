using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    static class Predictor
    {
        public static async Task<Dictionary<int, float>> LinearExtrapolation(JToken data, int noDataPoints)
        {
            //Get Equation
            var ab = await LeEquation(data, noDataPoints);
            var a = ab.Keys.ToArray()[0];
            var b = ab.Values.ToArray()[0];
            //Convert Equation to a Dictionary of values
            var dataPointDictionary = new Dictionary<int, float>();
            for (var x = 0; x < noDataPoints - 1;)
            {
                var y = a + b*x; //y = a + bx
                dataPointDictionary.Add(x,y);
                x++;
            }
            //Send to Graph
            return dataPointDictionary;
        }

        private static async Task<Dictionary<float,float>> LeEquation(JToken data, int noDataPoints)//Get Equation
        {
            var ey = await Predictor.ey(data, noDataPoints);
            var ex = await Predictor.ex(noDataPoints);
            var exSquared = await Predictor.exSquared(noDataPoints);
            var exy = await Predictor.exy(data, noDataPoints);
            var a = ((ey * exSquared) - (ex * exy)) / ((noDataPoints * exSquared) - (ex) *(ex));
            var b = (((noDataPoints*(exy)) - ((ex) * (ey))) / ((noDataPoints * exSquared) - ((ex) * (ex))));
            //y = a + bx
            var abFloats= new Dictionary<float, float>(){{a,b}};
            return abFloats;
        }

        private static async Task<float> ey(JToken data, int noDataPoints)
        {
            float y = 0;
            var keysArray = data.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (var i = 0; i < noDataPoints - 1;)
            {
                i++;
                var a = keysArray[noDataPoints - i - 1];
                y += Convert.ToSingle(data[a]["1. open"]);
            }
            return y;
        }

        private static async Task<float> exy(JToken data, int noDataPoints)
        {
            float exy = 0;
            var keysArray = data.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (var i = 0; i < noDataPoints - 1;)
            {
                i++;
                var a = keysArray[noDataPoints - i - 1];
                exy += (Convert.ToSingle(data[a]["1. open"]) * i);
            }
            return exy;
        }

        private static async Task<int> ex(int noDataPoints)
        {
            int x = 0;
            for (int i = 0; i < noDataPoints;)
            {
                x += i;
                i++;
            }
            return x;
        }

        private static async Task<int> exSquared(int noDataPoints)
        {
            int xSquared = 0;
            for (int i = 1; i < noDataPoints;)
            {
                xSquared += i * i;
                i++;
            }
            return xSquared;
        }
    }
}
