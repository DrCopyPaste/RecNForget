using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RecNForget.Controls
{
    public class TimeSpanTextBox : TextBox
    {
        //public TextBoxMask Mask { get; set; }

        private string internalStringMemory = string.Empty;

        public TimeSpanTextBox()
        {
            this.TextChanged += new TextChangedEventHandler(MaskedTextBox_TextChanged);
        }

        void MaskedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var oldValue = internalStringMemory;
            //TimeSpan oldTimeSpan = TimeSpan.Zero;
            //var oldTimeSpanResultOK = TimeSpan.TryParseExact(oldValue, "d':'hh':'mm':'ss", CultureInfo.CurrentCulture, out oldTimeSpan);

            //var oldDays = oldTimeSpan.Days;
            //var oldHours = oldTimeSpan.Hours;
            //var oldMinutes = oldTimeSpan.Minutes;
            //var oldSeconds = oldTimeSpan.Seconds;

            var newValue = (sender as TimeSpanTextBox).Text;            

            //this.CaretIndex = this.Text.Length;
            var tbEntry = sender as TimeSpanTextBox;

            // remove all chars but 0-9
            Regex rgx = new Regex("[^0-9]");
            var workingValue = rgx.Replace(newValue, "");

            // remove all leading zeroes
            workingValue = workingValue.TrimStart('0');

            // pad with zeroes from the left to have at least 7 digits total (more than 7 digits means more digits for days...
            workingValue = workingValue.Length < 7 ? workingValue.PadLeft(7, '0') : workingValue;

            //var newSeconds = int.Parse(workingValue.Substring(workingValue.Length - 2, 2));
            //var newMinutes = int.Parse(workingValue.Substring(workingValue.Length - 4, 2));
            //var newHours = int.Parse(workingValue.Substring(workingValue.Length - 6, 2));
            //var newDays = int.Parse(workingValue.Substring(0, workingValue.Length - 6));

            //var secondsChanged = newSeconds == oldSeconds;
            //var minutesChanged = newMinutes == oldMinutes;
            //var hoursChanged = newHours == oldHours;
            //var daysChanged = newDays == oldDays;

            // format with : from right 3 times afer every second digit
            workingValue = workingValue.Insert(workingValue.Length - 2, ":");
            workingValue = workingValue.Insert(workingValue.Length - 5, ":");
            workingValue = workingValue.Insert(workingValue.Length - 8, ":");

            tbEntry.Text = workingValue;
            internalStringMemory = workingValue;

            this.CaretIndex = this.Text.Length;
        }
    }
}
