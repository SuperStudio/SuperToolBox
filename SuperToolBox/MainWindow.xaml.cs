using SuperControls.Style;
using SuperControls.Style.Plugin;
using SuperControls.Style.Windows;
using SuperToolBox.Config;
using SuperToolBox.Entity;
using SuperToolBox.Upgrade;
using SuperToolBox.ViewModel;
using SuperToolBox.Windows;
using SuperUtils.Framework.WinNativeMethods;
using SuperUtils.IO;
using SuperUtils.Media;
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
            if (vieModel != null && vieModel.ToolTabs != null)
                ConfigManager.Main.BeforeOpenedTools =
                    string.Join(",", vieModel.ToolTabs.Select(arg => arg.ToolID));
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

            }
        }

        public Action HotKeyHandler1;
        public Action HotKeyHandler2;
        public bool HotKeySuccess1;
        public bool HotKeySuccess2;

        private void BaseWindow_ContentRendered(object sender, EventArgs e)
        {
            this.TopMenu = TopMenus;


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
            InitThemeSelector();
            InitUpgrade();
            OpenBeforeTools();
            InitNotice();
        }

        public void InitNotice()
        {
            noticeViewer.SetConfig(UrlManager.NOTICE_URL, ConfigManager.Main.LatestNotice);
            noticeViewer.onError += (error) =>
            {
                App.Logger?.Error(error);
            };

            noticeViewer.onShowMarkdown += (markdown) =>
            {
                //MessageCard.Info(markdown);
            };
            noticeViewer.onNewNotice += (newNotice) =>
            {
                ConfigManager.Main.LatestNotice = newNotice;
                ConfigManager.Main.Save();
            };

            noticeViewer.BeginCheckNotice();
        }

        private void OpenBeforeTools()
        {
            if (!string.IsNullOrEmpty(ConfigManager.Main.BeforeOpenedTools))
            {
                foreach (var item in ConfigManager.Main.BeforeOpenedTools.Split(','))
                {
                    if (long.TryParse(item, out long id))
                        OpenToolById(id);
                }
            }
        }


        public void InitUpgrade()
        {
            UpgradeHelper.Init(this);
            UpgradeHelper.OnBeforeCopyFile += () =>
            {
                this.CloseToTaskBar = false;
                this.Close();
            };
            CheckUpgrade();
        }

        private async void CheckUpgrade()
        {
            // 启动后检查更新
            try
            {
                await Task.Delay(UpgradeHelper.AUTO_CHECK_UPGRADE_DELAY);
                (string LatestVersion, string ReleaseDate, string ReleaseNote) result = await UpgradeHelper.GetUpgradeInfo();
                string remote = result.LatestVersion;
                string ReleaseDate = result.ReleaseDate;
                if (!string.IsNullOrEmpty(remote))
                {
                    string local = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    local = local.Substring(0, local.Length - ".0.0".Length);
                    if (local.CompareTo(remote) < 0)
                    {
                        bool opened = (bool)new MsgBox(
                            $"存在新版本\n版本：{remote}\n日期：{ReleaseDate}").ShowDialog();
                        if (opened)
                            UpgradeHelper.OpenWindow(this);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void OpenSetting(object sender, RoutedEventArgs e)
        {

        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            //string local = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //local = local.Substring(0, local.Length);
            //System.Windows.Media.Imaging.BitmapImage bitmapImage = ImageHelper.ImageFromUri("pack://application:,,,/Resources/ICO/Icon_128.png");
            //SuperControls.Style.Windows.About about = new SuperControls.Style.Windows.About(this, bitmapImage, "SuperToolBox",
            //    "Window 超级工具箱", local, ConfigManager.RELEASE_DATE,
            //    "Github", UrlManager.PROJECT_URL, "Chao", "GPL-3.0");
            //about.OnOtherClick += (s, ev) =>
            //{
            //    //FileHelper.TryOpenUrl(UrlManager.WebPage);
            //};
            //about.ShowDialog();

            Dialog_About about = new Dialog_About();
            string local = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            local = local.Substring(0, local.Length - ".0.0".Length);
            about.AppName = "SuperToolBox";
            about.AppSubName = "Window 超级工具箱";
            about.Version = local;
            about.ReleaseDate = ConfigManager.RELEASE_DATE;
            about.Author = "Chao";
            about.License = "GPL-3.0";
            about.GithubUrl = "https://github.com/SuperStudio/SuperToolBox";
            about.WebUrl = "https://github.com/SuperStudio/SuperToolBox";
            about.JoinGroupUrl = "http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=NUWfnBvGoMYevSVIvO5rWQEc0yz34p7m&authKey=V77%2F8cODFvwONlULFSxHtdlCqqYHPT16%2BQsoRD2%2FlJLzX6Kyu0Eob8KO0jC7OREG&noverify=0&group_code=589132982";
            about.Image = ImageHelper.ImageFromUri("pack://application:,,,/SuperToolBox;Component/Resources/ICO/ICON_128.png");
            about.ShowDialog();
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
            if (string.IsNullOrEmpty(value))
                value = "";
            vieModel.LoadToolList(value);
        }

        private void OpenTool(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && long.TryParse(button.Tag.ToString(), out long id))
            {

                OpenToolById(id);
                //for (int i = 0; i < vieModel.ToolList.Count; i++)
                //{
                //    if (vieModel.ToolList[i].ToolID == id)
                //    {

                //        break;
                //    }
                //}

            }
        }
        private void OpenToolById(long id)
        {
            vieModel.OpenTool(id);
            // 设置选中
            for (int i = vieModel.ToolTabs.Count - 1; i >= 0; i--)
            {
                if (vieModel.ToolTabs[i].ToolID == id)
                {
                    tabControl.SelectedIndex = i;
                    break;
                }
            }
        }

        private void CloseTabItem(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null || element.Tag == null) return;
            string toolID = element.Tag.ToString();
            int idx = -1;
            for (int i = 0; i < vieModel.ToolTabs.Count; i++)
            {
                if (vieModel.ToolTabs[i].ToolID.ToString().Equals(toolID))
                {
                    idx = i;
                    break;
                }
            }



            if (idx >= 0 && idx < vieModel.ToolTabs.Count)
            {
                vieModel.ToolTabs.RemoveAt(idx);
            }
        }

        private void OpenDonate(object sender, RoutedEventArgs e)
        {
            Window_Donate window_Donate = new Window_Donate();
            window_Donate.SetUrl(UrlManager.GetDonateJsonUrl());
            window_Donate.ShowDialog();
        }

        private void ShowPluginWindow(object sender, RoutedEventArgs e)
        {
            Window_Plugin window_Plugin = new Window_Plugin();

            PluginConfig config = new PluginConfig();
            config.PluginBaseDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
            config.RemoteUrl = UrlManager.GetPluginUrl();
            // 读取本地配置
            window_Plugin.OnEnabledChange += (data, enabled) =>
            {
                return true;
            };

            window_Plugin.OnBeginDelete += (data) =>
            {
                return true;
            };

            window_Plugin.OnBeginDownload += (data) =>
            {
                return true;
            };
            window_Plugin.Icon = this.Icon;
            window_Plugin.SetConfig(config);
            window_Plugin.Show();
        }

        public void InitThemeSelector()
        {
            ThemeSelectorDefault.AddTransParentColor("TabItem.Background");
            ThemeSelectorDefault.AddTransParentColor("Window.Title.Background");
            ThemeSelectorDefault.AddTransParentColor("ListBoxItem.Background");
            ThemeSelectorDefault.AddTransParentColor("DataGrid.Header.Background");
            ThemeSelectorDefault.AddTransParentColor("DataGrid.Header.Hover.Background");
            ThemeSelectorDefault.AddTransParentColor("DataGrid.Row.Even.Background");
            ThemeSelectorDefault.AddTransParentColor("DataGrid.Row.Odd.Background");
            ThemeSelectorDefault.AddTransParentColor("DataGrid.Row.Hover.Background");
            ThemeSelectorDefault.SetThemeConfig(ConfigManager.Settings.ThemeIdx, ConfigManager.Settings.ThemeID);
            ThemeSelectorDefault.onThemeChanged += (ThemeIdx, ThemeID) =>
            {
                ConfigManager.Settings.ThemeIdx = ThemeIdx;
                ConfigManager.Settings.ThemeID = ThemeID;
                ConfigManager.Settings.Save();
            };
            ThemeSelectorDefault.onBackGroundImageChanged += (image) =>
            {
                ImageBackground.Source = image;
            };
            ThemeSelectorDefault.onSetBgColorTransparent += () =>
            {
                BorderTitle.Background = Brushes.Transparent;
            };

            ThemeSelectorDefault.onReSetBgColorBinding += () =>
            {
                BorderTitle.SetResourceReference(Control.BackgroundProperty, "Window.Title.Background");
            };

            ThemeSelectorDefault.InitThemes();
        }

        private void ShowUpgradeWindow(object sender, RoutedEventArgs e)
        {
            UpgradeHelper.OpenWindow(this);
        }

        private void OpenFeedBack(object sender, RoutedEventArgs e)
        {
            FileHelper.TryOpenUrl(UrlManager.FeedbackUrl);
        }

        private void OpenHelp(object sender, RoutedEventArgs e)
        {
            FileHelper.TryOpenUrl(UrlManager.HelpUrl);
        }

        private void OpenAppDir(object sender, RoutedEventArgs e)
        {
            FileHelper.TryOpenPath(AppDomain.CurrentDomain.BaseDirectory);
        }

        private void ShowAscii(object sender, RoutedEventArgs e)
        {
            Window_Ascii window_Ascii = new Window_Ascii((int)ConfigManager.Main.AsciiSelectedIndex);
            window_Ascii.OnSelectedChanged += (index) =>
            {
                ConfigManager.Main.AsciiSelectedIndex = index;
                ConfigManager.Main.Save();
            };
            window_Ascii.Icon = this.Icon;
            window_Ascii.Show();
        }

        private void HideSide(object sender, RoutedEventArgs e)
        {
            //MenuItem menuItem = sender as MenuItem;
            //if (menuItem.IsChecked)
            //{
            //    SideGridColumn.Width = new GridLength(200);
            //}
            //else
            //{
            //    SideGridColumn.Width = new GridLength(0);
            //    Border_SizeChanged(null, null);
            //}
        }
    }
}
