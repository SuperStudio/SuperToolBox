﻿using GalaSoft.MvvmLight.Command;
using SuperControls.Style.Upgrade;
using SuperToolBox.Config;
using SuperUtils.NetWork.Crawler;
using System.Windows;

namespace SuperToolBox.Commands
{
    public class OpenWindow
    {
        public static RelayCommand<Window> Upgrade { get; set; }


        static OpenWindow()
        {
            Upgrade = new RelayCommand<Window>(parent =>
            {
                SuperUpgrader upgrader = new SuperUpgrader();
                upgrader.InfoUrl = UrlManager.UpdateUrl;
                upgrader.FileListUrl = UrlManager.UpdateFileListUrl;
                upgrader.FilePathUrl = UrlManager.UpdateFilePathUrl;
                upgrader.ReleaseUrl = UrlManager.ReleaseUrl;
                upgrader.UpgradeSource = UrlManager.UpgradeSource;
                upgrader.Language = "zh-CN";
                upgrader.Header = new CrawlerHeader(null).Default;
                upgrader.Logger = null;//todo
                upgrader.BeforeUpdateDelay = 5;
                upgrader.AfterUpdateDelay = 1;
                upgrader.UpDateFileDir = "TEMP";
                upgrader.AppName = "SuperToolBox.exe";
                Dialog_Upgrade dialog_Upgrade = new Dialog_Upgrade(parent, upgrader);
                dialog_Upgrade.LocalVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                dialog_Upgrade.ShowDialog();
            });
        }
    }
}
