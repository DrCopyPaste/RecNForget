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
        private static string copyRightControlPath = @"..\..\..\RecNForget\Controls\CopyrightControl.xaml";

        private static StringBuilder textBlockResultBuilder;
        private static StringBuilder mdFileResultBuilder;

        static void Main(string[] args)
        {
            var xamlHeader = new StringBuilder();
            xamlHeader.AppendLine("<!-- THIS FILE IS AUTO GENERATED -->");
            xamlHeader.AppendLine("<!-- don't change manually - INSTEAD run RecNForget.CopyRightHelper to replace -->");
            xamlHeader.AppendLine();
            xamlHeader.AppendLine(@"<UserControl x:Class=""RecNForget.Controls.CopyrightControl""");
            xamlHeader.AppendLine("\t\t" + @"xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""");
            xamlHeader.AppendLine("\t\t" + @"xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""");
            xamlHeader.AppendLine("\t\t" + @"xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""");
            xamlHeader.AppendLine("\t\t" + @"xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""");
            xamlHeader.AppendLine("\t\t" + @"xmlns:local=""clr-namespace:RecNForget.Controls""");
            xamlHeader.AppendLine("\t\t" + @"mc:Ignorable=""d"">");
            xamlHeader.AppendLine("\t" + @"<Grid>");
            xamlHeader.AppendLine("\t\t" + @"<TextBlock Style = ""{StaticResource DefaultTextBlockStyle}"" >");

            var xamlFooter = new StringBuilder();
            xamlFooter.AppendLine("\t\t" + "</TextBlock>");
            xamlFooter.AppendLine("\t" + "</Grid>");
            xamlFooter.AppendLine("</UserControl>");

            textBlockResultBuilder = new StringBuilder();
            textBlockResultBuilder.Append(xamlHeader.ToString());

            mdFileResultBuilder = new StringBuilder();

            /*START EDITING COPYRIGHT HERE*/
            /*START EDITING COPYRIGHT HERE*/
            /*START EDITING COPYRIGHT HERE*/

            AddText("RecNForget is written in C# using .NET Framework 4.8 and Microsoft WPF");
            AddLineBreak();
            AddText("Code by DrCopyPaste ");
            AddLink("github.com/DrCopyPaste/RecNForget", "https://github.com/DrCopyPaste/RecNForget");
            AddLineBreak();
            AddText("Logo by DrCopyPaste (using icons from onlinewebfonts.com - see below)");
            AddLineBreak();
            AddText("Beep sounds by a cheap program from DrCopyPaste with love.");
            AddLineBreak();
            AddText("Click sounds by ");
            AddLink("lit-audio@gmx.de", "mailto:lit-audio@gmx.de");
            AddText("/");
            AddLink("soundcloud.com/wolfgankh", "https://soundcloud.com/wolfgankh");
            AddLineBreak();
            AddLineBreak();
            AddText("The following libaries are used by RecNForget:");
            AddLineBreak();

            // there seems no way around it, included dlls must me added and maintained manually
            // one COULD of course maybe parse nuget feed of included packages and just blindly paste the license that it could query from there
            // but then again licenses DO differ quite a lot
            // best to handle this on a case by case basis and keep includes to a minimum
            // ToDo persist licenses instead of just linking to online content

            // ------------------ LIBRARIES LIST ----------------------------------

            AddLibraryLink(
                libaryName: "FMUtils.KeyboardHook.1.0.140.2145",
                libraryUrl: "https://github.com/factormystic/FMUtils.KeyboardHook#readme",
                manufacturerName: "Factor Mystic",
                licenseCaption: "License",
                licenseUrl: "https://github.com/factormystic/FMUtils.KeyboardHook/blob/master/license.txt");

            AddLibraryLink(
                libaryName: "Hardcodet.NotifyIcon.Wpf.1.0.8",
                libraryUrl: "http://www.hardcodet.net/wpf-notifyicon",
                manufacturerName: "Philipp Sumi",
                licenseCaption: "CPOL 1.02",
                licenseUrl: "https://www.codeproject.com/info/cpol10.aspx");

            AddLibraryLink(
                libaryName: "NAudio.1.10.0",
                libraryUrl: "https://github.com/naudio/NAudio",
                manufacturerName: "Mark Heath & Contributors",
                licenseCaption: "Ms-PL",
                licenseUrl: "https://github.com/naudio/NAudio/blob/master/license.txt");

            AddLibraryLink(
                libaryName: "Nerdbank.GitVersioning.3.0.50",
                libraryUrl: "https://github.com/aarnott/Nerdbank.GitVersioning",
                manufacturerName: "Andrew Arnott",
                licenseCaption: "MIT",
                licenseUrl: "https://licenses.nuget.org/MIT");

            AddLibraryLink(
                libaryName: "Newtonsoft.Json.12.0.3",
                libraryUrl: "https://www.newtonsoft.com/json",
                manufacturerName: "James Newton-King",
                licenseCaption: "MIT",
                licenseUrl: "https://licenses.nuget.org/MIT");

            AddLibraryLink(
                libaryName: "Octokit.0.40.0",
                libraryUrl: "https://github.com/octokit/octokit.net",
                manufacturerName: "GitHub",
                licenseCaption: "MIT",
                licenseUrl: "https://licenses.nuget.org/MIT");

            AddLibraryLink(
                libaryName: "Ookii.Dialogs.Wpf.1.1.0",
                libraryUrl: "https://github.com/caioproiete/ookii-dialogs-wpf",
                manufacturerName: "Sven Groot,Caio Proiete",
                licenseCaption: "License",
                licenseUrl: "https://github.com/caioproiete/ookii-dialogs-wpf/blob/master/LICENSE");

            AddLibraryLink(
                libaryName: "WiX Toolset build tools v3.11.2.4516",
                libraryUrl: "https://wixtoolset.org/",
                manufacturerName: ".NET Foundation",
                licenseCaption: "MS-RL",
                licenseUrl: "https://wixtoolset.org/about/license/");

            // ------------------ END LIBRARIES LIST ----------------------------------

            AddLineBreak();
            AddText("The following images were used to create RecNForget:");
            AddLineBreak();

            /*END EDITING COPYRIGHT HERE*/
            /*END EDITING COPYRIGHT HERE*/
            /*END EDITING COPYRIGHT HERE*/

            var mdBaseInfoTemplateBuilder = new StringBuilder();
            mdBaseInfoTemplateBuilder.AppendLine("{0} was generated from " + MdLink("onlinewebfonts.com/icon/{1}", "https://www.onlinewebfonts.com/icon/{1}"));
            mdBaseInfoTemplateBuilder.AppendLine();
            mdBaseInfoTemplateBuilder.AppendLine(string.Format(@"Icon made from {0} is licensed by CC BY 3.0", MdLink("Icon Fonts", "http://www.onlinewebfonts.com/icon")));
            mdBaseInfoTemplateBuilder.AppendLine();

            var textBlockInfoTemplateBuilder = new StringBuilder();
            textBlockInfoTemplateBuilder.Append(XamlText("{0} was generated from "));
            textBlockInfoTemplateBuilder.AppendLine(XamlLink("onlinewebfonts.com/icon/{1}", "https://www.onlinewebfonts.com/icon/{1}"));
            textBlockInfoTemplateBuilder.AppendLine(XamlLineBreak());
            textBlockInfoTemplateBuilder.Append(XamlText("Icon made from "));
            textBlockInfoTemplateBuilder.AppendLine(XamlLink("Icon Fonts", "http://www.onlinewebfonts.com/icon"));
            textBlockInfoTemplateBuilder.Append(XamlText(" is licensed by CC BY 3.0"));
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

            foreach (var line in xamlFileCopyrightInfos.Values.OrderBy(v => v))
            {
                textBlockResultBuilder.Append(line);
            }

            foreach (var line in mdFileCopyrightInfos.Values.OrderBy(v => v))
            {
                mdFileResultBuilder.Append(line);
            }

            var expectedUnmatched = unmatchedFiles.Where(
                f =>
                    f.EndsWith("WixUI-logo.svg")
                    || f.EndsWith("WixUIBannerBmp.bmp")
                    || f.EndsWith("WixUIBannerBmp.png")
                    || f.EndsWith("WixUIDialogBmp.bmp")
                    || f.EndsWith("WixUIDialogBmp.png")
                    || f.EndsWith("logo.ico")
                    || f.EndsWith("logo.svg")
                    || f.EndsWith("logo_big.png")
                    || f.EndsWith("logo_small.png"));

            foreach (var expectedUnmatchedFile in expectedUnmatched)
            {
                AddLogoFileLines(expectedUnmatchedFile);
            }

            textBlockResultBuilder.Append(xamlFooter);

            var leftUnmatched = unmatchedFiles.Except(expectedUnmatched);


            //string[] rawXaml = xamlFileCopyrightInfos.Values.ToArray();

            var mdOutFile = new FileInfo(copyRightFilePath);
            //if (mdOutFile.Exists) File.Delete(mdOutFile.FullName);
            File.WriteAllText(mdOutFile.FullName, mdFileResultBuilder.ToString());

            var xamlOutFile = new FileInfo(copyRightControlPath);
            //if (xamlOutFile.Exists) File.Delete(xamlOutFile.FullName);
            File.WriteAllText(xamlOutFile.FullName, textBlockResultBuilder.ToString());

            // ToDo include contents for unmatched files also!

            // unmatched files should only contain the self created "audio player" style buttons (skip/pause/play/record/stop)
            // as well as logo files and installer files (which also contain the logo)

            //File.WriteAllText()

            // ToDo: make actual comparison icon_generation path and used images in ressources and installer
            // right now this has to be done manually, following the nameing convention
        }

        private static void AddText(string text)
        {
            textBlockResultBuilder.Append(XamlText(text));

            mdFileResultBuilder.Append(text);
        }

        private static void AddLink(string caption, string url)
        {
            textBlockResultBuilder.Append(XamlLink(caption, url));

            mdFileResultBuilder.Append(MdLink(caption, url));
        }

        private static void AddLineBreak()
        {
            textBlockResultBuilder.AppendLine(XamlLineBreak());

            mdFileResultBuilder.AppendLine();
            mdFileResultBuilder.AppendLine();
        }

        private static void AddLogoFileLines(string logoFileName)
        {
            AddText(logoFileName + " was generated from ");
            AddLink("onlinewebfonts.com/icon/55290", "https://www.onlinewebfonts.com/icon/55290");
            AddText(" and ");
            AddLink("onlinewebfonts.com/icon/715", "https://www.onlinewebfonts.com/icon/715");
            AddLineBreak();
            AddText("Icon made from ");
            AddLink("Icon Fonts", "http://www.onlinewebfonts.com/icon");
            AddText(" is licensed by CC BY 3.0");
            AddLineBreak();
        }

        private static void AddLibraryLink(string libaryName, string libraryUrl, string manufacturerName, string licenseCaption, string licenseUrl)
        {
            AddLink(libaryName, libraryUrl);
            AddText(" Copyright (c) " + manufacturerName + " - ");
            AddLink(licenseCaption, licenseUrl);
            AddLineBreak();
        }

        private static string MdLink(string caption, string url)
        {
            // md links: [link text](link url)
            string mdUrlPattern = "[{0}]({1})";
            return string.Format(mdUrlPattern, caption, url);
        }

        private static string XamlLineBreak()
        {
            return "\t\t\t<LineBreak />";
        }

        private static string XamlText(string content)
        {
            var contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("\t\t\t<Run>");
            contentBuilder.AppendLine("\t\t\t\t" + content.Replace("&", "&amp;"));
            contentBuilder.AppendLine("\t\t\t</Run>");

            return contentBuilder.ToString();
        }

        private static string XamlLink(string caption, string url)
        {
            // xaml links: <Hyperlink NavigateUri="link url" RequestNavigate="Hyperlink_RequestNavigate" Style="{StaticResource Default_Hyperlink_Style}">link text</ Hyperlink>
            var xamlUrlPatternBuilder = new StringBuilder();
            xamlUrlPatternBuilder.AppendLine("\t\t\t<Hyperlink NavigateUri=\"{0}\" RequestNavigate=\"Hyperlink_RequestNavigate\">{1}</Hyperlink>");

            return string.Format(xamlUrlPatternBuilder.ToString(), url, caption.Replace("&", "&amp;"));
        }


    }
}
