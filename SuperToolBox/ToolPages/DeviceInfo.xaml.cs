using SuperControls.Style;
using SuperToolBox.Entity;
using SuperUtils.IO;
using SuperUtils.Time;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public ObservableCollection<DriverInfo> CurrentDriverInfos {
            get { return _CurrentDriverInfos; }

            set {
                _CurrentDriverInfos = value;
                RaisePropertyChanged();
            }
        }
        private BaseDeviceInfoView _InfoView;

        public BaseDeviceInfoView InfoView {
            get { return _InfoView; }

            set {
                _InfoView = value;
                RaisePropertyChanged();
            }
        }

        private bool _LoadingBasicView = true;

        public bool LoadingBasicView {
            get { return _LoadingBasicView; }

            set {
                _LoadingBasicView = value;
                RaisePropertyChanged();
            }
        }

        private delegate void LoadDriverInfoDelegate(DriverInfo info);

        private void LoadDriverInfo(DriverInfo info)
        {
            CurrentDriverInfos.Add(info);
        }



        private void LoadInfo(string name = "")
        {
            Task.Run(async () => {
                if (name.Equals("概要")) {
                    LoadBasic();
                }


                foreach (BaseDeviceInfo deviceInfo in BaseDeviceInfo.ALL_DEVICE_INFOS) {
                    if (!deviceInfo.Name.Equals(name))
                        continue;
                    Dispatcher.Invoke((Action)(() => {
                        loadingLine.Visibility = Visibility.Visible;
                    }));
                    Func<string[], Dictionary<string, string>, Task<Dictionary<string, object>>> func = deviceInfo.HandleAction;
                    if (deviceInfo.ValueDict == null)
                        deviceInfo.ValueDict = await func(deviceInfo.Win32Name, deviceInfo.InfoDictName);
                    Dispatcher.Invoke((Action)(() => {
                        string dataGridName = deviceInfo.DataGridName;
                        object v = rootGrid.FindName(dataGridName);
                        if (v != null && v is System.Windows.Controls.DataGrid dataGrid) {
                            dataGrid.ItemsSource = null;
                            dataGrid.ItemsSource = deviceInfo.ValueDict;
                        }
                        loadingLine.Visibility = Visibility.Collapsed;
                    }));
                }


            });
        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is TabControl tabControl &&
                !tabControl.IsLoaded ||
                e.AddedItems == null ||
                e.AddedItems.Count == 0)
                return;

            TabItem tabItem = e.AddedItems[0] as TabItem;
            if (tabItem != null &&
                tabItem.Header != null &&
                tabItem.Header.ToString() is string header) {
                LoadInfo(header);
            }
        }


        private void LoadBasic()
        {
            Task.Run(async () => {
                if (CurrentDriverInfos != null)
                    return;
                try {


                    List<DriverInfo> driverInfos = await BaseDeviceInfo.LoadDiskInfo();
                    CurrentDriverInfos = new ObservableCollection<DriverInfo>();
                    foreach (DriverInfo driverInfo in driverInfos) {
                        await App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new LoadDriverInfoDelegate(LoadDriverInfo), driverInfo);
                    }
                    LoadingBasicView = true;

                    InfoView = new BaseDeviceInfoView();

                    // CPU
                    ManagementObject data = new ManagementObjectSearcher("select * from Win32_Processor").Get().Cast<ManagementObject>().First();
                    InfoView.CPUID = BaseDeviceInfoView.ParseCPUID((string)data["Name"]);
                    InfoView.CPUSpeed = BaseDeviceInfoView.ParseCpuSpeed((uint)data["MaxClockSpeed"]);

                    // 内存
                    InfoView.MemUsed = "可用内存 " + BaseDeviceInfoView.ParseMem(SuperUtils.Systems.SysInfo.GetPhysicalAvailableMemoryInMiB() * 1024 * 1024);
                    InfoView.MemTotal = BaseDeviceInfoView.ParseMem(SuperUtils.Systems.SysInfo.GetTotalMemoryInMiB() * 1024 * 1024);

                    // 显卡
                    data = new ManagementObjectSearcher("select * from Win32_VideoController").Get().Cast<ManagementObject>().First();
                    InfoView.GraphicsID = BaseDeviceInfoView.ParseGPUID((string)data["Name"]);
                    InfoView.GraphicsMem = BaseDeviceInfoView.ParseMem(data["AdapterRAM"]);

                    // 操作系统
                    data = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().Cast<ManagementObject>().First();
                    InfoView.SysType = BaseDeviceInfoView.ParseSysType((string)data["Caption"]);
                    InfoView.SysVersion = InfoView.ParseSysVersion((string)data["Caption"]);
                } catch (Exception ex) {
                    App.Logger.Error(ex);
                } finally {
                    LoadingBasicView = false;
                }

            });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBasic();
        }

    }
}
