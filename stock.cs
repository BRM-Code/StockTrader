using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StockTrader_.NET_Framework_
{
    public class stock
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string primaryExchange { get; set; }
        public string calculationPrice { get; set; }
        public string open { get; set; }
        public string openTime { get; set; }
        public string close { get; set; }
        public string closeTime { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string latestPrice { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public string latestUpdate { get; set; }
        public string latestVolume { get; set; }
        public string iexRealtimePrice { get; set; }
        public string iexRealtimeSize { get; set; }
        public string iexLastUpdated { get; set; }
        public string delayedPrice { get; set; }
        public string delayedPriceTime { get; set; }
        public string extendedPrice { get; set; }
        public string extendedChange { get; set; }
        public string extendedChangePercent { get; set; }
        public string extendedPriceTime { get; set; }
        public string previousClose { get; set; }
        public string previousVolume { get; set; }
        public string change { get; set; }
        public string changePercent { get; set; }
        public string volume { get; set; }
        public string iexMarketPercent { get; set; }
        public string iexVolume { get; set; }
        public string avgTotalVolume { get; set; }
        public string iexBidPrice { get; set; }
        public string iexBidSize { get; set; }
        public string iexAskPrice { get; set; }
        public string iexAskSize { get; set; }
        public string marketCap { get; set; }
        public string peRatio { get; set; }
        public string week52High { get; set; }
        public string week52Low { get; set; }
        public string ytdChange { get; set; }
        public string lastTradeTime { get; set; }
        public bool isUSMarketOpen { get; set; }
    }
}
