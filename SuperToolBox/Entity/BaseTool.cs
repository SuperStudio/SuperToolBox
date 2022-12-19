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

        public BaseTool()
        {

        }
        public BaseTool(long ToolID, string Name, string UIPageName)
        {
            this.ToolID = ToolID;
            this.Name = Name;
            this.UIPageName = UIPageName;
        }
    }
}
