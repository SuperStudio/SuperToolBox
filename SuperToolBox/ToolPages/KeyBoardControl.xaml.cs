using SuperToolBox.Config;
using SuperToolBox.Windows;
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

namespace SuperToolBox.ToolPages
{
    /// <summary>
    /// Interaction logic for DeviceInfo.xaml
    /// </summary>
    public partial class KeyBoardControl : Page
    {
        public KeyBoardControl()
        {
            InitializeComponent();
        }

        static Window_KeyBoardView keyBoardView;



        public void SetStatus()
        {
            if (keyBoardView == null)
            {
                keyBoardView = new Window_KeyBoardView();
                keyBoardView.Show();
            }
            keyBoardView.Visibility = ConfigManager.KeyBoardConfig.ShowKeyView ? Visibility.Visible : Visibility.Collapsed;

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetStatus();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SetStatus();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ConfigManager.KeyBoardConfig.Save();
        }
    }
}
