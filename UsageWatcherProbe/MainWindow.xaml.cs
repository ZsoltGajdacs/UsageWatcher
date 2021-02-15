using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using UsageWatcher;
using UsageWatcher.Model;
using UsageWatcher.Enums;

namespace UsageWatcherProbe
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

        private void Overall_Usage_Btn_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan usage = watcher.UsageForGivenTimeframe(startTime, startTime + TimeSpan.FromDays(1));
            MessageBox.Show(usage.ToString(), "Overall usage", MessageBoxButton.OK,
                MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }

        private void Usage_In_Blocks_Click(object sender, RoutedEventArgs e)
        {
            List<UsageBlock> usageBlockList = watcher.UsageListForGivenTimeFrame(startTime, startTime + TimeSpan.FromDays(1));
            string msg = UsageBlockListToString(usageBlockList);

            MessageBox.Show(msg, "Usage in block", MessageBoxButton.OK,
                MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }

        private void Gaps_In_Usage_Click(object sender, RoutedEventArgs e)
        {
            List<UsageBlock> usageBlockList = watcher.NotUsageListForGivenTimeFrame(startTime, startTime + TimeSpan.FromDays(1));
            string msg = UsageBlockListToString(usageBlockList);

            MessageBox.Show(msg, "Gaps in usage", MessageBoxButton.OK,
                MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }

        private string UsageBlockListToString(List<UsageBlock> usageBlockList)
        {
            StringBuilder sb = new StringBuilder();

            foreach (UsageBlock block in usageBlockList)
            {
                sb.Append(block.StartTime);
                sb.Append(" -> ");
                sb.Append(block.EndTime);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
