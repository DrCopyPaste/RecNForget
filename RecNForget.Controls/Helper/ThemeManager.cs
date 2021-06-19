using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows;

namespace RecNForget.Controls.Helper
{
    public class ThemeManager
    {
        public static List<string> GetAllThemeNames()
        {
            var asm = Assembly.GetExecutingAssembly();
            Stream stream = asm.GetManifestResourceStream(asm.GetName().Name + ".g.resources");

            var pages = new List<Uri>();
            var themeNames = new List<string>();

            using (var reader = new System.Resources.ResourceReader(stream))
            {
                foreach (System.Collections.DictionaryEntry entry in reader)
                {
                    var keyName = entry.Key as string;

                    if (keyName.StartsWith("themes") && keyName.EndsWith("baml"))
                    {
                        themeNames.Add(keyName.Replace(".baml", "").Split('/')[1]);
                    }
                }
            }

            return themeNames;
        }

        public static void ChangeTheme(string themeName)
        {
            Uri dictUri = new Uri("/RecNForget.Controls;component/Themes/" + themeName + ".xaml", UriKind.RelativeOrAbsolute);
            ResourceDictionary resourceDict = Application.LoadComponent(dictUri) as ResourceDictionary;

            try
            {
                Application.Current.Resources = resourceDict;
            }
            catch { }
        }
    }
}
