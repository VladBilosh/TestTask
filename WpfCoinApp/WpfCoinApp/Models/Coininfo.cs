using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCoinApp.Models
{
    public class Coininfo
    {
        public IEnumerable data { get; internal set; }

        public class CoinData
        {
            public string id { get; set; }
            public int rank { get; set; }
            public string symbol { get; set; }
            public string name { get; set; }
            public double? supply { get; set; }
            public double? maxSupply { get; set; }
            public double? marketCapUsd { get; set; }
            public double? volumeUsd24Hr { get; set; }
            public double? priceUsd { get; set; }
            public double? changePercent24Hr { get; set; }
            public double? vwap24Hr { get; set; }
            public string explorer { get; set; }
        }
    }
}
