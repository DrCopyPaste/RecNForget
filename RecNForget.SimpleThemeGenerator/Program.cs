using System;
using System.IO;
using System.Text.RegularExpressions;

namespace RecNForget.SimpleThemeGenerator
{
    class Program
    {
        private static string baseThemeName = @"Simple_White";
        private static string baseThemePath = $@"..\..\..\..\RecNForget.Controls\Themes\{baseThemeName}.xaml";

        private static string simpleWhiteColors = @"
    <Color x:Key=""Transparent_Color"">Transparent</Color>
    <Brush x:Key=""Transparent_Brush"">Transparent</Brush>

    <Color x:Key=""Primary_Color"">Black</Color>
    <Brush x:Key=""Primary_Brush"">Black</Brush>

    <Color x:Key=""Secondary_Color"">White</Color>
    <Brush x:Key=""Secondary_Brush"">White</Brush>


    <Color x:Key=""PrimaryShade_Color"">#808080</Color>
    <Brush x:Key=""PrimaryShade_Brush"">#808080</Brush>

    <Color x:Key=""SecondaryShade_Color"">#dddddd</Color>
    <Brush x:Key=""SecondaryShade_Brush"">#dddddd</Brush>
";
        private static string simpleBlackColors = @"
    <Color x:Key=""Transparent_Color"">Transparent</Color>
    <Brush x:Key=""Transparent_Brush"">Transparent</Brush>

    <Color x:Key=""Primary_Color"">White</Color>
    <Brush x:Key=""Primary_Brush"">White</Brush>

    <Color x:Key=""Secondary_Color"">Black</Color>
    <Brush x:Key=""Secondary_Brush"">Black</Brush>


    <Color x:Key=""PrimaryShade_Color"">#dddddd</Color>
    <Brush x:Key=""PrimaryShade_Brush"">#dddddd</Brush>

    <Color x:Key=""SecondaryShade_Color"">#808080</Color>
    <Brush x:Key=""SecondaryShade_Brush"">#808080</Brush>
";

        static void Main(string[] args)
        {
            var baseThemeContents = File.ReadAllText(baseThemePath);
            var colorSeparator = @"<!--RecNForgetThemeColorSeparator_Leave_This_embracing_start_and_end_of_color_definition-->";

            // expect exactly 3 parts in correct order: head, colors themselves, rest
            var fileParts = baseThemeContents.Split(colorSeparator);
            var partToReplace = colorSeparator + fileParts[1] + colorSeparator;
            var textToInsert = colorSeparator + simpleBlackColors + colorSeparator;

            var replacedText = baseThemeContents.Replace(partToReplace, textToInsert);

            File.WriteAllText(baseThemePath.Replace(baseThemeName, "Simple_Black"), replacedText);

            Console.WriteLine("Hello World!");
        }
    }
}
