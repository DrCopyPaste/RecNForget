using System.IO;
using System.Linq;
using System.Text;
using RecNForget.Help;

namespace RecNForget.GithubHelpGenerator
{
    /// <summary>
    /// this program creates a folder structure under the root of this repository that looks like this
    /// /Help/features.md (table of contents for all features)
    /// /Help/Features/<FeatureClassName>.md
    /// 
    /// /Help/Features/ includes all features found under the RecNForget.Help.Features namespace
    /// quickstart "behaves" like a feature, but is added seperately
    /// </summary>
    public class Program
    {
        private static string baseUrl = "https://github.com/DrCopyPaste/RecNForget/blob/master/Help/Features/";
        private static string tocPath = @"..\..\..\..\Help";
        private static string featuresPath = @"..\..\..\..\Help\Features";

        public static void Main(string[] args)
        {
            DirectoryInfo tocFolder = new DirectoryInfo(tocPath);
            EnsureEmptyFolder(tocFolder);

            DirectoryInfo featuresFolder = new DirectoryInfo(featuresPath);
            EnsureEmptyFolder(featuresFolder);

            var quickStart = new Help.General.QuickStart();

            // only cover actual features here (not bugfixes or feature changes or verbose information)
            // ToDo filter out obsolete features like in for NewToVersionDialog (MinVersion/MaxVersion)
            var allFeatures = HelpFeature.All.Where(f => f.FeatureClass == HelpFeatureClass.NewFeature);

            // create a master help page that contains a table of contents with links to all features
            // create an md file for each feature
            StringBuilder tocPageContents = new StringBuilder();
            tocPageContents.AppendLine(HeadingLine("RecNForget - Help Topics"));

            tocPageContents.AppendLine(FeatureHyperLinkLine(quickStart.Title, quickStart.Id));
            File.WriteAllText(Path.Combine(featuresPath, string.Format("{0}.md", quickStart.Id)), quickStart.HelpLinesAsString());

            foreach (var feature in allFeatures)
            {
                tocPageContents.AppendLine(FeatureHyperLinkLine(feature.Title, feature.Id));
                File.WriteAllText(Path.Combine(featuresPath, string.Format("{0}.md", feature.Id)), feature.HelpLinesAsString());
            }

            File.WriteAllText(Path.Combine(tocPath, "toc.md"), tocPageContents.ToString());
        }

        private static string HeadingLine(string contents)
        {
            return string.Format("# {0}", contents);
        }

        private static string FeatureHyperLinkLine(string linkText, string linkedFeature)
        {
            return string.Format("- [{0}]({1}{2}.md)", linkText, baseUrl, linkedFeature);
        }

        private static void EnsureEmptyFolder(DirectoryInfo folder)
        {
            if (folder == null)
            {
                return;
            }

            if (folder.Exists)
            {
                folder.Delete(true);
            }

            if (!folder.Exists)
            {
                folder.Create();
            }
        }
    }
}
