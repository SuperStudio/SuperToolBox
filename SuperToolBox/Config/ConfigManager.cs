using SuperToolBox.Config.PageConfig;
using SuperToolBox.Config.WindowConfig;
using SuperUtils.Framework.ORM.Config;
using System;

namespace SuperToolBox.Config
{
    public static class ConfigManager
    {
        public const string SQLITE_DATA_PATH = "user_data.sqlite";
        public const string RELEASE_DATE = "2023-03-26";


        public static Main Main { get; set; }
        public static Settings Settings { get; set; }
        public static UrlEncodePage UrlEncodePage { get; set; }
        public static HeaderFormat HeaderFormat { get; set; }


        static ConfigManager()
        {
            Main = Main.CreateInstance();
            Settings = Settings.CreateInstance();
            UrlEncodePage = UrlEncodePage.CreateInstance();
            HeaderFormat = HeaderFormat.CreateInstance();
        }

        public static void InitConfig()
        {
            System.Reflection.PropertyInfo[] propertyInfos = typeof(ConfigManager).GetProperties();
            foreach (var item in propertyInfos)
            {
                AbstractConfig config = item.GetValue(null) as AbstractConfig;
                if (config == null)
                    throw new Exception("无法识别的 AbstractConfig");
                config.Read();
            }
        }

    }
}
