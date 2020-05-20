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
using UsageWatcher;

namespace UsageWatcherTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Watcher watcher;
        public MainWindow()
        {
            InitializeComponent();
            
            watcher = new Watcher(Resolution.HALF_MINUTE);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan usage = watcher.UsageSoFar();
            MessageBox.Show(usage.ToString(), "Eddigi használat", MessageBoxButton.OK,
                MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
