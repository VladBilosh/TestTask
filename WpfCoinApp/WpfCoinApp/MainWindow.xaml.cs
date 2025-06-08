using System.Text;
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
using System.Net;
using System.Net.Http;
using System.Diagnostics;

namespace WpfCoinApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string APIkey = "9937f378074076df75f56501936268a2a8efc25d656413055a936ca72f06dcc3";

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainPage(MainFrame));
        }

        private async void SerchBtn_Click(object sender, RoutedEventArgs e)
        {
            await GetCoinInfoAsync();
        }

        private async Task GetCoinInfoAsync()
        {
            using HttpClient client = new HttpClient();
            try
            {
                string url = $"https://rest.coincap.io/v3/assets?search={TBCoin.DataContext}&apiKey={APIkey}";
                string json = await client.GetStringAsync(url);

                Coininfo? coinInfo = JsonConvert.DeserializeObject<Coininfo>(json);

                if (coinInfo?.data != null && coinInfo.data.Cast<object>().Any())
                {
                    var firstCoin = coinInfo.data.Cast<Coininfo.CoinData>().FirstOrDefault();
                    CoinName_Copy.DataContext = firstCoin?.name ?? "Not found";
                    CoinPrice_Copy2.DataContext = firstCoin?.priceUsd?.ToString() ?? "-";
                }
                else
                {
                    CoinName_Copy.DataContext = "Not found";
                    CoinPrice_Copy2.DataContext = "-";
                }
            }
            catch
            {
                CoinName_Copy.DataContext = "Error";
                CoinPrice_Copy2.DataContext = "-";
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}