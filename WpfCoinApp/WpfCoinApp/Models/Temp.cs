using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCoinApp.Models
{

    public class Rootobject
    {
        public Datum[] data { get; set; }
        public long timestamp { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string rank { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string supply { get; set; }
        public string maxSupply { get; set; }
        public string marketCapUsd { get; set; }
        public string volumeUsd24Hr { get; set; }
        public string priceUsd { get; set; }
        public string changePercent24Hr { get; set; }
        public string vwap24Hr { get; set; }
        public string explorer { get; set; }
        public Tokens tokens { get; set; }
    }

    public class Tokens
    {
        public string[] _1 { get; set; }
        public string[] _10 { get; set; }
        public string[] _101 { get; set; }
        public string[] _137 { get; set; }
        public string[] _42161 { get; set; }
        public string[] _43114 { get; set; }
        public string[] _56 { get; set; }
    }

}
