using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StockTrader_.NET_Framework_
{
    public static class Predictor
    {
        public static async Task<float[]> TenkanSen(JToken data, int noDataPoints)
        {
            var arrayLength = Convert.ToSingle(noDataPoints) / 9.0;
            var yValues = new float[(int)Math.Ceiling((decimal)arrayLength)];
            var keysArray = data.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (var index1 = 0; index1 < noDataPoints;)
            {
                var index2Max = index1 + 9;
                int index2;
                float high = 0;
                float low = 0;
                for (index2 = index1; index2 < index2Max;)
                {
                    if (index2 == noDataPoints) {break;}
                    var b = keysArray[noDataPoints - index2 - 1];
                    var valueHigh = Convert.ToSingle(data[b]["2. high"]);
                    var valueLow = Convert.ToSingle(data[b]["3. low"]);
                    if (valueHigh > high || index2 == index2Max - 9) {high = valueHigh;}
                    if (valueLow < low || index2 == index2Max - 9) {low = valueLow;}
                    index2++;
                }
                yValues[index1/9] = (float) ((high + low)/2);
                index1 +=9;
            }
            return yValues;
        }

        public static async Task<float[]> SenkouA(float[] TenkanSen, float[] KijkunSen)
        {
            var yValues = new float[TenkanSen.Length];
            var valueHold = (int) Math.Ceiling((double) (TenkanSen.Length / KijkunSen.Length));
            var currentValueHold = valueHold;
            var j = valueHold;
            for (var i = 0; i < TenkanSen.Length;)
            {
                if (currentValueHold == 0) { j++; }
                yValues[i] = (float) ((TenkanSen[i] + KijkunSen[j])/2);
                currentValueHold -= 1;
                i++;
            }
            return yValues;
        }

        public static async Task<float[]> SenkouB(JToken data, int noDataPoints)
        {
            var arrayLength = Convert.ToSingle(noDataPoints) / 52.0;
            var yValues = new float[(int)Math.Ceiling((decimal)arrayLength)];
            var keysArray = data.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (var index1 = 0; index1 < noDataPoints;)
            {
                var index2Max = index1 + 52;
                int index2;
                float high = 0;
                float low = 0;
                for (index2 = index1; index2 < index2Max;)
                {
                    if (index2 == noDataPoints) { break; }
                    var b = keysArray[noDataPoints - index2 - 1];
                    var valueHigh = Convert.ToSingle(data[b]["2. high"]);
                    var valueLow = Convert.ToSingle(data[b]["3. low"]);
                    if (valueHigh > high || index2 == index2Max - 9) { high = valueHigh; }
                    if (valueLow < low || index2 == index2Max - 9) { low = valueLow; }
                    index2++;
                }
                yValues[index1 / 52] = (float)((high + low)/2);
                index1 += 52;
            }
            return yValues;
        }

        public static async Task<float[]> Chikou(JToken data, int noDataPoints)
        {
            var arrayLength = Convert.ToSingle(noDataPoints) / 26.0;
            var yValues = new float[(int)Math.Ceiling((decimal)arrayLength)];
            var keysArray = data.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (var index1 = 0; index1 < noDataPoints;)
            {
                var b = keysArray[noDataPoints - index1 - 1];
                yValues[index1/26] = Convert.ToSingle(data[b]["4. close"]);
                index1 += 26;
            }
            return yValues;
        }

        public static async Task<float[]> KijkunSen(JToken data, int noDataPoints)
        {
            var arrayLength = Convert.ToSingle(noDataPoints) / 26.0;
            var yValues = new float[(int)Math.Ceiling((decimal)arrayLength)];
            var keysArray = data.ToObject<Dictionary<string, object>>().Keys.ToArray();
            for (var index1 = 0; index1 < noDataPoints;)
            {
                var index2Max = index1 + 26;
                int index2;
                float high = 0;
                float low = 0;
                for (index2 = index1; index2 < index2Max;)
                {
                    if (index2 == noDataPoints) { break; }
                    var b = keysArray[noDataPoints - index2 - 1];
                    var valueHigh = Convert.ToSingle(data[b]["2. high"]);
                    var valueLow = Convert.ToSingle(data[b]["3. low"]);
                    if (valueHigh > high || index2 == index2Max - 9) { high = valueHigh; }
                    if (valueLow < low || index2 == index2Max - 9) { low = valueLow; }
                    index2++;
                }
                yValues[index1/26] = (float)((high + low)/2);
                index1 += 26;
            }
            return yValues;
        }


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
                    if (index2 == noDataPoints)
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

            for (var i = 0; i < yValues.Length;)
            {
                Debug.WriteLine(yValues[i]);
                i++;
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
            var ey = await Predictor.Ey(data, noDataPoints);
            var ex = await Predictor.Ex(noDataPoints);
            var exSquared = await Predictor.ExSquared(noDataPoints);
            var exy = await Predictor.Exy(data, noDataPoints);
            var a = ((ey * exSquared) - (ex * exy)) / ((noDataPoints * exSquared) - (ex) *(ex));
            var b = (((noDataPoints*(exy)) - ((ex) * (ey))) / ((noDataPoints * exSquared) - ((ex) * (ex))));
            //y = a + bx
            var abFloats= new Dictionary<float, float>(){{a,b}};
            return abFloats;
        }

        private static async Task<float> Ey(JToken data, int noDataPoints)
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
