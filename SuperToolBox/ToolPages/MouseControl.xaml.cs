using SuperControls.Style;
using SuperToolBox.Entity;
using SuperToolBox.ViewModel;
using SuperToolBox.Windows;
using SuperUtils.Framework.Hooks;
using SuperUtils.Framework.WinNativeMethods;
using SuperUtils.Time;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
using static SuperUtils.Framework.Hooks.HookManager;
using static SuperUtils.Framework.WinNativeMethods.HotKey;

namespace SuperToolBox.ToolPages
{
    /// <summary>
    /// Interaction logic for MouseControl.xaml
    /// </summary>
    public partial class MouseControl : Page
    {

        private bool loaded = false;
        private VieModel_MouseControl vieModel;

        public MouseControl()
        {
            InitializeComponent();
            vieModel = new VieModel_MouseControl();
            this.DataContext = vieModel;
        }

        private void ShowMouseInfo(object sender, RoutedEventArgs e)
        {

        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // 捕获鼠标状态
            Console.WriteLine("Page_Loaded");
            loaded = true;

            // 键盘
            HookManager.KeyUp += HookManager_KeyUp;
            //HookManager.KeyDown += HookManager_KeyDown;
            //HookManager.KeyPress += HookManager_KeyPress;

            // 鼠标
            HookManager.MouseUp += HookManager_MouseUp;
            HookManager.MouseDown += HookManager_MouseDown;
            HookManager.MouseMove += HookManager_MouseMove;
            HookManager.MouseClick += HookManager_MouseClick;
            HookManager.MouseWheel += HookManager_MouseWheel;
            HookManager.MouseDoubleClick += HookManager_MouseDoubleClick;

            if (mainWindow == null)
            {
                foreach (Window item in App.Current.Windows)
                {
                    if (item.Name.Equals("main"))
                    {
                        mainWindow = (MainWindow)item;
                        if (mainWindow.HotKeySuccess1)
                        {
                            mainWindow.HotKeyHandler1 += () =>
                            {
                                StartCommands(null, null);
                            };
                        }
                        else
                        {
                            MessageBox.Show("注册热键失败：Ctrl+F11");
                        }
                        if (mainWindow.HotKeySuccess2)
                        {
                            mainWindow.HotKeyHandler2 += () =>
                            {
                                StopCommands(null, null);
                            };
                        }
                        else
                        {
                            MessageBox.Show("注册热键失败：Ctrl+F12");
                        }
                        break;
                    }
                }
            }
        }

        private MainWindow mainWindow { get; set; }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // 键盘
            HookManager.KeyUp -= HookManager_KeyUp;
            //HookManager.KeyDown -= HookManager_KeyDown;
            //HookManager.KeyPress -= HookManager_KeyPress;

            // 鼠标
            HookManager.MouseUp -= HookManager_MouseUp;
            HookManager.MouseDown -= HookManager_MouseDown;
            HookManager.MouseMove -= HookManager_MouseMove;
            HookManager.MouseClick -= HookManager_MouseClick;
            HookManager.MouseWheel -= HookManager_MouseWheel;
            HookManager.MouseDoubleClick -= HookManager_MouseDoubleClick;
        }


