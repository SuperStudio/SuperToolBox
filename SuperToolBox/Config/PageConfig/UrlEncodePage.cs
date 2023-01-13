using SuperUtils.Framework.ORM.Config;
using System.Windows;

namespace SuperToolBox.Config.PageConfig
{

    public class UrlEncodePage : AbstractConfig
    {
        private UrlEncodePage() : base(ConfigManager.SQLITE_DATA_PATH, $"PageConfig.UrlEncodePage")
        {
            OriginText = "";
            TargetText = "";
        }

        private static UrlEncodePage _instance = null;

        public static UrlEncodePage CreateInstance()
        {
            if (_instance == null) _instance = new UrlEncodePage();
            return _instance;
        }
        public bool AutoWrap { get; set; }
        public string OriginText { get; set; }
        public string TargetText { get; set; }

    }
}
