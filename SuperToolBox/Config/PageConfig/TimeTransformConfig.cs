using SuperUtils.Framework.ORM.Config;
using System.Windows;

namespace SuperToolBox.Config.PageConfig
{

    public class TimeTransformConfig : AbstractConfig
    {
        private TimeTransformConfig() : base(ConfigManager.SQLITE_DATA_PATH, $"PageConfig.TimeTransformConfig")
        {

        }

        private static TimeTransformConfig _instance = null;

        public static TimeTransformConfig CreateInstance()
        {
            if (_instance == null) _instance = new TimeTransformConfig();
            return _instance;
        }

        public string TimeStampText { get; set; }
        public string LocalTimeText { get; set; }

    }
}
