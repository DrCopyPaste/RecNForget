using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecNForget.CopyRightHelper
{
    /// <summary>
    /// USAGE:
    /// execute this program to override copyright info for the about section from RecNForget client
    /// also updates COPYRIGHT.md (whose contents should also be used (i.e. manually copied (and/ or referenced?) - there are no md includes ;/) in README.md)
    /// </summary>
    // from onlinewebfonts.com:
    // You must credit the author Copy this link on your web
    // <div>Icon made from <a href="http://www.onlinewebfonts.com/icon">Icon Fonts</a> is licensed by CC BY 3.0</div>
    class Program
    {
        private static string iconBasePath = @"..\..\..\icon_generation";
        private static string copyRightFilePath = @"..\..\..\COPYRIGHT.md";

        static void Main(string[] args)
        {
            var mdBaseInfoTemplateBuilder = new StringBuilder();
            mdBaseInfoTemplateBuilder.AppendLine("The picture {0} was generated from " + MdFileLinkString("onlinewebfonts.com/icon/{1}", "https://www.onlinewebfonts.com/icon/{1}"));
            mdBaseInfoTemplateBuilder.AppendLine();
            mdBaseInfoTemplateBuilder.AppendLine(string.Format(@"Icon made from {0} is licensed by CC BY 3.0", MdFileLinkString("Icon Fonts", "http://www.onlinewebfonts.com/icon")));
            mdBaseInfoTemplateBuilder.AppendLine();

            var textBlockInfoTemplateBuilder = new StringBuilder();
            textBlockInfoTemplateBuilder.Append(XamText("The picture {0} was generated from "));
            textBlockInfoTemplateBuilder.AppendLine(XamlFileLinkString("onlinewebfonts.com/icon/{1}", "https://www.onlinewebfonts.com/icon/{1}"));
            textBlockInfoTemplateBuilder.AppendLine(XamlLineBreak());
            textBlockInfoTemplateBuilder.Append(XamText("Icon made from "));
            textBlockInfoTemplateBuilder.AppendLine(XamlFileLinkString("Icon Fonts", "http://www.onlinewebfonts.com/icon"));
            textBlockInfoTemplateBuilder.Append(XamText(" is licensed by CC BY 3.0"));
            textBlockInfoTemplateBuilder.AppendLine(XamlLineBreak());

            // must be formatted with {0} = filename and {1} = onlinewbefonts icon url id (number)
            var mdBaseInfoTemplate = mdBaseInfoTemplateBuilder.ToString();
            var textBlockInfoTemplate = textBlockInfoTemplateBuilder.ToString();

            var baseDirectory = new DirectoryInfo(iconBasePath);
            var allFiles = new List<string>();
            var unmatchedFiles = new List<string>();

            var matchedFileonlinewebfontsUrls = new Dictionary<string, int>();

            // we dont expect more levels than just one
            foreach (var subDir in baseDirectory.GetDirectories())
            {
                foreach (var file in subDir.GetFiles())
                {
                    // this still relies on the convention that ONLY one number shall exist in the filename and that is the identifier for onlinewebfonts
                    var pattern = new Regex(@".*icon\d+\..*");
                    var numberPattern = new Regex(@"\d+");

                    matchedFileonlinewebfontsUrls.Add(
                        file.Name,
                        pattern.IsMatch(file.Name) ? int.Parse(numberPattern.Match(file.Name).Value) : -1);

                    allFiles.Add(file.FullName);
                }
            }

            var mdFileCopyrightInfos = new Dictionary<string, string>();
            var xamlFileCopyrightInfos = new Dictionary<string, string>();

            foreach (var file in matchedFileonlinewebfontsUrls)
            {
                if (file.Value > -1)
                {
                    mdFileCopyrightInfos.Add(file.Key, string.Format(mdBaseInfoTemplate, file.Key, file.Value.ToString()));
                    xamlFileCopyrightInfos.Add(file.Key, string.Format(textBlockInfoTemplate, file.Key, file.Value.ToString()));
                }
                else
                {
                    unmatchedFiles.Add(file.Key);
                }
            }

            string[] rawMd = mdFileCopyrightInfos.Values.ToArray();
            string[] rawXaml = xamlFileCopyrightInfos.Values.ToArray();

            var mdOutFile = new FileInfo(copyRightFilePath);
            if (mdOutFile.Exists) File.Delete(mdOutFile.FullName);
            File.WriteAllLines(mdOutFile.FullName, rawMd);

            var xamlOutFile = new FileInfo("xaml-out-test.txt");
            if (xamlOutFile.Exists) File.Delete(xamlOutFile.FullName);
            File.WriteAllLines(xamlOutFile.Name, rawXaml);

            // ToDo include condents for unmatched files also!

            // unmatched files should only contain the self created "audio player" style buttons (skip/pause/play/record/stop)
            // as well as logo files and installer files (which also contain the logo)

            //File.WriteAllText()

            // ToDo: make actual comparison icon_generation path and used images in ressources and installer
            // right now this has to be done manually, following the nameing convention

        }

        private static string MdFileLinkString(string caption, string url)
        {
            // md links: [link text](link url)
            string mdUrlPattern = "[{0}]({1})";
            return string.Format(mdUrlPattern, caption, url);
        }

        private static string XamlLineBreak()
        {
            return "\t\t\t\t\t\t<LineBreak />";
        }

        private static string XamText(string content)
        {
            var contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("\t\t\t\t\t\t<Run>");
            contentBuilder.AppendLine("\t\t\t\t\t\t\t" + content);
            contentBuilder.AppendLine("\t\t\t\t\t\t</Run>");

            return contentBuilder.ToString();
        }

        private static string XamlFileLinkString(string caption, string url)
        {
            // xaml links: <Hyperlink NavigateUri="link url" RequestNavigate="Hyperlink_RequestNavigate" Style="{StaticResource Default_Hyperlink_Style}">link text</ Hyperlink>
            var xamlUrlPatternBuilder = new StringBuilder();
            xamlUrlPatternBuilder.AppendLine("\t\t\t\t\t\t<Hyperlink NavigateUri=\"{0}\" RequestNavigate=\"Hyperlink_RequestNavigate\">{1}</Hyperlink>");

            return string.Format(xamlUrlPatternBuilder.ToString(), url, caption);
        }


    }
}
