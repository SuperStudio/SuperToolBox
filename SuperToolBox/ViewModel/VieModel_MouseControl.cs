using SuperToolBox.Config;
using SuperToolBox.Entity;
using SuperUtils.Common;
using SuperUtils.Framework.ORM.Mapper;
using SuperUtils.WPF.VieModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using static SuperUtils.Framework.Hooks.HookManager;

namespace SuperToolBox.ViewModel
{
    public class VieModel_MouseControl : ViewModelBase
    {

        private static SqliteMapper<MouseCommand> commandMapper { get; set; }

        public VieModel_MouseControl()
        {
            Init();
        }

        static VieModel_MouseControl()
        {
            commandMapper = new SqliteMapper<MouseCommand>(ConfigManager.SQLITE_DATA_PATH);
        }

        public override void Init()
        {
            MouseCommands = new ObservableCollection<MouseCommand>();
            List<MouseCommand> mouseCommands = commandMapper.SelectList();
            foreach (var item in mouseCommands)
            {
                MouseCommands.Add(item);
            }

        }


        public void AddCommand()
        {
            if (MouseCommands == null) MouseCommands = new ObservableCollection<MouseCommand>();
            MouseCommand mouseCommand = new MouseCommand();
            mouseCommand.Delay = MouseCommand.DEFAULT_DELAY;
            mouseCommand.CommandOrder = MouseCommands.Count + 1;
            mouseCommand.MouseButton = MouseButton.Left;
            mouseCommand.MouseAction = MouseAction.Click;
            MouseCommands.Add(mouseCommand);
            commandMapper.Insert(mouseCommand);
        }

        public void DeleteByID(long id)
        {
            commandMapper.DeleteById(id);
            int idx = -1;
            for (int i = 0; i < MouseCommands.Count; i++)
            {
                if (MouseCommands[i].ID == id)
                {
                    idx = i;
                    break;
                }
            }
            if (idx >= 0 && idx < MouseCommands.Count)
                MouseCommands.RemoveAt(idx);
        }

        public void UpdateByID(long id)
        {
            MouseCommand mouseCommand = MouseCommands.Where(arg => arg.ID == id).FirstOrDefault();
            if (mouseCommand != null)
                commandMapper.Update(mouseCommand);
        }

        private ObservableCollection<MouseCommand> _MouseCommands;
        public ObservableCollection<MouseCommand> MouseCommands
        {
            get { return _MouseCommands; }
            set { _MouseCommands = value; RaisePropertyChanged(); }
        }
        private bool _RunningCommands;
        public bool RunningCommands
        {
            get { return _RunningCommands; }
            set { _RunningCommands = value; RaisePropertyChanged(); }
        }
    }

}
