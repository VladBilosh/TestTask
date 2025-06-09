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

namespace WpfCoinApp
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private Frame mainFrame;
        private static readonly HttpClient httpClient = new HttpClient();
        private int topCount = 10; // Default to top 10

        public MainPage()
        {
            InitializeComponent();
            SetCurrencyListTemplate();
            SetUserAgentHeader();
            _ = LoadTopCurrenciesAsync();
        }

        public MainPage(Frame mainFrame) : this()
        {
            this.mainFrame = mainFrame;
        }

        private void SetUserAgentHeader()
        {
            if (!httpClient.DefaultRequestHeaders.UserAgent.Any())
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; WpfCoinApp/1.0)");
            }
        }

        private async Task LoadTopCurrenciesAsync()
        {
            try
            {
                string url = $"https://rest.coincap.io/v3/assets?limit={topCount}&apiKey=9937f378074076df75f56501936268a2a8efc25d656413055a936ca72f06dcc3";
                var json = await httpClient.GetStringAsync(url);
                var coinInfo = JsonConvert.DeserializeObject<Coininfo>(json);
                CurrencyList.ItemsSource = coinInfo?.data;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Failed to load data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = $"https://rest.coincap.io/v3/assets?search={SearchBox.Text}&limit={topCount}&apiKey=9937f378074076df75f56501936268a2a8efc25d656413055a936ca72f06dcc3";
                var json = await httpClient.GetStringAsync(url);
                var coinInfo = JsonConvert.DeserializeObject<Coininfo>(json);
                CurrencyList.ItemsSource = coinInfo?.data;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Failed to search: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetCurrencyListTemplate()
        {
            var template = new DataTemplate(typeof(Coininfo.CoinData));

            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            // ID
            var idText = new FrameworkElementFactory(typeof(TextBlock));
            idText.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("id"));
            idText.SetValue(TextBlock.WidthProperty, 100.0);
            stackPanelFactory.AppendChild(idText);

            // Name
            var nameText = new FrameworkElementFactory(typeof(TextBlock));
            nameText.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("name"));
            nameText.SetValue(TextBlock.WidthProperty, 200.0);
            stackPanelFactory.AppendChild(nameText);

            // Symbol
            var symbolText = new FrameworkElementFactory(typeof(TextBlock));
            symbolText.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("symbol"));
            symbolText.SetValue(TextBlock.WidthProperty, 100.0);
            stackPanelFactory.AppendChild(symbolText);

            // PriceUsd
            var priceText = new FrameworkElementFactory(typeof(TextBlock));
            priceText.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("priceUsd"));
            priceText.SetValue(TextBlock.WidthProperty, 150.0);
            stackPanelFactory.AppendChild(priceText);

            template.VisualTree = stackPanelFactory;
            CurrencyList.ItemTemplate = template;
        }

        private void CurrencyList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrencyList.SelectedItem is Coininfo.CoinData coin && mainFrame != null)
            {
                mainFrame.Navigate(new DetailsPage(coin.id, mainFrame, coin));
            }
        }

        private void CurrencyList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrencyList.SelectedItem is Coininfo.CoinData coin && mainFrame != null)
            {
                mainFrame.Navigate(new DetailsPage(coin.id, mainFrame, coin));
            }
        }

        // Add this event handler to respond to user input for N
        private async void TopCountBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(TopCountBox.Text, out int n) && n > 0)
            {
                topCount = n;
                await LoadTopCurrenciesAsync();
            }
        }
    }
}
