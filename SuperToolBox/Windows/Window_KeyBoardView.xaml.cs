using SuperToolBox.Config;
using SuperUtils.Common;
using SuperUtils.Framework.Hooks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperToolBox.Windows
{
    /// <summary>
    /// Interaction logic for Window_KeyBoardView.xaml
    /// </summary>
    public partial class Window_KeyBoardView : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Window_KeyBoardView()
        {
            InitializeComponent();
            this.DataContext = this;
            Init();
        }

        private string _CurrentKey = "键盘";
        public string CurrentKey
        {
            get { return _CurrentKey; }
            set
            {
                _CurrentKey = value;
                RaisePropertyChanged();
            }
        }
        private bool _Shift = false;
        public bool Shift
        {
            get { return _Shift; }
            set
            {
                _Shift = value;
                RaisePropertyChanged();
            }
        }
        private bool _Ctrl = false;
        public bool Ctrl
        {
            get { return _Ctrl; }
            set
            {
                _Ctrl = value;
                RaisePropertyChanged();
            }
        }
        private bool _Alt = false;
        public bool Alt
        {
            get { return _Alt; }
            set
            {
                _Alt = value;
                RaisePropertyChanged();
            }
        }

        public void Init()
        {
            HookManager.KeyUp += HookManager_KeyUp;
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.KeyPress += HookManager_KeyPress;
        }

        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"KeyUp = {e.Key}");
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                Shift = false;
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                Ctrl = false;
            if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
                Alt = false;
        }

        private void HookManager_KeyPress(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"KeyPress = {e.Key}");


        }
        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"KeyDown = {e.Key}");
            CurrentKey = KeyBoardHelper.KeyToString(e.Key);
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                Shift = true;
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                Ctrl = true;

            if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
                Alt = true;
        }


        bool canDragWindow = false;

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && canDragWindow)
            {
                this.DragMove();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            canDragWindow = true;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canDragWindow = false;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            double height = SystemParameters.PrimaryScreenHeight;
            double width = SystemParameters.PrimaryScreenWidth;
            double margin = 20;
            this.Left = width - this.Width - margin;
            this.Top = height - this.Height - margin * 2;

        }

        private void CloseMenu(object sender, RoutedEventArgs e)
        {
            ConfigManager.KeyBoardConfig.ShowKeyView = false;
        }
    }
}
