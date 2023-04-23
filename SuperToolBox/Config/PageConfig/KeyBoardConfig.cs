using SuperUtils.Framework.ORM.Config;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SuperToolBox.Config.PageConfig
{

    public class KeyBoardConfig : AbstractConfig, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private KeyBoardConfig() : base(ConfigManager.SQLITE_DATA_PATH, $"PageConfig.KeyBoardConfig")
        {

        }

        private static KeyBoardConfig _instance = null;

        public static KeyBoardConfig CreateInstance()
        {
            if (_instance == null) _instance = new KeyBoardConfig();
            return _instance;
        }
        private bool _ShowKeyView = true;
        public bool ShowKeyView
        {
            get { return _ShowKeyView; }
            set { _ShowKeyView = value; RaisePropertyChanged(); }
        }

    }
}