        private void HookManager_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Console.WriteLine(e.Button);
        }

        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            // 监听 F12
            Console.WriteLine(e.Key);
            if (e.Key == Key.F12 && !vieModel.RunningCommands)
            {

                if (dataGrid.Items != null && dataGrid.Items.Count > 0)
                {
                    List<MouseCommand> list = dataGrid.Items.Cast<MouseCommand>().ToList();
                    int idx = dataGrid.SelectedIndex;
                    if (idx >= 0 && idx < list.Count)
                    {
                        Point point = HookManager.GetMousePosition();
                        MouseCommand mouseCommand = list[idx];
                        mouseCommand.PointX = (int)point.X;
                        mouseCommand.PointY = (int)point.Y;
                        vieModel.UpdateByID(mouseCommand.ID);
                    }

                }
            }
            else if (e.Key == Key.F9 && !vieModel.RunningCommands)
            {
                StartCommands(null, null);
            }
            else if (e.Key == Key.F10 && vieModel.RunningCommands)
            {
                StopCommands(null, null);
            }
        }

        private void HookManager_KeyPress(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"KeyPress = {e.Key}");
        }
        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"KeyDown = {e.Key}");
        }

        private void HookManager_MouseMove(object sender, MouseEventExtArgs e)
        {
            //Console.WriteLine($"MouseMove ({e.X},{e.Y})");
            mousePointTextBlock.Text = $"({e.X},{e.Y})";
        }

        private void HookManager_MouseClick(object sender, MouseEventExtArgs e)
        {
            Console.WriteLine($"MouseClick {e.ChangedButton}");
        }


        public enum MouseType
        {
            Up,
            Down,
            Click,
            DoubleClick,
            WHEELUP,
            WHEELDOWN,
        }

        public class MouseTypeData
        {
            public MouseType type;
            public string msg;

            public MouseTypeData(MouseType type, string msg)
            {
                this.type = type;
                this.msg = msg;
            }
        }
        public class MouseButtonData
        {
            public HookManager.MouseButton button;
            public string msg;

            public MouseButtonData(HookManager.MouseButton button, string msg)
            {
                this.button = button;
                this.msg = msg;
            }
        }


        Dictionary<HookManager.MouseButton, string> MOUSE_BUTTON_DICT = new Dictionary<HookManager.MouseButton, string>()
        {
            {HookManager.MouseButton.None,"鼠标" },
            {HookManager.MouseButton.Left,"左键" },
            {HookManager.MouseButton.Middle,"中键" },
            {HookManager.MouseButton.Right,"右键" },
            {HookManager.MouseButton.XButton1,"XButton1" },
            {HookManager.MouseButton.XButton2,"XButton2" },
        };

        Dictionary<HookManager.MouseAction, string> MOUSE_ACTION_DICT = new Dictionary<HookManager.MouseAction, string>()
        {
            {HookManager.MouseAction.None,"移动" },
            {HookManager.MouseAction.Click,"单击" },
            {HookManager.MouseAction.DBClick,"双击" },
            {HookManager.MouseAction.Down,"按下" },
            {HookManager.MouseAction.Up,"弹起" },
            {HookManager.MouseAction.WHEELUP,"上滚" },
            {HookManager.MouseAction.WHEELDOWN,"下滚" },
        };

        private void ShowCurrentMouseStatus(HookManager.MouseAction action, HookManager.MouseButton button)
        {
            mouseDownTextBlock.Text = MOUSE_BUTTON_DICT[button] + MOUSE_ACTION_DICT[action];

        }
        private void HookManager_MouseUp(object sender, MouseEventExtArgs e)
        {
            ShowCurrentMouseStatus(HookManager.MouseAction.Up, e.ChangedButton);
        }

        private void HookManager_MouseDown(object sender, MouseEventExtArgs e)
        {
            ShowCurrentMouseStatus(HookManager.MouseAction.Down, e.ChangedButton);
        }


        private void HookManager_MouseDoubleClick(object sender, MouseEventExtArgs e)
        {
            ShowCurrentMouseStatus(HookManager.MouseAction.DBClick, e.ChangedButton);
        }
        private void HookManager_MouseMiddleDown(object sender, MouseEventExtArgs e)
        {
            ShowCurrentMouseStatus(HookManager.MouseAction.Down, e.ChangedButton);
        }


        private void HookManager_MouseWheel(object sender, MouseEventExtArgs e)
        {
            HookManager.MouseAction type = e.Delta > 0 ? HookManager.MouseAction.WHEELUP : HookManager.MouseAction.WHEELDOWN;
            ShowCurrentMouseStatus(type, e.ChangedButton);
        }


        private void SaveMouseCommands(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Tag != null)
            {
                long.TryParse(textBox.Tag.ToString(), out long id);
                vieModel.UpdateByID(id);
            }
        }

        private void DeleteCommand(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null && button.Tag != null)
            {
                long.TryParse(button.Tag.ToString(), out long id);
                vieModel.DeleteByID(id);
            }
        }

        private void ShowEditPopup(object sender, RoutedEventArgs e)
        {

        }

        private void NewMouseCommand(object sender, RoutedEventArgs e)
        {
            vieModel.AddCommand();
        }



        private void MouseEventSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!loaded) return;
            if (e.AddedItems == null || e.AddedItems.Count == 0) return;
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.Tag != null)
            {
                long.TryParse(comboBox.Tag.ToString(), out long id);
                vieModel.UpdateByID(id);
            }
        }

        private void SetMousePoint(object sender, RoutedEventArgs e)
        {
            MessageCard.Info("按下 F12 设置鼠标位置");
        }

        private void AddNewProject(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RenameProject(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteProjectInMenuItem(object sender, RoutedEventArgs e)
        {

        }

        private void RenameTextBoxLostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void DeleteProject(object sender, RoutedEventArgs e)
        {

        }

        private void StartCommands(object sender, RoutedEventArgs e)
        {
            if (vieModel.RunningCommands || dataGrid.Items == null || dataGrid.Items.Count == 0)
            {
                return;
            }
            vieModel.RunningCommands = true;
            List<MouseCommand> list = dataGrid.Items.Cast<MouseCommand>().ToList();
            statusTextBox.AppendText(DateHelper.Now() + Environment.NewLine);
            statusTextBox.ScrollToEnd();
            int idx = 1;
            Task.Run(async () =>
            {
                while (vieModel.RunningCommands)
                {
                    Dispatcher.Invoke(() =>
                    {
                        statusTextBox.AppendText($"[{DateHelper.Now()}] => 第 {idx} 次执行{Environment.NewLine}");
                        statusTextBox.ScrollToEnd();
                    });
                    foreach (MouseCommand command in list)
                    {
                        if (!vieModel.RunningCommands)
                            return;

                        HookManager.MouseButton button = command.MouseButton;
                        HookManager.MouseAction action = command.MouseAction;

                        if (button == HookManager.MouseButton.None)
                            action = HookManager.MouseAction.None;

                        if (action == HookManager.MouseAction.WHEELDOWN || action == HookManager.MouseAction.WHEELUP)
                        {
                            if (button != HookManager.MouseButton.Middle)
                            {
                                action = HookManager.MouseAction.None;
                                button = HookManager.MouseButton.None;
                            }
                        }
                        Dispatcher.Invoke(() =>
                        {
                            string cmdText = $"{MOUSE_BUTTON_DICT[button]}{MOUSE_ACTION_DICT[action]}";
                            statusTextBox.AppendText($"[{DateHelper.Now()}] => 移动到 ({command.PointX},{command.PointY}，执行动作：{cmdText}){Environment.NewLine}");
                            statusTextBox.ScrollToEnd();
                        });

                        // 执行命令

                        int x = command.PointX;
                        int y = command.PointY;
                        if (action == HookManager.MouseAction.None || button == HookManager.MouseButton.None)
                            MouseController.MoveTo(x, y);
                        else
                            MouseController.MouseEvent(x, y, button, action);
                        await Task.Delay(command.Delay);
                    }
                    idx++;
                }
            });


        }

        private void StopCommands(object sender, RoutedEventArgs e)
        {
            vieModel.RunningCommands = false;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine($"{(sender as Border).Tag} MouseDown");
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine($"{(sender as Border).Tag} MouseUp");
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Console.WriteLine($"{(sender as Border).Tag} MouseEnter");
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Console.WriteLine($"{(sender as Border).Tag} MouseLeave");
        }

        private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Console.WriteLine($"MouseWheel => {e.Delta}");

        }
    }
}
