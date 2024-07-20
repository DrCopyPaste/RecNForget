using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for RecordingTimerControl.xaml
    /// </summary>
    public partial class RecordingTimerControl : UserControl
    {
        public RecordingTimerControl()
        {
            InitializeComponent();
        }

        public bool TimerStartAfterIsEnabled
        {
            get { return (bool)GetValue(TimerStartAfterIsEnabledProperty); }
            set { SetValue(TimerStartAfterIsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimerStartAfterIsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimerStartAfterIsEnabledProperty =
            DependencyProperty.Register("TimerStartAfterIsEnabled", typeof(bool), typeof(RecordingTimerControl), new PropertyMetadata(false));

        public bool TimerStopAfterIsEnabled
        {
            get { return (bool)GetValue(TimerStopAfterIsEnabledProperty); }
            set { SetValue(TimerStopAfterIsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimerStopAfterIsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimerStopAfterIsEnabledProperty =
            DependencyProperty.Register("TimerStopAfterIsEnabled", typeof(bool), typeof(RecordingTimerControl), new PropertyMetadata(false));

        public string CurrentRecordingStartAfterTimer
        {
            get { return (string)GetValue(CurrentRecordingStartAfterTimerProperty); }
            set { SetValue(CurrentRecordingStartAfterTimerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentRecordingStartAfterTimer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentRecordingStartAfterTimerProperty =
            DependencyProperty.Register("CurrentRecordingStartAfterTimer", typeof(string), typeof(RecordingTimerControl), new PropertyMetadata(string.Empty));

        public string CurrentRecordingStopAfterTimer
        {
            get { return (string)GetValue(CurrentRecordingStopAfterTimerProperty); }
            set { SetValue(CurrentRecordingStopAfterTimerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentRecordingStopAfterTimer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentRecordingStopAfterTimerProperty =
            DependencyProperty.Register("CurrentRecordingStopAfterTimer", typeof(string), typeof(RecordingTimerControl), new PropertyMetadata(string.Empty));

        public RelayCommand ToggleStartAfterTimerIsEnabledRelayCommand
        {
            get { return (RelayCommand)GetValue(ToggleStartAfterTimerIsEnabledRelayCommandProperty); }
            set { SetValue(ToggleStartAfterTimerIsEnabledRelayCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleStartAfterTimerIsEnabledRelayCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleStartAfterTimerIsEnabledRelayCommandProperty =
            DependencyProperty.Register("ToggleStartAfterTimerIsEnabledRelayCommand", typeof(RelayCommand), typeof(RecordingTimerControl), new PropertyMetadata(null));

        public RelayCommand ToggleStopAfterTimerIsEnabledRelayCommand
        {
            get { return (RelayCommand)GetValue(ToggleStopAfterTimerIsEnabledRelayCommandProperty); }
            set { SetValue(ToggleStopAfterTimerIsEnabledRelayCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleStopAfterTimerIsEnabledRelayCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleStopAfterTimerIsEnabledRelayCommandProperty =
            DependencyProperty.Register("ToggleStopAfterTimerIsEnabledRelayCommand", typeof(RelayCommand), typeof(RecordingTimerControl), new PropertyMetadata(null));

        public bool StartAfterTimerIsRunning
        {
            get { return (bool)GetValue(StartAfterTimerIsRunningProperty); }
            set { SetValue(StartAfterTimerIsRunningProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartAfterTimerIsRunning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAfterTimerIsRunningProperty =
            DependencyProperty.Register("StartAfterTimerIsRunning", typeof(bool), typeof(RecordingTimerControl), new PropertyMetadata(null));

        public bool StopAfterTimerIsRunning
        {
            get { return (bool)GetValue(StopAfterTimerIsRunningProperty); }
            set { SetValue(StopAfterTimerIsRunningProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StopAfterTimerIsRunning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopAfterTimerIsRunningProperty =
            DependencyProperty.Register("StopAfterTimerIsRunning", typeof(bool), typeof(RecordingTimerControl), new PropertyMetadata(null));

        public string RecordingTimerStartAfterMax
        {
            get { return (string)GetValue(RecordingTimerStartAfterMaxProperty); }
            set { SetValue(RecordingTimerStartAfterMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RecordingTimerStartAfterMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecordingTimerStartAfterMaxProperty =
            DependencyProperty.Register("RecordingTimerStartAfterMax", typeof(string), typeof(RecordingTimerControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string RecordingTimerStopAfterMax
        {
            get { return (string)GetValue(RecordingTimerStopAfterMaxProperty); }
            set { SetValue(RecordingTimerStopAfterMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RecordingTimerStopAfterMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecordingTimerStopAfterMaxProperty =
            DependencyProperty.Register("RecordingTimerStopAfterMax", typeof(string), typeof(RecordingTimerControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
