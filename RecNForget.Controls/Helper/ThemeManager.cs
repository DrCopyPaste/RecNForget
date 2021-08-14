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
        public static Dictionary<string, string> GetAllThemeNames()
        {
            var asm = Assembly.GetExecutingAssembly();
            Stream stream = asm.GetManifestResourceStream(asm.GetName().Name + ".g.resources");

            var pages = new List<Uri>();
            var themeNames = new Dictionary<string, string>();

            using (var reader = new System.Resources.ResourceReader(stream))
            {
                foreach (System.Collections.DictionaryEntry entry in reader)
                {
                    var keyName = entry.Key as string;

                    if (keyName.StartsWith("themes") && keyName.EndsWith("baml"))
                    {
                        var fileName = keyName.Replace(".baml", "").Split('/')[1];
                        var displayName = fileName.ToLower().Replace("_", " ");

                        themeNames.Add(fileName, displayName);
                    }
                }
            }

            return themeNames;
        }

        public static void ChangeTheme(string themeFileName)
        {
            Uri dictUri = new Uri("/RecNForget.Controls;component/Themes/" + themeFileName + ".xaml", UriKind.RelativeOrAbsolute);
            ResourceDictionary resourceDict = Application.LoadComponent(dictUri) as ResourceDictionary;

            try
            {
                Application.Current.Resources = resourceDict;
            }
            catch { }
        }
    }
}
