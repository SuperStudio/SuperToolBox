
using SuperToolBox.Config;
using SuperToolBox.Entity;
using SuperUtils.Common;
using SuperUtils.Framework.ORM.Mapper;
using SuperUtils.WPF.VieModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;

namespace SuperToolBox.ViewModel
{
    public class VieModel_Main : ViewModelBase
    {
        public static List<BaseTool> TOOLS = new List<BaseTool>()
        {
            {new BaseTool(1,"URL编码/解码","UrlEncodeDecode") },
            {new BaseTool(2,"加密","Encrypt") },
            {new BaseTool(3,"鼠标控制","MouseControl") },
            {new BaseTool(4,"设备信息","DeviceInfo") },
            //{new BaseTool(5,"网络监控","NetWorkMonitor") },
        };

        public VieModel_Main()
        {
            Init();
        }

        public void Init()
        {
            ToolList = new List<BaseTool>();
            foreach (var item in TOOLS)
            {
                BaseTool baseTool = new BaseTool();
                baseTool.Name = item.Name;
                baseTool.ToolID = item.ToolID;
                baseTool.UIPageName = $"pack://application:,,,/ToolPages/{ item.UIPageName}.xaml";
                ToolList.Add(baseTool);
            }

            LoadToolList();
        }

        public void LoadToolList(string search = "")
        {
            CurrentToolList = new ObservableCollection<BaseTool>();
            // 筛选
            List<BaseTool> toolList = ToolList.Where(arg => arg.Name.ToLower().IndexOf(search.ToLower()) >= 0).ToList();
            foreach (var item in toolList)
            {
                CurrentToolList.Add(item);
            }
        }

        public List<BaseTool> ToolList { get; set; }

        private ObservableCollection<BaseTool> _CurrentToolList;
        public ObservableCollection<BaseTool> CurrentToolList
        {
            get { return _CurrentToolList; }
            set { _CurrentToolList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<BaseTool> _ToolTabs;
        public ObservableCollection<BaseTool> ToolTabs
        {
            get { return _ToolTabs; }
            set { _ToolTabs = value; RaisePropertyChanged(); }
        }


        public void OpenTool(long toolID)
        {
            if (ToolTabs == null) ToolTabs = new ObservableCollection<BaseTool>();
            BaseTool baseTool = CurrentToolList.Where(arg => arg.ToolID == toolID).FirstOrDefault();
            if (baseTool != null && !ToolTabs.Contains(baseTool))
                ToolTabs.Add(baseTool);
        }


    }

}
