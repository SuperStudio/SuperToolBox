using SuperControls.Style;
using SuperToolBox.Config;
using SuperToolBox.Entity;
using SuperToolBox.ViewModel;
using SuperToolBox.Windows;
using SuperUtils.Framework.WinNativeMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SuperUtils.Framework.WinNativeMethods.HotKey;

namespace SuperToolBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {

        private VieModel_Main vieModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }


        public void Init()
        {
            vieModel = new VieModel_Main();
            this.DataContext = vieModel;
            ConfigManager.InitConfig(); // 读取配置
            CreateSqlTables();
        }
        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveConfigValue();
            HotKey.UnregisterAllHotKey();
        }


        private void CreateSqlTables()
        {
            MouseCommand.InitSqlite();
        }


        private void SaveConfigValue()
        {
            ConfigManager.Main.X = this.Left;
            ConfigManager.Main.Y = this.Top;
            ConfigManager.Main.Width = this.Width;
            ConfigManager.Main.Height = this.Height;
            ConfigManager.Main.WindowState = (long)baseWindowState;
            ConfigManager.Main.Save();
        }

        public void AdjustWindow()
        {

            if (ConfigManager.Main.FirstRun)
            {
                this.Width = SystemParameters.WorkArea.Width * 0.8;
                this.Height = SystemParameters.WorkArea.Height * 0.8;
                this.Left = SystemParameters.WorkArea.Width * 0.1;
                this.Top = SystemParameters.WorkArea.Height * 0.1;
            }
            else
            {
                if (ConfigManager.Main.Height == SystemParameters.WorkArea.Height && ConfigManager.Main.Width < SystemParameters.WorkArea.Width)
                {
                    baseWindowState = 0;
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    this.CanResize = true;
                }
                else
                {
                    this.Left = ConfigManager.Main.X;
                    this.Top = ConfigManager.Main.Y;
                    this.Width = ConfigManager.Main.Width;
                    this.Height = ConfigManager.Main.Height;
                }


                baseWindowState = (BaseWindowState)ConfigManager.Main.WindowState;
                if (baseWindowState == BaseWindowState.FullScreen)
                {
                    this.WindowState = System.Windows.WindowState.Maximized;
                }
                else if (baseWindowState == BaseWindowState.None)
                {
                    baseWindowState = 0;
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }

            }
        }

        public Action HotKeyHandler1;
        public Action HotKeyHandler2;
        public bool HotKeySuccess1;
        public bool HotKeySuccess2;

        private void BaseWindow_ContentRendered(object sender, EventArgs e)
        {
            AdjustWindow();
            if (ConfigManager.Main.FirstRun) ConfigManager.Main.FirstRun = false;

            // 注册停止/开始的热键
            HotKeySuccess1 = HotKey.RegisterHotKey(this, 95270001, (uint)Modifiers.None | (uint)Modifiers.Control, Key.F11, () =>
            {
                //MessageBox.Show("触发热键 F11");
                HotKeyHandler1?.Invoke();
            });
            HotKeySuccess2 = HotKey.RegisterHotKey(this, 95270002, (uint)Modifiers.None | (uint)Modifiers.Control, Key.F12, () =>
             {
                 //MessageBox.Show("触发热键 F11");
                 HotKeyHandler2?.Invoke();
             });
        }

        private void OpenSetting(object sender, RoutedEventArgs e)
        {

        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            new About(this).ShowDialog();
        }

        private void ShowSettingsPopup(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Border border = sender as Border;
                ContextMenu contextMenu = border.ContextMenu;
                contextMenu.PlacementTarget = border;
                contextMenu.Placement = PlacementMode.Top;
                contextMenu.IsOpen = true;
            }
            e.Handled = true;
        }

        private void searchBox_Search(object sender, RoutedEventArgs e)
        {
            string value = searchBox.Text;
            if (string.IsNullOrEmpty(value)) value = "";
            vieModel.LoadToolList(value);
        }

        private void OpenTool(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && long.TryParse(button.Tag.ToString(), out long id))
            {
                vieModel.OpenTool(id);
                // 设置选中

                for (int i = 0; i < vieModel.ToolTabs.Count; i++)
                {
                    if (vieModel.ToolTabs[i].ToolID == id)
                    {
                        tabControl.SelectedIndex = i;
                        break;
                    }
                }

                //for (int i = 0; i < vieModel.ToolList.Count; i++)
                //{
                //    if (vieModel.ToolList[i].ToolID == id)
                //    {

                //        break;
                //    }
                //}

            }
        }

        private void CloseTabItem(object sender, RoutedEventArgs e)
        {
            int idx = tabControl.SelectedIndex;
            if (idx >= 0 && idx < vieModel.ToolTabs.Count)
            {
                vieModel.ToolTabs.RemoveAt(idx);
            }
        }
    }
}
