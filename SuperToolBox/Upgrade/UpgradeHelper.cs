﻿
using SuperToolBox.Config;
using SuperControls.Style.Upgrade;
using SuperUtils.NetWork;
using SuperUtils.NetWork.Crawler;
using SuperUtils.WPF.VieModel;
using SuperUtils.WPF.VisualTools;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace SuperToolBox.Upgrade
{
    public static class UpgradeHelper
    {
        public static Action OnBeforeCopyFile { get; set; }
        public static int AUTO_CHECK_UPGRADE_DELAY = 60 * 1000;
        public static void Init(Window parent)
        {
            Upgrader = new SuperUpgrader();
            Upgrader.UpgradeSourceDict = UrlManager.UpgradeSourceDict;
            Upgrader.UpgradeSourceIndex = UrlManager.GetRemoteIndex();
            Upgrader.Language = "zh-CN";
            Upgrader.Header = new CrawlerHeader(SuperWebProxy.SystemWebProxy).Default;
            Upgrader.Logger = null;//todo
            Upgrader.BeforeUpdateDelay = 5;
            Upgrader.AfterUpdateDelay = 1;
            Upgrader.UpDateFileDir = "TEMP";
            Upgrader.AppName = "SuperToolBox.exe";
            Window = parent;
            CreateDialog_Upgrade();
        }

        public static void CreateDialog_Upgrade()
        {
            dialog_Upgrade = new Dialog_Upgrade(Upgrader);
            dialog_Upgrade.LocalVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            dialog_Upgrade.OnSourceChanged += (s, e) =>
            {
                // 保存当前选择的地址
                int index = e.NewValue;
                UrlManager.SetRemoteIndex(index);
                ConfigManager.Settings.RemoteIndex = index;
                ConfigManager.Settings.Save();
            };
            dialog_Upgrade.Closed += (s, e) =>
            {
                WindowClosed = true;
            };
            dialog_Upgrade.OnExitApp += () =>
            {
                OnBeforeCopyFile?.Invoke();
            };
            WindowClosed = false;
        }

        private static bool WindowClosed { get; set; }
        private static Window Window { get; set; }

        public static void OpenWindow(Window window)
        {
            if (WindowClosed)
                CreateDialog_Upgrade();
            dialog_Upgrade?.ShowDialog(window);

        }

        public static async Task<(string LatestVersion, string ReleaseDate, string ReleaseNote)> GetUpgradeInfo()
        {
            if (Upgrader == null) return (null, null, null);
            return await Upgrader.GetUpgradeInfo();
        }

        private static SuperUpgrader Upgrader { get; set; }
        private static Dialog_Upgrade dialog_Upgrade { get; set; }

    }
}
