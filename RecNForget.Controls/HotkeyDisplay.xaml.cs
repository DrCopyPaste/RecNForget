using RecNForget.Controls.Extensions;
using RecNForget.Controls.Helper;
using RecNForget.Controls.IoC;
using RecNForget.Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for HotkeyDisplay.xaml
    /// </summary>
    public partial class HotkeyDisplay : UserControl
    {
        public string HotkeySettingString
        {
            get { return (string)GetValue(HotkeySettingStringProperty); }
            set { SetValue(HotkeySettingStringProperty, value); }
        }       

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HotkeySettingStringProperty =
            DependencyProperty.Register("HotkeySettingString", typeof(string), typeof(HotkeyDisplay), new PropertyMetadata(default(string), HotkeySettingStringChanged));

        private static void HotkeySettingStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HotkeyDisplay)
            {
                var display = d as HotkeyDisplay;



                if (display.HotkeyStackPanel.Children.Count > 0)
                {
                    display.HotkeyStackPanel.Children.Clear();
                }

                var separatedKeyStrings = ListFromSettingString(display.HotkeySettingString);

                var grid = GetHotkeyListAsButtonGrid(
                    hotkeys: separatedKeyStrings,
                    buttonStyle: (Style)display.FindResource("HotkeyDisplayButton"),
                    labelStyle: (Style)display.FindResource("Base_Label"),
                    spacing: 3
                    
                    );

                display.HotkeyStackPanel.Children.Add(grid);
            }
        }

        private static List<string> ListFromSettingString(string setting)
        {
            List<string> keys = new List<string>();

            // modifier keys
            if (setting.Contains("Shift=True"))
            {
                keys.Add("Shift");
            }

            if (setting.Contains("Ctrl=True"))
            {
                keys.Add("Ctrl");
            }

            if (setting.Contains("Alt=True"))
            {
                keys.Add("Alt");
            }

            if (setting.Contains("Win=True"))
            {
                keys.Add("Win");
            }

            var actualKey = setting.Split(';')[0].Replace("Key=", string.Empty);
            if (actualKey != string.Empty && actualKey != "None")
            {
                keys.Add(actualKey);
            }

            return keys;
        }

        public HotkeyDisplay()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            { }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;
            }
        }

        public static StackPanel GetHotkeyListAsButtonGrid(List<string> hotkeys, Style buttonStyle = null, Style labelStyle = null, double ? spacing = null, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Center, System.Windows.VerticalAlignment verticalAlignment = System.Windows.VerticalAlignment.Center)
        {
            var stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            for (int i= 0; i < hotkeys.Count; i++)
            {
                var tempButton = new System.Windows.Controls.Button();
                tempButton.HorizontalAlignment = horizontalAlignment;
                tempButton.VerticalAlignment = verticalAlignment;
                if (buttonStyle != null)
                {
                    tempButton.Style = buttonStyle;
                }

                tempButton.Content = hotkeys[i];

                stackPanel.Children.Add(tempButton);

                if (i != (hotkeys.Count - 1))
                {
                    // insert separator only if this is not the only (or the last) element
                    var label = new Label();
                    label.HorizontalAlignment = horizontalAlignment;
                    label.VerticalAlignment = verticalAlignment;
                    label.Padding = new System.Windows.Thickness(spacing ?? 1d);
                    label.Content = "+";
                    if (labelStyle != null)
                    {
                        label.Style = labelStyle;
                    }

                    stackPanel.Children.Add(label);
                }
            }

            return stackPanel;
        }
    }
}
