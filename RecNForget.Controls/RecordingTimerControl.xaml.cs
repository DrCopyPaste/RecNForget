using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
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

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for RecordingTimerControl.xaml
    /// </summary>
    public partial class RecordingTimerControl : UserControl, INotifyPropertyChanged
    {
        private string currentCountDownTimerMax = "0:00:00:00"; // TimeSpan.Zero;

        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public string CurrentCountDownTimerMax
        {
            get { return currentCountDownTimerMax; }
            set
            {
                currentCountDownTimerMax = value;
                OnPropertyChanged();

                TimeSpan outputTimeSpan = TimeSpan.Zero;
                if (TimeSpan.TryParseExact(value, "d':'hh':'mm':'ss", CultureInfo.CurrentCulture, out outputTimeSpan))
                {
                    internalTimeSpan = outputTimeSpan;
                }
            }
        }

        private TimeSpan internalTimeSpan = TimeSpan.Zero;
        private TimeSpan originalTimeSpan = TimeSpan.Zero;
        private TimeSpan timerTimeSpan = TimeSpan.Zero;


        public RecordingTimerControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TimeSpanTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //TimerTimeSpanTextBox.Select(0, 0);
                e.Handled = true;
        }

        private void DeleteSelectedFileButton_Click(object sender, RoutedEventArgs e)
        {
            timerTimeSpan = internalTimeSpan;
            originalTimeSpan = timerTimeSpan;

            TimerTimeSpanTextBox.IsEnabled = false;

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (timerTimeSpan.TotalSeconds > 0)
            {
                timerTimeSpan = timerTimeSpan.Subtract(TimeSpan.FromSeconds(1));
                CurrentCountDownTimerMax = $"{timerTimeSpan.Days}:{timerTimeSpan.Hours:D2}:{timerTimeSpan.Minutes:D2}:{timerTimeSpan.Seconds:D2}";
            }
            else
            {
                var originalTimerMax = originalTimeSpan;
                CurrentCountDownTimerMax = $"{originalTimerMax.Days}:{originalTimerMax.Hours:D2}:{originalTimerMax.Minutes:D2}:{originalTimerMax.Seconds:D2}";

                dispatcherTimer.Stop();
            }
        }
    }
}
