using SuperToolBox.Entity;
using SuperUtils.WPF.VieModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SuperToolBox.ViewModel
{
    public class VieModel_Main : ViewModelBase
    {
        public static List<BaseTool> TOOLS = new List<BaseTool>()
        {
            {new BaseTool(1,"URL编码/解码","UrlEncodeDecode",1 )},
            {new BaseTool(2,"加密","Encrypt" ,1)},
            {new BaseTool(3,"鼠标控制","MouseControl",1) },
            {new BaseTool(4,"设备信息","DeviceInfo",1 )},
            {new BaseTool(5,"Header格式化","HeaderFormat",1 )},
            {new BaseTool(6,"键盘控制","KeyBoardControl",1 )},
            {new BaseTool(7,"时间戳","TimeTransform",1 )},
            {new BaseTool(8,"进制转换","CharTransform",1 )},
            //{new BaseTool(6,"右键菜单","RightMenu",1 )},
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
                baseTool.MaxOpenCount = item.MaxOpenCount;
                baseTool.UIPageName = $"pack://application:,,,/ToolPages/{item.UIPageName}.xaml";
                ToolList.Add(baseTool);
            }

            LoadToolList();
            ToolTabs.CollectionChanged += (s, e) =>
            {
                if (ToolTabs != null && ToolTabs.Count > 0)
                    ShowSoft = false;
                else
                    ShowSoft = true;
            };
        }

        public void LoadToolList(string search = "")
        {
            ToolTabs = new ObservableCollection<BaseTool>();
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

        private bool _ShowSoft = true;
        public bool ShowSoft
        {
            get { return _ShowSoft; }
            set { _ShowSoft = value; RaisePropertyChanged(); }
        }


        public void OpenTool(long toolID)
        {
            if (ToolTabs == null) ToolTabs = new ObservableCollection<BaseTool>();
            BaseTool baseTool = CurrentToolList.Where(arg => arg.ToolID == toolID).FirstOrDefault();
            if (baseTool != null)
            {
                BaseTool existTool = ToolTabs.FirstOrDefault(arg => arg.ToolID == toolID);
                if (existTool == null)
                    ToolTabs.Add(baseTool);
                else
                {
                    int count = ToolTabs.Count(arg => arg.ToolID == toolID);
                    if (baseTool.MaxOpenCount == 0)
                        ToolTabs.Add(baseTool);
                    else if (count < baseTool.MaxOpenCount)
                        ToolTabs.Add(baseTool);
                }
            }

        }


    }

}
