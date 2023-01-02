using SuperUtils.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperToolBox.Entity
{
    public class DriverInfo
    {
        public DriverInfo()
        {
        }

        public string Name { get; set; }
        public string VolumeLabel { get; set; }
        public DriveType DriveType { get; set; }
        public string DriveFormat { get; set; }
        private long _TotalSize { get; set; }
        public long TotalSize
        {
            get { return _TotalSize; }
            set
            {
                _TotalSize = value;
                TotalSizeText = value.ToProperFileSize();
            }
        }
        private long _AvailableFreeSpace;
        public long AvailableFreeSpace
        {
            get { return _AvailableFreeSpace; }
            set
            {
                _AvailableFreeSpace = value;
                AvailableFreeSpaceText = value.ToProperFileSize();
            }
        }
        public long TotalFreeSpace { get; set; }
        private long _UsedSpace;
        public long UsedSpace
        {
            get { return _UsedSpace; }
            set
            {
                _UsedSpace = value;
                UsedSpaceText = value.ToProperFileSize();
                UseSpaceRate = $"{Math.Round(100 * (double)value / (double)TotalSize, 2)} %";
            }
        }
        public string RootDirectory { get; set; }
        public string UsedSpaceText { get; set; }
        public string TotalSizeText { get; set; }
        public string AvailableFreeSpaceText { get; set; }
        public string UseSpaceRate { get; set; }
    }
}
