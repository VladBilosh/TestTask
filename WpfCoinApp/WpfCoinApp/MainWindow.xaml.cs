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

namespace WpfCoinApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string APIkey = "9937f378074076df75f56501936268a2a8efc25d656413055a936ca72f06dcc3";

        private void SerchBtn_Click(object sender, RoutedEventArgs e)
        {
            GetCoinInfo();
        }

        void GetCoinInfo()
        {
            using (WebClient client = new WebClient())
            {
                string url = string.Format("https://rest.coincap.io/v3/assets?apiKey={0}&appid={1}", TBCoin.Text, APIkey);
                var json = client.DownloadString(url);
                Coininfo.Root coinInfo = JsonConvert.DeserializeObject<Coininfo.Root>(json);

                CoinName_Copy.DataContext = coinInfo.data.name;
                CoinPrice_Copy2.DataContext = coinInfo.data.priceUsd;
            }
        }

    }
}