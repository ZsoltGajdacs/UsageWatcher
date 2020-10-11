using System;
using System.Windows;
using UsageWatcher;

namespace UsageWatcherTest
{
    public partial class MainWindow : Window
    {
        private readonly IWatcher watcher;

        private readonly DateTime startTime;
        public MainWindow()
        {
            InitializeComponent();

            startTime = DateTime.Now;
            watcher = new Watcher("testApp", Resolution.HalfMinute, 
                SavePreference.KeepDataForToday, DataPrecision.HighPrecision);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan usage = watcher.UsageForGivenTimeframe(startTime, startTime + TimeSpan.FromDays(1));
            MessageBox.Show(usage.ToString(), "Eddigi használat", MessageBoxButton.OK,
                MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
