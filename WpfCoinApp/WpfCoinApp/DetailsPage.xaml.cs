using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using WpfCoinApp.Models;
using System.Net;
using System.Net.Http;
using System.Diagnostics;

namespace WpfCoinApp
{
    /// <summary>
    /// Interaction logic for DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : Page
    {
        private Frame mainFrame;
        private string assetId;
        private static readonly HttpClient httpClient = new HttpClient();
        private Coininfo.CoinData coinInfo = null!;

        public TextBlock? NameText { get; private set; }
        public TextBlock? PriceText { get; private set; }
        public TextBlock? VolumeText { get; private set; }
        public string Id { get; }
        public Models.Coininfo.CoinData Coin { get; }

        internal class Market
        {
            public string? exchangeId { get; set; }
            public string? baseSymbol { get; set; }
            public string? quoteSymbol { get; set; }
            public double? priceUsd { get; set; }
            public double? volumeUsd24Hr { get; set; }
        }

        internal class MarketsResponse
        {
            public Market[]? data { get; set; }
        }
        public class Coininfo
        {
            public CoinData[]? data { get; set; } 

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

        public DetailsPage(string assetId, Frame mainFrame, Coininfo.CoinData coinInfo)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;
            this.assetId = assetId;
            _ = LoadDetailsAsync();
            _ = LoadMarketsAsync();
            this.coinInfo = coinInfo;
            DataContext = this.coinInfo;
        }

        public DetailsPage(string id, Frame mainFrame, Models.Coininfo.CoinData coin)
        {
            Id = id;
            this.mainFrame = mainFrame;
            Coin = coin;
        }

        private async Task LoadMarketsAsync()
        {
            try
            {
                string url = $"https://rest.coincap.io/v3/assets/{assetId}/markets?apiKey=9937f378074076df75f56501936268a2a8efc25d656413055a936ca72f06dcc3";
                var json = await httpClient.GetStringAsync(url);
                var marketInfo = JsonConvert.DeserializeObject<MarketsResponse>(json);
                MarketsList.ItemsSource = marketInfo?.data?.ToList() ?? new List<Market>();
            }
            catch
            {
                MarketsList.ItemsSource = null;
            }
        }

        private async Task LoadDetailsAsync()
        {
            try
            {
                string url = $"https://rest.coincap.io/v3/assets?ids={assetId}&apiKey=9937f378074076df75f56501936268a2a8efc25d656413055a936ca72f06dcc3";
                var json = await httpClient.GetStringAsync(url);
                var coinInfo = JsonConvert.DeserializeObject<Coininfo>(json);
                var data = coinInfo?.data?.FirstOrDefault();
                if (data != null)
                {
                    SetTextSafe(NameText, $"Name: {data.name}");
                    SetTextSafe(PriceText, $"Price: {data.priceUsd}");
                    SetTextSafe(VolumeText, $"Volume: {data.volumeUsd24Hr}");
                    SetTextSafe(ChangeText, $"Change (24Hr): {data.changePercent24Hr}");
                }
                else
                {
                    SetTextSafe(NameText, "Name: N/A");
                    SetTextSafe(PriceText, "Price: N/A");
                    SetTextSafe(VolumeText, "Volume: N/A");
                    SetTextSafe(ChangeText, "Change (24Hr): N/A");
                }
            }
            catch (Exception ex)
            {
                SetTextSafe(NameText, $"Error loading details: {ex.Message}");
                SetTextSafe(PriceText, "");
                SetTextSafe(VolumeText, "");
                SetTextSafe(ChangeText, "");
            }
        }

        private void SetTextSafe(TextBlock? textBlock, string value)
        {
            if (textBlock != null)
                textBlock.Text = value;
        }

        private void MarketsList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MarketsList.SelectedItem is Market market && !string.IsNullOrWhiteSpace(market.exchangeId))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = $"https://www.{market.exchangeId}.com/",
                        UseShellExecute = true
                    });
                }
                catch { }
            }
        }
    }
}

