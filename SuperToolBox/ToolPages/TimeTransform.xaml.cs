using SuperControls.Style;
using SuperToolBox.Config;
using SuperToolBox.Entity;
using SuperToolBox.Windows;
using SuperUtils.Common;
using SuperUtils.IO;
using SuperUtils.Time;
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
using System.Xml.Linq;

namespace SuperToolBox.ToolPages
{
    /// <summary>
    /// Interaction logic for DeviceInfo.xaml
    /// </summary>
    public partial class TimeTransform : Page
    {
        public TimeTransform()
        {
            InitializeComponent();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ConfigManager.TimeTransformConfig.TimeStampText = TimeStampTextBox.Text;
            ConfigManager.TimeTransformConfig.LocalTimeText = LocalTimeTextBox.Text;
            ConfigManager.TimeTransformConfig.Save();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCurrentInfo();
            RefreshCurrentTime();

        }

        public void RefreshCurrentTime()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Dispatcher.Invoke(() =>
                    {
                        currentTimeTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    });
                    Task.Delay(1000).Wait();
                }
            });
        }

        public async void LoadCurrentInfo()
        {
            string name = "时区";
            BaseDeviceInfo deviceInfo = BaseDeviceInfo.ALL_DEVICE_INFOS.FirstOrDefault(arg => arg.Name.Equals(name));
            if (deviceInfo == null)
                return;

            deviceInfo.ValueDict = new Dictionary<string, object>();

            Func<string[], Dictionary<string, string>, Task<Dictionary<string, object>>> func = deviceInfo.HandleAction;

            Dictionary<string, object> dict = await func(deviceInfo.Win32Name, deviceInfo.InfoDictName);
            if (dict != null && dict.Count > 0)
            {
                foreach (var item in dict)
                {
                    if (deviceInfo.ValueDict.ContainsKey(item.Key))
                        continue;

                    deviceInfo.ValueDict.Add(item.Key, item.Value);
                }
            }

            Dispatcher.Invoke((Action)(() =>
            {
                Dictionary<string, object> d = deviceInfo.ValueDict;
                if(d.ContainsKey("时区名称") && d["时区名称"] is string region) {
                    regionText.Text = region;
                }
            }));

        }

        private void TimeStampToLocalTime(object sender, RoutedEventArgs e)
        {
            bool success = long.TryParse(TimeStampTextBox.Text, out long timeStamp);
            if (!success)
            {
                LocalTimeTextBox.Text = "解析失败";
                return;
            }
            try
            {
                DateTime dateTime = DateHelper.UnixTimeStampToDateTime(timeStamp, TimeComboBox.SelectedIndex == 0);
                LocalTimeTextBox.Text = dateTime.ToLocalDate();
            }
            catch (Exception ex)
            {
                LocalTimeTextBox.Text = ex.Message.Replace(Environment.NewLine, "");
            }
        }

        private void LocalTimeToTimeStamp(object sender, RoutedEventArgs e)
        {
            bool success = DateTime.TryParse(LocalTimeTextBox.Text, out DateTime dt);
            if (!success)
            {
                TimeStampTextBox.Text = "解析失败";
            }
            else
            {
                TimeStampTextBox.Text = DateHelper.DateTimeToUnixTimeStamp(dt, TimeComboBox.SelectedIndex == 0).ToString();
            }
        }

        private void CopyCurrentTime(object sender, RoutedEventArgs e)
        {
            ClipBoard.TrySetDataObject(currentTimeTextBox.Text);
        }

        private void FillCurrentTime(object sender, RoutedEventArgs e)
        {
            string currentTIme = currentTimeTextBox.Text;
            LocalTimeTextBox.Text = currentTIme;
            LocalTimeToTimeStamp(null, null);
        }
    }
}
