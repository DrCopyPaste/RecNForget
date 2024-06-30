using CommunityToolkit.Mvvm.Input;
using RecNForget.Services.Contracts;
using RecNForget.WPF.Services.Contracts;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for SelectedFileControl.xaml
    /// </summary>
    public partial class SelectedFileControl : UserControl, INotifyPropertyChanged
    {
        private readonly IActionService actionService = null;
        private readonly IAppSettingService appSettingService = null;
        private readonly IAudioPlaybackService audioPlaybackService = null;

        public SelectedFileControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string FileInfoText
        {
            get { return (string)GetValue(FileInfoTextProperty); }
            set { SetValue(FileInfoTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileInfoText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileInfoTextProperty =
            DependencyProperty.Register("FileInfoText", typeof(string), typeof(SelectedFileControl), new PropertyMetadata(string.Empty));

        private static void OnSelectedFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SelectedFileControl;
            control.OnPropertyChanged(nameof(HasSelectedFile));
        }

        public bool HasSelectedFile
        {
            get { return !string.IsNullOrEmpty(SelectedFilePath); }
        }

        public string SelectedFilePath
        {
            get { return (string)GetValue(SelectedFilePathProperty); }
            set { SetValue(SelectedFilePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedFilePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedFilePathProperty =
            DependencyProperty.Register("SelectedFilePath", typeof(string), typeof(SelectedFileControl), new PropertyMetadata(string.Empty, OnSelectedFilePathChanged));

        public RelayCommand ChangeSelectedFileNameCommand
        {
            get { return (RelayCommand)GetValue(ChangeSelectedFileNameCommandProperty); }
            set { SetValue(ChangeSelectedFileNameCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangeSelectedFileNameCommandProperty =
            DependencyProperty.Register("ChangeSelectedFileNameCommand", typeof(RelayCommand), typeof(SelectedFileControl), new PropertyMetadata(null));

        public RelayCommand DeleteSelectedFileCommand
        {
            get { return (RelayCommand)GetValue(DeleteSelectedFileCommandProperty); }
            set { SetValue(DeleteSelectedFileCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteSelectedFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteSelectedFileCommandProperty =
            DependencyProperty.Register("DeleteSelectedFileCommand", typeof(RelayCommand), typeof(SelectedFileControl), new PropertyMetadata(null));

        public RelayCommand ExportSelectedFileCommand
        {
            get { return (RelayCommand)GetValue(ExportSelectedFileCommandProperty); }
            set { SetValue(ExportSelectedFileCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExportSelectedFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExportSelectedFileCommandProperty =
            DependencyProperty.Register("ExportSelectedFileCommand", typeof(RelayCommand), typeof(SelectedFileControl), new PropertyMetadata(null));
    }
}
