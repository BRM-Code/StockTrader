using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrader_.NET_Framework_
{
    public class stock
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string primaryExchange { get; set; }
        public float calculationPrice { get; set; }
        public float open { get; set; }
        public int openTime { get; set; }
        public float close { get; set; }
        public int closeTime { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float latestPrice { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public int latestUpdate { get; set; }
        public int latestVolume { get; set; }
        public float iexRealtimePrice { get; set; }
        public int iexRealtimeSize { get; set; }
        public int iexLastUpdated { get; set; }
        public float delayedPrice { get; set; }
        public int delayedPriceTime { get; set; }
        public float extendedPrice { get; set; }
        public float extendedChange { get; set; }
        public float extendedChangePercent { get; set; }
        public int extendedPriceTime { get; set; }
        public float previousClose { get; set; }
        public int previousVolume { get; set; }
        public float change { get; set; }
        public float changePercent { get; set; }
        public int volume { get; set; }
        public float iexMarketPercent { get; set; }
        public int iexVolume { get; set; }
        public int avgTotalVolume { get; set; }
        public float iexBidPrice { get; set; }
        public int iexBidSize { get; set; }
        public float iexAskPrice { get; set; }
        public int iexAskSize { get; set; }
        public int marketCap { get; set; }
        public float peRatio { get; set; }
        public float week52High { get; set; }
        public float week52Low { get; set; }
        public float ytdChange { get; set; }
        public int lastTradeTime { get; set; }
        public bool isUSMarketOpen { get; set; }
    }
}
