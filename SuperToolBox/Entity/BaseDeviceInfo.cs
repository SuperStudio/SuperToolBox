using Microsoft.Win32;
using SuperUtils.IO;
using SuperUtils.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SuperToolBox.Entity
{
    public class BaseDeviceInfo
    {

        #region "英文转中文表"

        // 注意，static 初始化由上之下，引用的某个 static 变量还未初始化的话，为 NULL

        private static Dictionary<string, string> SysInfoDictName = new Dictionary<string, string>()
        {
            {"UUID","计算机 UUID" },
            {"Caption","操作系统名称" },
            {"InstallDate","操作系统安装日期" },
            {"SerialNumber","产品ID" },
            {"Version","操作系统版本" },
            {"BuildNumber","构建版本" },
            {"BuildType","构建类型" },
            {"CodeSet","字符集" },
            {"CountryCode","国家代码" },
            {"CurrentTimeZone","时区" },
            {"Debug","调试版本" },
            {"EncryptionLevel","加密级别" },
            {"FreePhysicalMemory","可用物理内存" },
            {"FreeSpaceInPagingFiles","可用页大小" },
            {"FreeVirtualMemory","可用虚拟内存" },
            {"TotalVirtualMemorySize","总虚拟内存" },
            {"TotalVisibleMemorySize","总可见内存" },
            {"Locale","语言标识符" },
            {"Manufacturer","制造商" },
            {"MaxNumberOfProcesses","MaxNumberOfProcesses" },
            {"MaxProcessMemorySize","MaxProcessMemorySize" },
            {"NumberOfProcesses","NumberOfProcesses" },
            {"NumberOfUsers","NumberOfUsers" },
            {"SizeStoredInPagingFiles","SizeStoredInPagingFiles" },
            {"SuiteMask","SuiteMask" },
        };
        private static Dictionary<string, string> CpuInfoDictName = new Dictionary<string, string>()
        {
            {"Name","名称" },
            {"CurrentClockSpeed","主频" },
            {"L1Cache","L1缓存" },
            {"L2CacheSize","L2缓存" },
            {"L3CacheSize","L3缓存" },
            {"Manufacturer","制造商" },
            {"NumberOfCores","内核" },
            {"NumberOfEnabledCore","已启用的内核数" },
            {"NumberOfLogicalProcessors","逻辑处理器" },
            {"VirtualizationFirmwareEnabled","虚拟化固件" },
            {"VMMonitorModeExtensions","VM监视器模式扩展" },
            {"AddressWidth","地址宽度" },
            {"Caption","详情" },
            {"DataWidth","数据宽度" },
            {"CurrentVoltage","当前电压" },
            {"DeviceID","设备ID" },
            {"ProcessorId","产品ID" },
            {"PowerManagementSupported","是否支持电源管理" },
        };

        private static Dictionary<string, string> BaseBoardDictName = new Dictionary<string, string>()
        {
            {"Product","产品" },
            {"HostingBoard","母版" },
            {"HotSwappable","热插拔" },
            {"Manufacturer","制造商" },
            {"Removable","可移除" },
            {"Replaceable","可替换" },
            {"SerialNumber","序列号" },
            {"Version","版本号" },
        };

        private static Dictionary<string, string> VideoInfoDictName = new Dictionary<string, string>()
        {
            {"Name","名称" },
            {"AdapterRAM","显存大小" },
            {"CurrentHorizontalResolution","水平分辨率" },
            {"CurrentVerticalResolution","垂直分辨率" },
            {"CurrentNumberOfColors","当前颜色数目" },
            {"CurrentBitsPerPixel","像素流水线" },
            {"CurrentRefreshRate","当前刷新率" },
            {"DriverDate","驱动程序日期" },
            {"DriverVersion","驱动程序版本" },
            {"InstalledDisplayDrivers","已安装的驱动路径" },
            {"MaxRefreshRate","最高刷新率" },
            {"MinRefreshRate","最低刷新率" },
            {"Caption","详情" },
            {"DeviceID","设备ID" },
            {"PNPDeviceID","产品ID" },
            {"VideoArchitecture","串流多重处理器" },

            {"ColorPlanes","色彩层数" },
            {"VideoMode","显示模式" },
        };


        private static Dictionary<string, string> KeyboardInfoDictName = new Dictionary<string, string>()
        {
            {"Name","名称" },
            {"NumberOfFunctionKeys","功能键数目" },
            {"DeviceID","设备ID" },
            {"PNPDeviceID","产品ID" },
            {"Layout","Layout" },
        };

        private static Dictionary<string, string> BiosInfoDictName = new Dictionary<string, string>()
        {
            {"Manufacturer","制造商" },
            {"SMBIOSBIOSVersion","SMBIOS 版本号" },
            {"ReleaseDate","生产日期" },
            {"CurrentLanguage","语言" },
            {"PrimaryBIOS","主Bios" },
            {"SMBIOSMajorVersion","SMBIOS 主版本号" },
            {"SMBIOSMinorVersion","SMBIOS 副版本号" },
            {"SystemBiosMajorVersion","系统 Bios 主版本号" },
            {"SystemBiosMinorVersion","系统 Bios 副版本号" },
            {"SMBIOS","SMBIOS" },
            {"Version","版本" },
            {"EmbeddedControllerMajorVersion","嵌入式控制器主版本" },
            {"EmbeddedControllerMinorVersion","嵌入式控制器副版本" },
        };
        private static Dictionary<string, string> MonitorInfoDictName = new Dictionary<string, string>()
        {
            {"Name","名称" },
            {"PNPDeviceID","设备ID" },
            {"DeviceID","DeviceID" },
            {"MonitorManufacturer","制造商" },
            {"PixelsPerXLogicalInch","水平像素大小" },
            {"PixelsPerYLogicalInch","垂直像素大小" },
            {"Availability","Availability" },
        };

        private static Dictionary<string, string> DiskDriveInfoDictName = new Dictionary<string, string>()
        {
            {"Caption","详情" },
            {"Size","容量" },
            {"TotalCylinders","柱面总数" },
            {"TotalHeads","磁头数目" },
            {"TotalSectors","扇区总数" },
            {"TotalTracks","磁道总数" },
            {"TracksPerCylinder","每柱面的磁道数目" },
            {"SectorsPerTrack","每磁道扇区数" },
            {"SerialNumber","序列号" },
            {"BytesPerSector","每扇区大小（字节）" },
            {"Description","说明" },
            {"PNPDeviceID","设备ID" },
            {"DeviceID","DeviceID" },
            {"Manufacturer","制造商" },
            {"FirmwareRevision","修订" },
            {"InterfaceType","接口类型" },
            {"MediaLoaded","可访问的文件系统" },
            {"MediaType","可访问的文件系统类型" },
            {"Partitions","多扇区" },
            {"SCSIBus","SCSIBus" },
            {"SCSILogicalUnit","SCSILogicalUnit" },
            {"SCSIPort","SCSIPort" },
            {"Signature","签名" },
        };
        private static Dictionary<string, string> TimezoneInfoDictName = new Dictionary<string, string>()
        {
            {"Caption","时区名称" },
            {"DaylightName","名称" },
            {"StandardName","标准时间名" },
            {"Bias","偏差（分钟）" },
        };

        #endregion

        /// <summary>
        /// 未加入的
        /// Win32_CodecFile
        /// Win32_CurrentProbe
        /// Win32_Desktop
        /// Win32_DeviceMemoryAddress
        /// Win32_DiskPartition
        /// Win32_DriverForDevice
        /// Win32_Environment
        /// Win32_Fan
        /// Win32_Group
        /// Win32_IP4RouteTable
        /// Win32_ComputerSystemProduct
        /// Win32_PageFileUsage
        /// </summary>

        public static List<BaseDeviceInfo> ALL_DEVICCE_INFOS = new List<BaseDeviceInfo>()
        {
            { new BaseDeviceInfo("系统信息",    new string[]{ "Win32_OperatingSystem" },   LoadSysBasicInfo,     SysInfoDictName,           "dataGrid1") },
            { new BaseDeviceInfo("CPU",      new string[] {  "Win32_Processor"},                 LoadCpuInfo,          CpuInfoDictName,           "dataGrid2") },
            { new BaseDeviceInfo("主板",      new string[]{  "Win32_BaseBoard"},                 LoadNormalInfo,    BaseBoardDictName,         "dataGrid3") },
            { new BaseDeviceInfo("显卡",      new string[]{  "Win32_VideoController",
                                                        "Win32_DisplayControllerConfiguration"}, LoadVideoInfo,        VideoInfoDictName,        "dataGrid4") },
            { new BaseDeviceInfo("键盘",      new string[]{  "Win32_Keyboard"},                   LoadNormalInfo,     KeyboardInfoDictName,     "dataGrid5") },
            { new BaseDeviceInfo("BIOS",      new string[]{  "Win32_BIOS"},                     LoadNormalInfo,     BiosInfoDictName,     "dataGrid6") },
            { new BaseDeviceInfo("显示器",     new string[]{  "Win32_DesktopMonitor"},           LoadNormalInfo,     MonitorInfoDictName,     "dataGrid7") },
            { new BaseDeviceInfo("系统硬盘",      new string[]{  "Win32_DiskDrive"},                 LoadNormalInfo,     DiskDriveInfoDictName,     "dataGrid8") },
            { new BaseDeviceInfo("时区",      new string[]{  "Win32_TimeZone"},                  LoadNormalInfo,     TimezoneInfoDictName,     "dataGrid9") },
        };

        public async void PrintAllSysInfo()
        {
            string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sysinfo.txt");
            long len = WIN32_CLASSES.Length;
            for (int i = 0; i < len; i++)
            {
                string item = WIN32_CLASSES[i];
                Console.WriteLine($"{i + 1}/{len} {Math.Round(100 * ((double)i + 1) / (double)len, 2)}% {item}");
                Dictionary<string, object> dictionary = await DeviceInformation(item);
                if (dictionary != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append($"{Environment.NewLine}===== {item} ====={Environment.NewLine}");
                    foreach (string key in dictionary.Keys)
                    {
                        builder.Append($"{key.PadRight(50)} => {dictionary[key]}{Environment.NewLine}");
                    }
                    builder.Append($"===== {item} ====={Environment.NewLine}{Environment.NewLine}");
                    FileHelper.TryAppendToFile(filepath, builder.ToString());
                }

            }
        }

        public static void PrintDeviceInfo(string title, Dictionary<string, object> dict)
        {
            Console.WriteLine($"========== {title} ==========");
            foreach (var key in dict.Keys)
            {
                Console.WriteLine($"{key.PadRight(10)} = {dict[key]}");
            }
            Console.WriteLine($"========== {title} ==========");
        }


        private static async Task<Dictionary<string, object>> DeviceInformation(string stringIn)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                try
                {
                    ManagementClass managementClass = new ManagementClass(stringIn);
                    //Create a ManagementObjectCollection to loop through
                    ManagementObjectCollection ManagemenobjCol = managementClass.GetInstances();
                    //Get the properties in the class
                    PropertyDataCollection properties = managementClass.Properties;
                    foreach (ManagementObject obj in ManagemenobjCol)
                    {
                        foreach (PropertyData property in properties)
                        {
                            try
                            {
                                if (obj.Properties[property.Name].Value != null)
                                    result.Add(property.Name, obj.Properties[property.Name].Value.ToString());
                                else
                                    result.Add(property.Name, "[NULL]");
                            }
                            catch
                            {
                                //Add codes to manage more informations
                            }
                        }
                    }
                }
                catch
                {
                    //Win 32 Classes Which are not defined on client system
                }
                return result;
            });
        }


        private static async Task<Dictionary<string, object>> LoadSysBasicInfo(string[] names, Dictionary<string, string> dictName)
        {
            return await Task.Run(async () =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("操作系统", Environment.OSVersion);


                string releaseId = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString();
                string UBR = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "UBR", "").ToString();
                result.Add("操作系统版本号", releaseId);

                if (Environment.Is64BitOperatingSystem)
                    result.Add("位数", "64");
                else
                    result.Add("位数", "32");

                Dictionary<string, object> dict = await LoadNormalInfo(names, dictName);
                foreach (var key in dict.Keys)
                {
                    if (!result.ContainsKey(key))
                        result.Add(key, dict[key]);
                }
                if (result.ContainsKey("时区"))
                {
                    int.TryParse(result["时区"].ToString(), out int CurrentTimeZone);
                    if (CurrentTimeZone >= 0)
                        result["时区"] = $"+{CurrentTimeZone / 60} H";
                    else
                        result["时区"] = $"-{Math.Abs(CurrentTimeZone) / 60} H";
                }
                if (result.ContainsKey("操作系统安装日期"))
                {
                    string dateString = result["操作系统安装日期"].ToString().Substring(0, "20211227212333".Length);
                    dateString = dateString.Insert(12, ":");
                    dateString = dateString.Insert(10, ":");
                    dateString = dateString.Insert(8, " ");
                    dateString = dateString.Insert(6, "-");
                    dateString = dateString.Insert(4, "-");
                    result["操作系统安装日期"] = dateString;
                }
                if (result.ContainsKey("操作系统版本"))
                {
                    result["操作系统版本"] += "." + UBR;
                }

                string[] sizeList = { "可用页大小", "可用物理内存", "可用虚拟内存", "总虚拟内存", "总可见内存" };
                foreach (var sizeString in sizeList)
                {
                    if (result.ContainsKey(sizeString))
                    {
                        long.TryParse(result[sizeString].ToString(), out long size);
                        size *= 1000;
                        result[sizeString] = size.ToProperFileSize();
                    }
                }

                result.Add("逻辑处理器", Environment.ProcessorCount);
                result.Add("登陆域", Environment.UserDomainName);
                result.Add("用户名", Environment.UserName);
                result.Add("计算机名称", Environment.MachineName);
                result.Add("系统页大小", Environment.SystemPageSize);
                result.Add("用户交互模式", Environment.UserInteractive);
                result.Add(".Net 版本", Environment.Version);
                //DeviceInfoDict.Add("物理内存", Environment.WorkingSet);
                var timespan = TimeSpan.FromMilliseconds(Environment.TickCount);
                result.Add("系统运行时间", DateHelper.ToReadableTime((long)timespan.TotalMilliseconds));
                result.Add("系统文件夹", Environment.SystemDirectory);




                return result;
            });
        }

        public static async Task<List<DriverInfo>> LoadDiskInfo()
        {
            return await Task.Run(() =>
            {
                List<DriverInfo> result = new List<DriverInfo>();
                //Drives
                foreach (System.IO.DriveInfo info in System.IO.DriveInfo.GetDrives())
                {
                    try
                    {
                        DriverInfo driverInfo = new DriverInfo();
                        driverInfo.Name = info.Name.Replace(":\\", "");
                        driverInfo.VolumeLabel = info.VolumeLabel;
                        driverInfo.DriveType = info.DriveType;
                        driverInfo.DriveFormat = info.DriveFormat;
                        driverInfo.TotalSize = info.TotalSize;
                        driverInfo.AvailableFreeSpace = info.AvailableFreeSpace;
                        driverInfo.TotalFreeSpace = info.TotalFreeSpace;
                        driverInfo.UsedSpace = info.TotalSize - info.AvailableFreeSpace;
                        driverInfo.RootDirectory = info.RootDirectory.FullName;
                        result.Add(driverInfo);
                    }
                    catch
                    {
                    }
                }
                return result;
            });

        }

        public static uint GetCacheSizes(ushort level)
        {
            ManagementClass mc = new ManagementClass("Win32_CacheMemory");
            ManagementObjectCollection moc = mc.GetInstances();
            List<uint> cacheSizes = new List<uint>(moc.Count);

            cacheSizes.AddRange(moc
              .Cast<ManagementObject>()
              .Where(p => (ushort)(p.Properties["Level"].Value) == level)
              .Select(p => (uint)(p.Properties["MaxCacheSize"].Value)));
            if (cacheSizes.Count > 0)
                return cacheSizes[0];
            else
                return 0;
        }

        public static async Task<Dictionary<string, object>> LoadCpuInfo(string[] names, Dictionary<string, string> dictName)
        {
            //if (CpuInfoDict != null) return true;
            Dictionary<string, object> dictionary = await GetAllInfoByNames(names);
            //BaseDeviceInfo.PrintDeviceInfo("123", dictionary);

            // 缓存
            //List<int> list = CacheProcessor.GetPerCoreCacheSizes();
            dictionary.Add("L1Cache", "-");
            //dictionary.Add("L2Cache", list[1]);
            //dictionary.Add("L3Cache", list[2]);

            if (dictName == null) return dictionary;
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in dictName.Keys)
            {
                if (dictionary.ContainsKey(key))
                {
                    if (key.Equals("CurrentClockSpeed"))
                    {
                        result.Add(dictName[key], $"{dictionary[key]} MHz");
                    }
                    else if ((key.Equals("L3CacheSize")) && dictionary[key] != null)
                    {
                        double.TryParse(dictionary[key].ToString(), out double value);
                        result.Add(dictName[key], $"{value / 1024} MB");
                    }
                    else if ((key.Equals("L1CacheSize") || key.Equals("L2CacheSize")))
                    {
                        result.Add(dictName[key], $"-");
                    }
                    else if (key.Equals("CurrentVoltage") || key.Equals("L3CacheSize"))
                    {
                        result.Add(dictName[key], $"{dictionary[key]} V");
                    }
                    else
                    {
                        result.Add(dictName[key], dictionary[key]);
                    }
                }
            }

            return result;
        }


        private async static Task<Dictionary<string, object>> GetAllInfoByNames(string[] names)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (names?.Length == 0) return result;
            foreach (var name in names)
            {
                Dictionary<string, object> dict = await DeviceInformation(name);
                foreach (var key in dict.Keys)
                {
                    if (result.ContainsKey(key) && result[key].ToString().Equals("[NULL]"))
                        result[key] = dict[key];
                    else if (!result.ContainsKey(key))
                        result.Add(key, dict[key]);
                }
            }
            return result;
        }

        public static async Task<Dictionary<string, object>> LoadVideoInfo(string[] names, Dictionary<string, string> dictName)
        {
            Dictionary<string, object> dictionary = await GetAllInfoByNames(names);
            if (dictName == null) return dictionary;
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in dictName.Keys)
            {
                if (dictionary.ContainsKey(key))
                {
                    if (key.Equals("InstalledDisplayDrivers") && dictionary[key] != null)
                    {
                        result.Add(dictName[key], $"{dictionary[key].ToString().Replace(",", Environment.NewLine)}");
                    }
                    else if (key.Equals("AdapterRAM") && dictionary[key] != null)
                    {
                        long.TryParse(dictionary[key].ToString(), out long value);
                        result.Add(dictName[key], $"{value.ToProperFileSize()}");
                    }
                    else if (key.Equals("DriverDate") && dictionary[key] != null)
                    {
                        result.Add(dictName[key], $"{dictionary[key].ToString().Substring(0, "20220721".Length)}");
                    }
                    else
                    {
                        result.Add(dictName[key], dictionary[key]);
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// 通用加载信息方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dictName"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, object>> LoadNormalInfo(string[] names, Dictionary<string, string> dictName)
        {
            Dictionary<string, object> dictionary = await GetAllInfoByNames(names);
            if (dictName == null) return dictionary;
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in dictName.Keys)
            {
                if (dictionary.ContainsKey(key))
                {
                    if (key.Equals("ReleaseDate") && dictionary[key] != null)
                    {
                        result.Add(dictName[key], $"{dictionary[key].ToString().Substring(0, "20220721".Length)}");
                    }
                    else if (key.Equals("Size") && dictionary[key] != null)
                    {
                        long.TryParse(dictionary[key].ToString(), out long value);
                        result.Add(dictName[key], $"{value.ToProperFileSize()}");
                    }
                    else
                    {
                        result.Add(dictName[key], dictionary[key]);
                    }

                }

            }

            return result;
        }



        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        public string[] Win32Name { get; set; }


        /// <summary>
        /// 数据以字典形式存储
        /// </summary>
        public Dictionary<string, object> ValueDict { get; set; }


        /// <summary>
        /// 英文到中文转换规则
        /// </summary>
        public Dictionary<string, string> InfoDictName { get; set; }

        public Func<string[], Dictionary<string, string>, Task<Dictionary<string, object>>> HandleAction { get; set; }

        private System.Windows.Controls.DataGrid DataGrid { get; set; }

        public string DataGridName { get; set; }


        public BaseDeviceInfo(string name, string[] win32Name, Func<string[], Dictionary<string, string>, Task<Dictionary<string, object>>> handleAction,
            Dictionary<string, string> infoDictName, string dataGridName)
        {
            Name = name;
            InfoDictName = infoDictName;
            Win32Name = win32Name;
            HandleAction = handleAction;
            DataGridName = dataGridName;
        }

        /// <summary>
        /// 注释的获取失败
        /// <br></br>
        /// 参考：https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/computer-system-hardware-classes
        /// </summary>
        static string[] WIN32_CLASSES =
        {
            "Win32_1394Controller",
            "Win32_1394ControllerDevice",
            //"Win32_AccountSID",
            //"Win32_ActionCheck",
            "Win32_ActiveRoute",
            "Win32_AllocatedResource",
            "Win32_ApplicationCommandLine",
            "Win32_ApplicationService",
            "Win32_AssociatedBattery",
            "Win32_AssociatedProcessorMemory",
            "Win32_AutochkSetting",
            "Win32_BaseBoard",
            "Win32_Battery",
            //"Win32_Binary",
            "Win32_BindImageAction",
            "Win32_BIOS",
            "Win32_BootConfiguration",
            "Win32_Bus"+
            "Win32_CacheMemory",
            "Win32_CDROMDrive",
            //"Win32_CheckCheck",
            "Win32_CIMLogicalDeviceCIMDataFile",
            //"Win32_ClassicCOMApplicationClasses",
            //"Win32_ClassicCOMClass",
            //"Win32_ClassicCOMClassSetting",
            //"Win32_ClassicCOMClassSettings",
            "Win32_ClassInforAction",
            "Win32_ClientApplicationSetting",
            "Win32_CodecFile",
            "Win32_COMApplicationSettings",
            "Win32_COMClassAutoEmulator",
            "Win32_ComClassEmulator",
            //"Win32_CommandLineAccess",
            "Win32_ComponentCategory",
            "Win32_ComputerSystem",
            "Win32_ComputerSystemProcessor",
            "Win32_ComputerSystemProduct",
            "Win32_ComputerSystemWindowsProductActivationSetting",
            //"Win32_Condition",
            "Win32_ConnectionShare",
            "Win32_ControllerHastHub",
            //"Win32_CreateFolderAction",
            "Win32_CurrentProbe",
            "Win32_DCOMApplication",
            "Win32_DCOMApplicationAccessAllowedSetting",
            "Win32_DCOMApplicationLaunchAllowedSetting",
            //"Win32_DCOMApplicationSetting",
            "Win32_DependentService",
            "Win32_Desktop",
            "Win32_DesktopMonitor",
            "Win32_DeviceBus",
            "Win32_DeviceMemoryAddress",
            //"Win32_Directory",
            //"Win32_DirectorySpecification",
            "Win32_DiskDrive",
            "Win32_DiskDrivePhysicalMedia",
            "Win32_DiskDriveToDiskPartition",
            "Win32_DiskPartition",
            "Win32_DiskQuota",
            "Win32_DisplayConfiguration",
            "Win32_DisplayControllerConfiguration",
            "Win32_DMAChanner",
            "Win32_DriverForDevice",
            "Win32_DriverVXD",
            //"Win32_DuplicateFileAction",
            "Win32_Environment",
            //"Win32_EnvironmentSpecification",
            //"Win32_ExtensionInfoAction",
            "Win32_Fan",
            //"Win32_FileSpecification",
            "Win32_FloppyController",
            "Win32_FloppyDrive",
            "Win32_FontInfoAction",
            "Win32_Group",
            "Win32_GroupDomain",
            "Win32_GroupUser",
            "Win32_HeatPipe",
            "Win32_IDEController",
            "Win32_IDEControllerDevice",
            //"Win32_ImplementedCategory",
            "Win32_InfraredDevice",
            "Win32_IniFileSpecification",
            //"Win32_InstalledSoftwareElement",
            "Win32_IP4PersistedRouteTable",
            "Win32_IP4RouteTable",
            //"Win32_IRQResource",
            "Win32_Keyboard",
            //"Win32_LaunchCondition",
            "Win32_LoadOrderGroup",
            "Win32_LoadOrderGroupServiceDependencies",
            "Win32_LoadOrderGroupServiceMembers",
            "Win32_LocalTime",
            "Win32_LoggedOnUser",
            "Win32_LogicalDisk",
            "Win32_LogicalDiskRootDirectory",
            "Win32_LogicalDiskToPartition",
            "Win32_LogicalFileAccess",
            "Win32_LogicalFileAuditing",
            "Win32_LogicalFileGroup",
            "Win32_LogicalFileOwner",
            "Win32_LogicalFileSecuritySetting",
            "Win32_LogicalMemoryConfiguration",
            "Win32_LogicalProgramGroup",
            "Win32_LogicalProgramGroupDirectory",
            "Win32_LogicalProgramGroupItem",
            "Win32_LogicalProgramGroupItemDataFile",
            "Win32_LogicalShareAccess",
            "Win32_LogicalShareAuditing",
            "Win32_LogicalShareSecuritySetting",
            "Win32_LogonSession",
            "Win32_LogonSessionMappedDisk",
            "Win32_MappedLogicalDisk",
            "Win32_MemoryArray",
            "Win32_MemoryArrayLocation",
            "Win32_MemoryDevice",
            "Win32_MemoryDeviceArray",
            "Win32_MemoryDeviceLocation",
            "Win32_MIMEInfoAction",
            "Win32_MotherboardDevice",
            "Win32_MoveFileAction",
            "Win32_NamedJobObject",
            "Win32_NamedJobObjectActgInfo",
            "Win32_NamedJobObjectLimit",
            "Win32_NamedJobObjectLimitSetting",
            "Win32_NamedJobObjectProcess",
            "Win32_NamedJobObjectSecLimit",
            "Win32_NamedJobObjectSecLimitSetting",
            "Win32_NamedJobObjectStatistics",
            "Win32_NetworkAdapter",
            "Win32_NetworkAdapterConfiguration",
            "Win32_NetworkAdapterSetting",
            "Win32_NetworkClient",
            "Win32_NetworkConnection",
            "Win32_NetworkLoginProfile",
            "Win32_NetworkProtocol",
            "Win32_NTDomain",
            "Win32_NTEventlogFile",
            //"Win32_NTLogEvent",
            //"Win32_NTLogEventComputer",
            //"Win32_NTLogEvnetLog",
            //"Win32_NTLogEventUser",
            "Win32_ODBCAttribute",
            "Win32_ODBCDataSourceAttribute",
            "Win32_ODBCDataSourceSpecification",
            "Win32_ODBCDriverAttribute",
            "Win32_ODBCDriverSoftwareElement",
            "Win32_ODBCDriverSpecification",
            "Win32_ODBCSourceAttribute",
            "Win32_ODBCTranslatorSpecification",
            "Win32_OnBoardDevice",
            "Win32_OperatingSystem",
            "Win32_OperatingSystemAutochkSetting",
            "Win32_OperatingSystemQFE",
            "Win32_OSRecoveryConfiguration",
            "Win32_PageFile",
            "Win32_PageFileElementSetting",
            "Win32_PageFileSetting",
            "Win32_PageFileUsage",
            "Win32_ParallelPort",
            "Win32_Patch",
            "Win32_PatchFile",
            "Win32_PatchPackage",
            "Win32_PCMCIAControler",
            "Win32_PerfFormattedData_ASP_ActiveServerPages",
            "Win32_PerfFormattedData_ASPNET_114322_ASPNETAppsv114322",
            "Win32_PerfFormattedData_ASPNET_114322_ASPNETv114322",
            "Win32_PerfFormattedData_ASPNET_2040607_ASPNETAppsv2040607",
            "Win32_PerfFormattedData_ASPNET_2040607_ASPNETv2040607",
            "Win32_PerfFormattedData_ASPNET_ASPNET",
            "Win32_PerfFormattedData_ASPNET_ASPNETApplications",
            "Win32_PerfFormattedData_aspnet_state_ASPNETStateService",
            "Win32_PerfFormattedData_ContentFilter_IndexingServiceFilter",
            "Win32_PerfFormattedData_ContentIndex_IndexingService",
            "Win32_PerfFormattedData_DTSPipeline_SQLServerDTSPipeline",
            "Win32_PerfFormattedData_Fax_FaxServices",
            "Win32_PerfFormattedData_InetInfo_InternetInformationServicesGlobal",
            "Win32_PerfFormattedData_ISAPISearch_HttpIndexingService",
            "Win32_PerfFormattedData_MSDTC_DistributedTransactionCoordinator",
            "Win32_PerfFormattedData_NETCLRData_NETCLRData",
            "Win32_PerfFormattedData_NETCLRNetworking_NETCLRNetworking",
            "Win32_PerfFormattedData_NETDataProviderforOracle_NETCLRData",
            "Win32_PerfFormattedData_NETDataProviderforSqlServer_NETDataProviderforSqlServer",
            "Win32_PerfFormattedData_NETFramework_NETCLRExceptions",
            "Win32_PerfFormattedData_NETFramework_NETCLRInterop",
            "Win32_PerfFormattedData_NETFramework_NETCLRJit",
            "Win32_PerfFormattedData_NETFramework_NETCLRLoading",
            "Win32_PerfFormattedData_NETFramework_NETCLRLocksAndThreads",
            "Win32_PerfFormattedData_NETFramework_NETCLRMemory",
            "Win32_PerfFormattedData_NETFramework_NETCLRRemoting",
            "Win32_PerfFormattedData_NETFramework_NETCLRSecurity",
            "Win32_PerfFormattedData_NTFSDRV_ControladordealmacenamientoNTFSdeSMTP",
            "Win32_PerfFormattedData_Outlook_Outlook",
            "Win32_PerfFormattedData_PerfDisk_LogicalDisk",
            "Win32_PerfFormattedData_PerfDisk_PhysicalDisk",
            "Win32_PerfFormattedData_PerfNet_Browser",
            "Win32_PerfFormattedData_PerfNet_Redirector",
            "Win32_PerfFormattedData_PerfNet_Server",
            "Win32_PerfFormattedData_PerfNet_ServerWorkQueues",
            "Win32_PerfFormattedData_PerfOS_Cache",
            "Win32_PerfFormattedData_PerfOS_Memory",
            "Win32_PerfFormattedData_PerfOS_Objects",
            "Win32_PerfFormattedData_PerfOS_PagingFile",
            "Win32_PerfFormattedData_PerfOS_Processor",
            "Win32_PerfFormattedData_PerfOS_System",
            "Win32_PerfFormattedData_PerfProc_FullImage_Costly",
            "Win32_PerfFormattedData_PerfProc_Image_Costly",
            "Win32_PerfFormattedData_PerfProc_JobObject",
            "Win32_PerfFormattedData_PerfProc_JobObjectDetails",
            //"Win32_PerfFormattedData_PerfProc_Process",
            //"Win32_PerfFormattedData_PerfProc_ProcessAddressSpace_Costly",
            //"Win32_PerfFormattedData_PerfProc_Thread",
            //"Win32_PerfFormattedData_PerfProc_ThreadDetails_Costly",
            "Win32_PerfFormattedData_RemoteAccess_RASPort",
            "Win32_PerfFormattedData_RemoteAccess_RASTotal",
            "Win32_PerfFormattedData_RSVP_RSVPInterfaces",
            "Win32_PerfFormattedData_RSVP_RSVPService",
            "Win32_PerfFormattedData_Spooler_PrintQueue",
            "Win32_PerfFormattedData_TapiSrv_Telephony",
            "Win32_PerfFormattedData_Tcpip_ICMP",
            "Win32_PerfFormattedData_Tcpip_IP",
            "Win32_PerfFormattedData_Tcpip_NBTConnection",
            "Win32_PerfFormattedData_Tcpip_NetworkInterface",
            "Win32_PerfFormattedData_Tcpip_TCP",
            "Win32_PerfFormattedData_Tcpip_UDP",
            "Win32_PerfFormattedData_TermService_TerminalServices",
            "Win32_PerfFormattedData_TermService_TerminalServicesSession",
            "Win32_PerfFormattedData_W3SVC_WebService",
            "Win32_PerfRawData_ASP_ActiveServerPages",
            "Win32_PerfRawData_ASPNET_114322_ASPNETAppsv114322",
            "Win32_PerfRawData_ASPNET_114322_ASPNETv114322",
            "Win32_PerfRawData_ASPNET_2040607_ASPNETAppsv2040607",
            "Win32_PerfRawData_ASPNET_2040607_ASPNETv2040607",
            "Win32_PerfRawData_ASPNET_ASPNET",
            "Win32_PerfRawData_ASPNET_ASPNETApplications",
            "Win32_PerfRawData_aspnet_state_ASPNETStateService",
            "Win32_PerfRawData_ContentFilter_IndexingServiceFilter",
            "Win32_PerfRawData_ContentIndex_IndexingService",
            "Win32_PerfRawData_DTSPipeline_SQLServerDTSPipeline",
            "Win32_PerfRawData_Fax_FaxServices",
            "Win32_PerfRawData_InetInfo_InternetInformationServicesGlobal",
            "Win32_PerfRawData_ISAPISearch_HttpIndexingService",
            "Win32_PerfRawData_MSDTC_DistributedTransactionCoordinator",
            "Win32_PerfRawData_NETCLRData_NETCLRData",
            "Win32_PerfRawData_NETCLRNetworking_NETCLRNetworking",
            "Win32_PerfRawData_NETDataProviderforOracle_NETCLRData",
            "Win32_PerfRawData_NETDataProviderforSqlServer_NETDataProviderforSqlServer",
            "Win32_PerfRawData_NETFramework_NETCLRExceptions",
            "Win32_PerfRawData_NETFramework_NETCLRInterop",
            "Win32_PerfRawData_NETFramework_NETCLRJit",
            "Win32_PerfRawData_NETFramework_NETCLRLoading",
            "Win32_PerfRawData_NETFramework_NETCLRLocksAndThreads",
            "Win32_PerfRawData_NETFramework_NETCLRMemory",
            "Win32_PerfRawData_NETFramework_NETCLRRemoting",
            "Win32_PerfRawData_NETFramework_NETCLRSecurity",
            "Win32_PerfRawData_NTFSDRV_ControladordealmacenamientoNTFSdeSMTP",
            "Win32_PerfRawData_Outlook_Outlook",
            "Win32_PerfRawData_PerfDisk_LogicalDisk",
            "Win32_PerfRawData_PerfDisk_PhysicalDisk",
            "Win32_PerfRawData_PerfNet_Browser",
            "Win32_PerfRawData_PerfNet_Redirector",
            "Win32_PerfRawData_PerfNet_Server",
            "Win32_PerfRawData_PerfNet_ServerWorkQueues",
            "Win32_PerfRawData_PerfOS_Cache",
            "Win32_PerfRawData_PerfOS_Memory",
            "Win32_PerfRawData_PerfOS_Objects",
            "Win32_PerfRawData_PerfOS_PagingFile",
            "Win32_PerfRawData_PerfOS_Processor",
            "Win32_PerfRawData_PerfOS_System",
            "Win32_PerfRawData_PerfProc_FullImage_Costly",
            "Win32_PerfRawData_PerfProc_Image_Costly",
            "Win32_PerfRawData_PerfProc_JobObject",
            "Win32_PerfRawData_PerfProc_JobObjectDetails",
            //"Win32_PerfRawData_PerfProc_Process",
            //"Win32_PerfRawData_PerfProc_ProcessAddressSpace_Costly",
            //"Win32_PerfRawData_PerfProc_Thread",
            //"Win32_PerfRawData_PerfProc_ThreadDetails_Costly",
            "Win32_PerfRawData_RemoteAccess_RASPort",
            "Win32_PerfRawData_RemoteAccess_RASTotal",
            "Win32_PerfRawData_RSVP_RSVPInterfaces",
            "Win32_PerfRawData_RSVP_RSVPService",
            "Win32_PerfRawData_Spooler_PrintQueue",
            "Win32_PerfRawData_TapiSrv_Telephony",
            "Win32_PerfRawData_Tcpip_ICMP",
            "Win32_PerfRawData_Tcpip_IP",
            "Win32_PerfRawData_Tcpip_NBTConnection",
            "Win32_PerfRawData_Tcpip_NetworkInterface",
            "Win32_PerfRawData_Tcpip_TCP",
            "Win32_PerfRawData_Tcpip_UDP",
            "Win32_PerfRawData_TermService_TerminalServices",
            "Win32_PerfRawData_TermService_TerminalServicesSession",
            "Win32_PerfRawData_W3SVC_WebService",
            "Win32_PhysicalMedia",
            "Win32_PhysicalMemory",
            "Win32_PhysicalMemoryArray",
            "Win32_PhysicalMemoryLocation",
            "Win32_PingStatus",
            "Win32_PNPAllocatedResource",
            "Win32_PnPDevice",
            //"Win32_PnPEntity",
            //"Win32_PnPSignedDriver",
            //"Win32_PnPSignedDriverCIMDataFile",
            "Win32_PointingDevice",
            "Win32_PortableBattery",
            "Win32_PortConnector",
            "Win32_PortResource",
            "Win32_POTSModem",
            "Win32_POTSModemToSerialPort",
            "Win32_Printer",
            "Win32_PrinterConfiguration",
            "Win32_PrinterController",
            "Win32_PrinterDriver",
            "Win32_PrinterDriverDll",
            "Win32_PrinterSetting",
            "Win32_PrinterShare",
            "Win32_PrintJob",
            //"Win32_Process",
            "Win32_Processor",
            //"Win32_Product",
            "Win32_ProductCheck",
            //"Win32_ProductResource",
            //"Win32_ProductSoftwareFeatures",
            "Win32_ProgIDSpecification",
            "Win32_ProgramGroup",
            "Win32_ProgramGroupContents",
            //"Win32_Property",
            "Win32_ProtocolBinding",
            "Win32_Proxy",
            "Win32_PublishComponentAction",
            "Win32_QuickFixEngineering",
            "Win32_QuotaSetting",
            "Win32_Refrigeration",
            "Win32_Registry",
            //"Win32_RegistryAction",
            //"Win32_RemoveFileAction",
            //"Win32_RemoveIniAction",
            "Win32_ReserveCost",
            "Win32_ScheduledJob",
            //"Win32_SCSIController",
            "Win32_SCSIControllerDevice",
            //"Win32_SecuritySettingOfLogicalFile",
            //"Win32_SecuritySettingOfLogicalShare",
            //"Win32_SelfRegModuleAction",
            "Win32_SerialPort",
            "Win32_SerialPortConfiguration",
            "Win32_SerialPortSetting",
            "Win32_ServerConnection",
            "Win32_ServerSession",
            //"Win32_Service",
            //"Win32_ServiceControl",
            //"Win32_ServiceSpecification",
            //"Win32_ServiceSpecificationService",
            "Win32_SessionConnection",
            "Win32_SessionProcess",
            "Win32_Share",
            "Win32_ShareToDirectory",
            //"Win32_ShortcutAction",
            //"Win32_ShortcutFile",
            //"Win32_ShortcutSAP",
            "Win32_SID",
            //"Win32_SoftwareElement",
            //"Win32_SoftwareElementAction",
            //"Win32_SoftwareElementCheck",
            //"Win32_SoftwareElementCondition",
            //"Win32_SoftwareElementResource",
            //"Win32_SoftwareFeature",
            //"Win32_SoftwareFeatureAction",
            //"Win32_SoftwareFeatureCheck",
            //"Win32_SoftwareFeatureParent",
            //"Win32_SoftwareFeatureSoftwareElements",
            "Win32_SoundDevice",
            "Win32_StartupCommand",
            //"Win32_SubDirectory",
            "Win32_SystemAccount",
            "Win32_SystemBIOS",
            "Win32_SystemBootConfiguration",
            "Win32_SystemDesktop",
            "Win32_SystemDevices",
            //"Win32_SystemDriver",
            //"Win32_SystemDriverPNPEntity",
            "Win32_SystemEnclosure",
            "Win32_SystemLoadOrderGroups",
            "Win32_SystemLogicalMemoryConfiguration",
            "Win32_SystemNetworkConnections",
            "Win32_SystemOperatingSystem",
            "Win32_SystemPartitions",
            "Win32_SystemProcesses",
            "Win32_SystemProgramGroups",
            "Win32_SystemResources",
            "Win32_SystemServices",
            "Win32_SystemSlot",
            "Win32_SystemSystemDriver",
            "Win32_SystemTimeZone",
            "Win32_SystemUsers",
            "Win32_TapeDrive",
            "Win32_TCPIPPrinterPort",
            "Win32_TemperatureProbe",
            "Win32_Terminal",
            "Win32_TerminalService",
            "Win32_TerminalServiceSetting",
            "Win32_TerminalServiceToSetting",
            "Win32_TerminalTerminalSetting",
            //"Win32_Thread",
            "Win32_TimeZone",
            "Win32_TSAccount",
            "Win32_TSClientSetting",
            "Win32_TSEnvironmentSetting",
            "Win32_TSGeneralSetting",
            "Win32_TSLogonSetting",
            "Win32_TSNetworkAdapterListSetting",
            "Win32_TSNetworkAdapterSetting",
            "Win32_TSPermissionsSetting",
            "Win32_TSRemoteControlSetting",
            "Win32_TSSessionDirectory",
            "Win32_TSSessionDirectorySetting",
            "Win32_TSSessionSetting",
            //"Win32_TypeLibraryAction",
            "Win32_UninterruptiblePowerSupply",
            "Win32_USBController",
            "Win32_USBControllerDevice",
            "Win32_USBHub",
            "Win32_UserAccount",
            "Win32_UserDesktop",
            "Win32_UserInDomain",
            "Win32_UTCTime",
            "Win32_VideoController",
            "Win32_VideoSettings",
            "Win32_VoltageProbe",
            "Win32_VolumeQuotaSetting",
            "Win32_WindowsProductActivation",
            "Win32_WMIElementSetting",
            "Win32_WMISetting"
        };


    }



    class CacheProcessor
    {
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        //[DllImport("kernel32.dll")]
        //public static extern int GetCurrentProcessorNumber();

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct GROUP_AFFINITY
        {
            public UIntPtr Mask;

            [MarshalAs(UnmanagedType.U2)]
            public ushort Group;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U2)]
            public ushort[] Reserved;
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern Boolean SetThreadGroupAffinity(IntPtr hThread, ref GROUP_AFFINITY GroupAffinity, ref GROUP_AFFINITY PreviousGroupAffinity);

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESSORCORE
        {
            public byte Flags;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NUMANODE
        {
            public uint NodeNumber;
        }

        public enum PROCESSOR_CACHE_TYPE
        {
            CacheUnified,
            CacheInstruction,
            CacheData,
            CacheTrace
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CACHE_DESCRIPTOR
        {
            public byte Level;
            public byte Associativity;
            public ushort LineSize;
            public uint Size;
            public PROCESSOR_CACHE_TYPE Type;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SYSTEM_LOGICAL_PROCESSOR_INFORMATION_UNION
        {
            [FieldOffset(0)]
            public PROCESSORCORE ProcessorCore;
            [FieldOffset(0)]
            public NUMANODE NumaNode;
            [FieldOffset(0)]
            public CACHE_DESCRIPTOR Cache;
            [FieldOffset(0)]
            private UInt64 Reserved1;
            [FieldOffset(8)]
            private UInt64 Reserved2;
        }

        public enum LOGICAL_PROCESSOR_RELATIONSHIP
        {
            RelationProcessorCore,
            RelationNumaNode,
            RelationCache,
            RelationProcessorPackage,
            RelationGroup,
            RelationAll = 0xffff
        }

        public struct SYSTEM_LOGICAL_PROCESSOR_INFORMATION
        {
#pragma warning disable 0649
            public UIntPtr ProcessorMask;
            public LOGICAL_PROCESSOR_RELATIONSHIP Relationship;
            public SYSTEM_LOGICAL_PROCESSOR_INFORMATION_UNION ProcessorInformation;
#pragma warning restore 0649
        }

        [DllImport(@"kernel32.dll", SetLastError = true)]
        public static extern bool GetLogicalProcessorInformation(IntPtr Buffer, ref uint ReturnLength);

        private const int ERROR_INSUFFICIENT_BUFFER = 122;

        private static SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] _logicalProcessorInformation = null;

        public static SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] LogicalProcessorInformation
        {
            get
            {
                if (_logicalProcessorInformation != null)
                    return _logicalProcessorInformation;

                uint ReturnLength = 0;

                GetLogicalProcessorInformation(IntPtr.Zero, ref ReturnLength);

                if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
                {
                    IntPtr Ptr = Marshal.AllocHGlobal((int)ReturnLength);
                    try
                    {
                        if (GetLogicalProcessorInformation(Ptr, ref ReturnLength))
                        {
                            int size = Marshal.SizeOf(typeof(SYSTEM_LOGICAL_PROCESSOR_INFORMATION));
                            int len = (int)ReturnLength / size;
                            _logicalProcessorInformation = new SYSTEM_LOGICAL_PROCESSOR_INFORMATION[len];
                            IntPtr Item = Ptr;

                            for (int i = 0; i < len; i++)
                            {
                                _logicalProcessorInformation[i] = (SYSTEM_LOGICAL_PROCESSOR_INFORMATION)Marshal.PtrToStructure(Item, typeof(SYSTEM_LOGICAL_PROCESSOR_INFORMATION));
                                Item += size;
                            }

                            return _logicalProcessorInformation;
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(Ptr);
                    }
                }
                return null;
            }
        }

        public static List<int> GetPerCoreCacheSizes()
        {
            List<int> result = new List<int>() { 0, 0, 0 };
            Int64 L1 = 0;
            Int64 L2 = 0;
            Int64 L3 = 0;

            var info = CacheProcessor.LogicalProcessorInformation;
            foreach (var entry in info)
            {
                if (entry.Relationship != CacheProcessor.LOGICAL_PROCESSOR_RELATIONSHIP.RelationCache)
                    continue;
                Int64 mask = (Int64)entry.ProcessorMask;
                if ((mask & (Int64)1) == 0)
                    continue;
                var cache = entry.ProcessorInformation.Cache;
                switch (cache.Level)
                {
                    case 1: L1 = L1 + cache.Size; break;
                    case 2: L2 = L2 + cache.Size; break;
                    case 3: L3 = L3 + cache.Size; break;
                    default:
                        break;
                }
            }
            result[0] = (int)L1;
            result[1] = (int)L2;
            result[2] = (int)L3;
            return result;
        }
    }

}
