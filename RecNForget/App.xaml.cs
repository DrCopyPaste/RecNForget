using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        [STAThread]
        public static void Main()
        {
            using (var appLock = new SingleInstanceApplicationLock())
            {
                if (!appLock.TryAcquireExclusiveLock())
                {
                    TaskbarIcon taskbarIcon = new TaskbarIcon();
                    taskbarIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

                    // show a balloon tip indicating that RecNForget is already running
                    taskbarIcon.ShowBalloonTip("Recording is already running.", "Another instance of RecNForget is already running, closing this one...", taskbarIcon.Icon, true);
                    Thread.Sleep(1000);

                    return;
                }

                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
        }
    }
}
