using SuperControls.Style;
using SuperToolBox.Entity;
using SuperUtils.IO;
using SuperUtils.Time;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Management;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SuperToolBox.ToolPages
{
    /// <summary>
    /// Interaction logic for DeviceInfo.xaml
    /// </summary>
    public partial class DeviceInfo : Page, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public DeviceInfo()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        /// <summary>
        /// 特殊的
        /// </summary>
        private static List<DriverInfo> DriverInfos { get; set; }

        private ObservableCollection<DriverInfo> _CurrentDriverInfos;

        public ObservableCollection<DriverInfo> CurrentDriverInfos
        {
            get { return _CurrentDriverInfos; }

            set
            {
                _CurrentDriverInfos = value;
                RaisePropertyChanged();
            }
        }

        private delegate void LoadDriverInfoDelegate(DriverInfo info);

        private void LoadDriverInfo(DriverInfo info)
        {
            CurrentDriverInfos.Add(info);
        }

        private void LoadInfo()
        {
            Task.Run(async () =>
            {
                foreach (BaseDeviceInfo deviceInfo in BaseDeviceInfo.ALL_DEVICCE_INFOS)
                {
                    Func<Dictionary<string, string>, Task<Dictionary<string, object>>> func = deviceInfo.HandleAction;
                    if (deviceInfo.ValueDict == null)
                        deviceInfo.ValueDict = await func(deviceInfo.InfoDictName);
                    Dispatcher.Invoke((Action)(() =>
                    {
                        string dataGridName = deviceInfo.DataGridName;
                        object v = rootGrid.FindName(dataGridName);
                        if (v != null && v is System.Windows.Controls.DataGrid dataGrid)
                        {
                            dataGrid.ItemsSource = null;
                            dataGrid.ItemsSource = deviceInfo.ValueDict;
                        }
                    }));
                }

                // 磁盘
                List<DriverInfo> driverInfos = await BaseDeviceInfo.LoadDiskInfo();
                CurrentDriverInfos = new ObservableCollection<DriverInfo>();
                foreach (DriverInfo driverInfo in driverInfos)
                {
                    await App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new LoadDriverInfoDelegate(LoadDriverInfo), driverInfo);
                }
            });
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInfo();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
