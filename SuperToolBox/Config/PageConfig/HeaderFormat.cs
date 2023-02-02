using SuperUtils.Framework.ORM.Config;
using System.Windows;

namespace SuperToolBox.Config.PageConfig
{

    public class HeaderFormat : AbstractConfig
    {
        private HeaderFormat() : base(ConfigManager.SQLITE_DATA_PATH, $"PageConfig.HeaderFormat")
        {
            OriginText = "";
            TargetText = "";
        }

        private static HeaderFormat _instance = null;

        public static HeaderFormat CreateInstance()
        {
            if (_instance == null) _instance = new HeaderFormat();
            return _instance;
        }
        public bool AutoWrap { get; set; }
        public bool AutoWrapTarget { get; set; }
        public string OriginText { get; set; }
        public string TargetText { get; set; }

    }
}
