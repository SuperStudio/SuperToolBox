using SuperControls.Style.Windows;
using SuperToolBox.Config;
using SuperUtils.Framework.Logger;
using SuperUtils.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SuperToolBox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Log Logger = Log.Instance;
        static App()
        {
            Window_ErrorMsg.OnFeedBack += () =>
            {
                FileHelper.TryOpenUrl(UrlManager.FeedbackUrl);
            };
            Window_ErrorMsg.OnLog += (str) =>
            {
                Logger.Error(str);
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG

#else
            // UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Window_ErrorMsg.App_DispatcherUnhandledException);

            // Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += Window_ErrorMsg.TaskScheduler_UnobservedTaskException;

            // 非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Window_ErrorMsg.CurrentDomain_UnhandledException);
#endif
            base.OnStartup(e);
        }
    }

}
