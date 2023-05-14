using SuperUtils.Framework.ORM.Config;
using System.Windows;

namespace SuperToolBox.Config.PageConfig
{

    public class CharTransformConfig : AbstractConfig
    {
        private CharTransformConfig() : base(ConfigManager.SQLITE_DATA_PATH, $"PageConfig.CharTransformConfig")
        {
            TypeIndex = 2;
        }

        private static CharTransformConfig _instance = null;

        public static CharTransformConfig CreateInstance()
        {
            if (_instance == null) _instance = new CharTransformConfig();
            return _instance;
        }
        public long TypeIndex { get; set; }
        public string OriginText { get; set; }

    }
}
