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
        KeyboardHook hook;
        public MainWindow()
        {
            InitializeComponent();
            hook = new KeyboardHook(KeyboardHook.Parameters.PassAllKeysToNextApp);
            hook.KeyIntercepted += Hook_KeyIntercepted;
        }

        private void Hook_KeyIntercepted(object sender, KeyboardHook.KeyboardHookEventArgs e)
        {
            string name = e.KeyName;
        }
    }
}
