using RecNForget.Services.Contracts;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for TimeSpanTextBox.xaml
    /// 
    /// Display can always be set directly via TextValueTimeSpan
    /// When the control (TimeSpanTextBox) is enabled a change in ExternallyBoundTextValueTimeSpan also triggers a value change of TextValueTimeSpan and vice versa (if that value is valid)
    /// 
    /// 
    /// This does explicitly NOT expose an actual timespan value
    /// When enabled this textbox updates the bound value from "TextValueTimeSpan" according to current valid text contents
    /// When disabled this does not update the underlying bound value (can be used for countdown timer mode in which u dont want to update the underlyling setting every second)
    /// </summary>
    public partial class TimeSpanTextBox : UserControl
    {
        public string TimerBoundTextValueTimeSpan
        {
            get { return (string)GetValue(TimerBoundTextValueTimeSpanProperty); }
            set { SetValue(TimerBoundTextValueTimeSpanProperty, value); }
        }

        public static readonly DependencyProperty TimerBoundTextValueTimeSpanProperty =
            DependencyProperty.Register("TimerBoundTextValueTimeSpan", typeof(string), typeof(TimeSpanTextBox), new PropertyMetadata(string.Empty));

        /// <summary>
        /// This binds to the setting value corresponding to the actually saved time
        /// </summary>
        public string SettingTextValueTimeSpan
        {
            get { return (string)GetValue(SettingTextValueTimeSpanProperty); }
            set { SetValue(SettingTextValueTimeSpanProperty, value); }
        }

        public static readonly DependencyProperty SettingTextValueTimeSpanProperty =
            DependencyProperty.Register("SettingTextValueTimeSpan", typeof(string), typeof(TimeSpanTextBox), new PropertyMetadata("0:00:00:00"));

        public TimeSpanTextBox()
        {
            InitializeComponent();
            this.IsEnabledChanged += TimeSpanTextBox_IsEnabledChanged;
        }

        private void TimeSpanTextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ShowEditableTextBoxConditionally();
        }

        private void ShowEditableTextBoxConditionally()
        {
            // IsEnabled means setting can be edited, IsEnabled == false means timer is running
            SettingTextValueTimeSpanBoxTextBox.Visibility = IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            TimerBoundTextValueTimeSpanTextBox.Visibility = IsEnabled ? Visibility.Collapsed : Visibility.Visible;
        }

        void TimeSpanTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!e.Changes.Any()) return;

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

            var parseSuccessful = TimeSpan.TryParseExact(workingValue, Formats.TimeSpanFormat, CultureInfo.InvariantCulture, out TimeSpan outputTimeSpan);
            ValidationErrorMark.Visibility = parseSuccessful ? Visibility.Hidden : Visibility.Visible;

            // only update underlying bound value if the entered value was valid
            if (!parseSuccessful)
            {
                e.Handled = true;
                return;
            }

            var binding = BindingOperations.GetBindingExpression((TextBox)sender, TextBox.TextProperty);
            binding.Target.SetCurrentValue(TextBox.TextProperty, workingValue);
            binding.UpdateSource();

            tbEntry.CaretIndex = tbEntry.Text.Length;
            e.Handled = true;
        }
    }
}
