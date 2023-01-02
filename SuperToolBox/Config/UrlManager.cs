using SuperUtils.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuperToolBox.Config
{
    public static class UrlManager
    {

        public static readonly string ReleaseUrl = "https://github.com/SuperStudio/SuperToolBox/releases";
        public static readonly string UpgradeSource = "https://superstudio.github.io";
        public static readonly string UpdateUrl = "https://superstudio.github.io/SuperToolBox-Upgrade/latest.json";
        public const string UpdateFileListUrl = "https://superstudio.github.io/SuperToolBox-Upgrade/list.json";
        public static string UpdateFilePathUrl = "https://superstudio.github.io/SuperToolBox-Upgrade/File/";
        private static string DonateJsonUrl = "https://superstudio.github.io/SuperSudio-Donate/config.json";
        public static string PluginUrl = "https://superstudio.github.io/SuperPlugins/";

        public static string GetDonateJsonUrl()
        {
            return DonateJsonUrl;
        }

        static UrlManager()
        {
            Type t = typeof(UrlManager);
            FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo fi in fields)
            {
                string name = fi.Name;
                string value = FileHelper.TryReadConfigFromJson(name);
                if (!string.IsNullOrEmpty(value))
                {
                    fi.SetValue(null, value);
                }
            }
        }
    }
}
