using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for OutputPathControl.xaml
    /// </summary>
    public partial class OutputPathControl : UserControl
    {
        public OutputPathControl()
        {
            InitializeComponent();
        }

        public string ProjectedOutputPathIncludingFilePattern
        {
            get { return (string)GetValue(ProjectedOutputPathIncludingFilePatternProperty); }
            set { SetValue(ProjectedOutputPathIncludingFilePatternProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectedOutputPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectedOutputPathIncludingFilePatternProperty =
            DependencyProperty.Register("ProjectedOutputPathIncludingFilePattern", typeof(string), typeof(OutputPathControl), new PropertyMetadata(default(string)));

        public RelayCommand UpdateFileNamePatternCommand
        {
            get { return (RelayCommand)GetValue(UpdateFileNamePatternCommandProperty); }
            set { SetValue(UpdateFileNamePatternCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateFileNamePatternCommandProperty =
            DependencyProperty.Register("UpdateFileNamePatternCommand", typeof(RelayCommand), typeof(OutputPathControl), new PropertyMetadata(null));

        public RelayCommand UpdateOutputFolderCommand
        {
            get { return (RelayCommand)GetValue(UpdateOutputFolderCommandProperty); }
            set { SetValue(UpdateOutputFolderCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateOutputFolder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateOutputFolderCommandProperty =
            DependencyProperty.Register("UpdateOutputFolderCommand", typeof(RelayCommand), typeof(OutputPathControl), new PropertyMetadata(null));
    }
}
