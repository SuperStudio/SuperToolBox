using SuperUtils.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SuperToolBox.Entity
{
    public class BaseDeviceInfoView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private string _CPUID { get; set; }
        public string CPUID {
            get { return _CPUID; }
            set {
                _CPUID = value;
                RaisePropertyChanged();
            }
        }
        private string _CPUSpeed { get; set; }

        public string CPUSpeed {
            get { return _CPUSpeed; }
            set {
                _CPUSpeed = value;
                RaisePropertyChanged();
            }
        }

        private string _MemTotal { get; set; }

        public string MemTotal {
            get { return _MemTotal; }
            set {
                _MemTotal = value;
                RaisePropertyChanged();
            }
        }
        private string _MemUsed { get; set; }

        public string MemUsed {
            get { return _MemUsed; }
            set {
                _MemUsed = value;
                RaisePropertyChanged();
            }
        }
        private string _GraphicsID { get; set; }

        public string GraphicsID {
            get { return _GraphicsID; }
            set {
                _GraphicsID = value;
                RaisePropertyChanged();
            }
        }
        private string _GraphicsMem { get; set; }

        public string GraphicsMem {
            get { return _GraphicsMem; }
            set {
                _GraphicsMem = value;
                RaisePropertyChanged();
            }
        }
        private string _SysVersion { get; set; }

        public string SysVersion {
            get { return _SysVersion; }
            set {
                _SysVersion = value;
                RaisePropertyChanged();
            }
        }
        private string _SysType { get; set; }

        public string SysType {
            get { return _SysType; }
            set {
                _SysType = value;
                RaisePropertyChanged();
            }
        }

        public string ParseSysVersion(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            return value.Replace("Microsoft", "").Replace(SysType, "").Trim();
        }
        public static string ParseSysType(string value)
        {
            if (string.IsNullOrEmpty(value) || value.IndexOf(" ") <= 0)
                return "";
            return value.Split(' ').Last().Trim();
        }

        public static string ParseMem(object o)
        {
            if (o != null && o.ToString() is string str && long.TryParse(str, out long value) && value > 0)
                return ((long)value).ToProperFileSize();
            return "-";
        }



        public static string ParseCPUID(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "";
            if (name.IndexOf(" ") <= 0)
                return name;

            Match match = Regex.Match(name, @"[a-zA-Z0-9]+-[a-zA-Z0-9]+");
            if (match != null && match.Success && match.Groups.Count > 0)
                return match.Groups[0].Value.Trim();
            return name;
        }

        public static string ParseGPUID(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "";
            if (name.IndexOf(" ") <= 0)
                return name;
            string[] arr = name.Split(' ');
            return $"{arr[arr.Length - 2]} {arr[arr.Length - 1]}";
        }

        public static string ParseCpuSpeed(uint clockSpeed)
        {
            double speed = Math.Round((double)clockSpeed / 1000, 2);
            return $"{speed} GHz";
        }
    }
}
