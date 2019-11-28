using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;
using RecNForget.Services;

namespace RecNForget.Setup.Extensions
{
    public class CustomActions
    {
        // yes there are custom actions that could do this task directly WITHIN wix like RemoveRegistryKey
        // BUT this way there is a good template to augment the (un)installation sequence in general with whatever
        // AND also this way we can move the magic strings that describe, what is to be deleted into a shared segment
        // and dont have to duplicate so much in setup project and the application itself
        private static void RemoveAutoStartWithWindowsFromRegistry()
        {
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            var recNForgetAutoStartRegistry = regKey.GetValue("RecNForget");

            if (recNForgetAutoStartRegistry != null)
            {
                regKey.DeleteValue("RecNForget");
            }
        }

        // yes there are custom actions that could do this task directly WITHIN wix like RemoveFiles
        // BUT see reasoning above...
        private static void RemoveApplicationConfigurationFiles()
        {
            AppSettingHelper.RemoveAppConfigSettingFile();
        }

        [CustomAction]
        public static ActionResult RemoveApplicationSettings(Session session)
        {
            RemoveAutoStartWithWindowsFromRegistry();
            RemoveApplicationConfigurationFiles();

            return ActionResult.Success;
        }
    }
}