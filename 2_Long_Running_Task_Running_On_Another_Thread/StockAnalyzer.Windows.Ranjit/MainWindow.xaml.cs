using StockAnalyzer.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace StockAnalyzer.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //ASYNC keyword is a way for us to indicate that this method here will contain asynchronous operations. 
        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            #region Before loading stock data
            var watch = new Stopwatch();
            watch.Start();
            StockProgress.Visibility = Visibility.Visible;
            StockProgress.IsIndeterminate = true;

            Search.Content = "Cancel";
            #endregion

            DataStoreAsync dataStore = new DataStoreAsync(Environment.CurrentDirectory);

            //AWAIT keyword is a way for us to indicate that we want to get back to this part of the code 
            // once the data is loaded without blocking UI thread.

            // TASKS of AWAIT keyword below
            // 1. Validate the success of asynchronous operation
            // 2. Make sures that there are no exceptions in the ASYNCHRONOUS operation
            // 3. Gives back the potential results
            // 4. Continuation back on the calling thread.
            var stocksDictionary = await dataStore.LoadStocks();
            Stocks.ItemsSource = stocksDictionary[Ticker.Text];

            #region After stock data is loaded
            StocksStatus.Text = $"Loaded stocks for {Ticker.Text} in {watch.ElapsedMilliseconds}ms";
            StockProgress.Visibility = Visibility.Hidden;
            Search.Content = "Search";
            #endregion
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));

            e.Handled = true;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
