using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for TimeSpanTextBox.xaml
    /// 
    /// Display can awlays be set directly via TextValueTimeSpan
    /// When the control (TimeSpanTextBox) is enabled a change in ExternallyBoundTextValueTimeSpan also triggers a value change of TextValueTimeSpan and vice versa (if that value is valid)
    /// 
    /// 
    /// This does explicitly NOT expose an actual timespan value
    /// When enabled this textbox updates the bound value from "TextValueTimeSpan" according to current valid text contents
    /// When disabled this does not update the underlying bound value (can be used for countdown timer mode in which u dont want to update the underlyling setting every second)
    /// </summary>
    public partial class TimeSpanTextBox : UserControl, INotifyPropertyChanged
    {
        public static string ParseFormat = "d':'hh':'mm':'ss";
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string TimerBoundTextValueTimeSpan
        {
            get { return (string)GetValue(TimerBoundTextValueTimeSpanProperty); }
            set { SetValue(TimerBoundTextValueTimeSpanProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimerBoundTextValueTimeSpan.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimerBoundTextValueTimeSpanProperty =
            DependencyProperty.Register("TimerBoundTextValueTimeSpan", typeof(string), typeof(TimeSpanTextBox), new PropertyMetadata(string.Empty));

        /// <summary>
        /// This binds to the setting value corresponding to the actually saved time
        /// </summary>
        public string SettingTextValueTimeSpan
        {
            get { return (string)GetValue(SettingTextValueTimeSpanProperty); }
            set {
                SetValue(SettingTextValueTimeSpanProperty, value);

                var parseSuccessful = TimeSpan.TryParseExact(value, ParseFormat, CultureInfo.InvariantCulture, out TimeSpan outputTimeSpan);
                ValidationErrorMark.Visibility = parseSuccessful ? Visibility.Hidden : Visibility.Visible;

                OnPropertyChanged();
            }
        }

        // Using a DependencyProperty as the backing store for ExternallyBoundTextValueTimeSpan.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingTextValueTimeSpanProperty =
            DependencyProperty.Register("SettingTextValueTimeSpan", typeof(string), typeof(TimeSpanTextBox), new PropertyMetadata("0:00:00:00"));

        public TimeSpanTextBox()
        {
            DataContext = this;
            InitializeComponent();

            this.IsEnabledChanged += TimeSpanTextBox_IsEnabledChanged;
            ShowEditableTextBoxConditionally();

            if (DesignerProperties.GetIsInDesignMode(this))
            { }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;
            }
        }

        private void TimeSpanTextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ShowEditableTextBoxConditionally();
        }

        private void ShowEditableTextBoxConditionally()
        {
            // if enabled show editable timespanbox SettingTextValueTimeSpanBoxTextBox which binds to SettingTextValueTimeSpan
            // otherwise show disabled timespanbox TimerBoundTextValueTimeSpan which binds to SettingTextValueTimeSpan
            SettingTextValueTimeSpanBoxTextBox.Visibility = IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            TimerBoundTextValueTimeSpanTextBox.Visibility = IsEnabled ? Visibility.Collapsed : Visibility.Visible;
        }

        void TimeSpanTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var newValue = (sender as TextBox).Text;
            var tbEntry = sender as TextBox;

            // remove all chars but 0-9
            Regex rgx = new Regex("[^0-9]");
            var workingValue = rgx.Replace(newValue, "");
            // remove all leading zeroes
            workingValue = workingValue.TrimStart('0');
            // pad with zeroes from the left to have at least 7 digits total (more than 7 digits means more digits for days...
            workingValue = workingValue.Length < 7 ? workingValue.PadLeft(7, '0') : workingValue;
            // format with : from right 3 times afer every second digit
            workingValue = workingValue.Insert(workingValue.Length - 2, ":");
            workingValue = workingValue.Insert(workingValue.Length - 5, ":");
            workingValue = workingValue.Insert(workingValue.Length - 8, ":");

            // maybe only write back setting value when focus is lost?
            tbEntry.Text = workingValue;
            tbEntry.CaretIndex = tbEntry.Text.Length;
        }
    }
}
