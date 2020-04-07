using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    internal static class Predictor
    {
        public static async Task<float[]> Ema(JToken data, int noDataPoints)
        {
            var sma = await Sma(data, noDataPoints);
            var keysArray = data.ToObject<Dictionary<string, JToken>>().Keys.ToArray();
            var previousEma = sma[0];
            var yValues = new float[noDataPoints];
            for (var i = 0; i < noDataPoints;)
            {
                var currentItem = keysArray[noDataPoints - i - 1];
                var k = 2.0 / ((noDataPoints - i) + 1.0);
                var price = Convert.ToSingle(data[currentItem]["1. open"]);
                yValues[i] = price * (float)k + previousEma * (1 - (float)k);
                previousEma = yValues[i];
                i++;
            }
            return yValues;
        }

        public static async Task<float[]> Sma(JToken data, int noDataPoints)// for this one we only need to calculate y
        {
            Debug.WriteLine("Finding SMA...");
            var keysArray = data.ToObject<Dictionary<string, object>>().Keys.ToArray();
            Debug.WriteLine($"Using a array of {keysArray.Length} values...");
            var arrayLength = Convert.ToSingle(noDataPoints) / 10.0;
            var yValues = new float[(int) Math.Ceiling((decimal) arrayLength)];
            for (var index1 = 0; index1 < noDataPoints;)
            {
                var fractional = false;
                float c = 0;
                float total = 0;
                var index2Max = index1 + 10;
                int index2;
                for (index2 = index1; index2 < index2Max;) //This adds 10 values together
                {
                    if (index2 == keysArray.Length)
                    {
                        fractional = true;
                        break;
                    }
                    var b = keysArray[noDataPoints - index2 - 1];
                    c += Convert.ToSingle(data[b]["1. open"]);
                    index2++;
                }

                if (fractional)
                {
                    total += c / (index2 - (index2Max - 10));
                }
                else
                {
                    total += c / 10;
                }
                Debug.WriteLine($"Assigning Value at index {index1 / 10}");
                yValues[index1 / 10] = total;
                index1 += 10;
            }
            return yValues;
        }

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
            var ex = await Predictor.Ex(noDataPoints);
            var exSquared = await Predictor.ExSquared(noDataPoints);
            var exy = await Predictor.Exy(data, noDataPoints);
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

        private static async Task<float> Exy(JToken data, int noDataPoints)
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

        private static async Task<int> Ex(int noDataPoints)
        {
            int x = 0;
            for (int i = 0; i < noDataPoints;)
            {
                x += i;
                i++;
            }
            return x;
        }

        private static async Task<int> ExSquared(int noDataPoints)
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
