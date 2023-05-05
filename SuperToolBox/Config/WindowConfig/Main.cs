using SuperUtils.Framework.ORM.Config;
using System.Windows;

namespace SuperToolBox.Config.WindowConfig
{
    public class Main : AbstractConfig
    {
        private Main() : base(ConfigManager.SQLITE_DATA_PATH, $"WindowConfig.Main")
        {
            Width = SystemParameters.WorkArea.Width * 0.8;
            Height = SystemParameters.WorkArea.Height * 0.8;
            FirstRun = true;
            BeforeOpenedTools = "";
        }

        private static Main _instance = null;

        public static Main CreateInstance()
        {
            if (_instance == null) _instance = new Main();

            return _instance;
        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public long WindowState { get; set; }


        public bool FirstRun { get; set; }

        public string BeforeOpenedTools { get; set; }
        public string LatestNotice { get; set; }
        public long AsciiSelectedIndex { get; set; }

    }
}
