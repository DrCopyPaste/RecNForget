using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for RecordingAndPlaybackControl.xaml
    /// </summary>
    public partial class RecordingAndPlaybackControl : UserControl
    {
        public RecordingAndPlaybackControl()
        {
            InitializeComponent();
        }

        public RelayCommand SelectInExplorerCommand
        {
            get { return (RelayCommand)GetValue(SelectInExplorerCommandProperty); }
            set { SetValue(SelectInExplorerCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectInExplorerCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectInExplorerCommandProperty =
            DependencyProperty.Register("SelectInExplorerCommand", typeof(RelayCommand), typeof(RecordingAndPlaybackControl), new PropertyMetadata(null));

        public RelayCommand SkipPrevCommand
        {
            get { return (RelayCommand)GetValue(SkipPrevCommandProperty); }
            set { SetValue(SkipPrevCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SkipPrevCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SkipPrevCommandProperty =
            DependencyProperty.Register("SkipPrevCommand", typeof(RelayCommand), typeof(RecordingAndPlaybackControl), new PropertyMetadata(null));

        public RelayCommand PlayPauseCommand
        {
            get { return (RelayCommand)GetValue(PlayPauseCommandProperty); }
            set { SetValue(PlayPauseCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayPauseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayPauseCommandProperty =
            DependencyProperty.Register("PlayPauseCommand", typeof(RelayCommand), typeof(RecordingAndPlaybackControl), new PropertyMetadata(null));

        public RelayCommand StopCommand
        {
            get { return (RelayCommand)GetValue(StopCommandProperty); }
            set { SetValue(StopCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StopCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopCommandProperty =
            DependencyProperty.Register("StopCommand", typeof(RelayCommand), typeof(RecordingAndPlaybackControl), new PropertyMetadata(null));

        public RelayCommand SkipNextCommand
        {
            get { return (RelayCommand)GetValue(SkipNextCommandProperty); }
            set { SetValue(SkipNextCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SkipNextCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SkipNextCommandProperty =
            DependencyProperty.Register("SkipNextCommand", typeof(RelayCommand), typeof(RecordingAndPlaybackControl), new PropertyMetadata(null));

        public RelayCommand ToggleRecordCommand
        {
            get { return (RelayCommand)GetValue(ToggleRecordCommandProperty); }
            set { SetValue(ToggleRecordCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleRecordCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleRecordCommandProperty =
            DependencyProperty.Register("ToggleRecordCommand", typeof(RelayCommand), typeof(RecordingAndPlaybackControl), new PropertyMetadata(null));

        public bool SkipPrevButtonEnabled
        {
            get { return (bool)GetValue(SkipPrevButtonEnabledProperty); }
            set { SetValue(SkipPrevButtonEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SkipPrevButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SkipPrevButtonEnabledProperty =
            DependencyProperty.Register("SkipPrevButtonEnabled", typeof(bool), typeof(RecordingAndPlaybackControl), new PropertyMetadata(false));

        public bool PlayPauseButtonEnabled
        {
            get { return (bool)GetValue(PlayPauseButtonEnabledProperty); }
            set { SetValue(PlayPauseButtonEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayPauseButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayPauseButtonEnabledProperty =
            DependencyProperty.Register("PlayPauseButtonEnabled", typeof(bool), typeof(RecordingAndPlaybackControl), new PropertyMetadata(false));

        public bool StopButtonEnabled
        {
            get { return (bool)GetValue(StopButtonEnabledProperty); }
            set { SetValue(StopButtonEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StopButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopButtonEnabledProperty =
            DependencyProperty.Register("StopButtonEnabled", typeof(bool), typeof(RecordingAndPlaybackControl), new PropertyMetadata(false));

        public bool SkipNextButtonEnabled
        {
            get { return (bool)GetValue(SkipNextButtonEnabledProperty); }
            set { SetValue(SkipNextButtonEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SkipNextButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SkipNextButtonEnabledProperty =
            DependencyProperty.Register("SkipNextButtonEnabled", typeof(bool), typeof(RecordingAndPlaybackControl), new PropertyMetadata(false));

        public bool RecordButtonEnabled
        {
            get { return (bool)GetValue(RecordButtonEnabledProperty); }
            set { SetValue(RecordButtonEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RecordButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecordButtonEnabledProperty =
            DependencyProperty.Register("RecordButtonEnabled", typeof(bool), typeof(RecordingAndPlaybackControl), new PropertyMetadata(false));
    }
}
