using DynamicData.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SuperToolBox.Entity
{
    public class BaseTool : INotifyPropertyChanged
    {
        private long _ToolID;
        public long ToolID
        {
            get { return _ToolID; }
            set { _ToolID = value; OnPropertyChanged(); }
        }
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }

        private string _UIPageName;
        public string UIPageName
        {
            get { return _UIPageName; }
            set { _UIPageName = value; OnPropertyChanged(); }
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
