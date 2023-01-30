using SuperUtils.WPF.VieModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SuperToolBox.Entity
{
    public class BaseTool : ViewModelBase
    {
        private long _ToolID;
        public long ToolID
        {
            get { return _ToolID; }
            set { _ToolID = value; RaisePropertyChanged(); }
        }
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; RaisePropertyChanged(); }
        }

        private string _UIPageName;
        public string UIPageName
        {
            get { return _UIPageName; }
            set { _UIPageName = value; RaisePropertyChanged(); }
        }




        private int _MaxOpenCount;
        /// <summary>
        /// 最大打开数量，0表示无上限
        /// </summary>
        public int MaxOpenCount
        {
            get { return _MaxOpenCount; }
            set { _MaxOpenCount = value; RaisePropertyChanged(); }
        }


        public BaseTool()
        {

        }
        public BaseTool(long ToolID, string Name, string UIPageName, int maxOpenCount)
        {
            this.ToolID = ToolID;
            this.Name = Name;
            this.UIPageName = UIPageName;
            this.MaxOpenCount = maxOpenCount;
        }
    }
}
