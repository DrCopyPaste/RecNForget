using FMUtils.KeyboardHook;
using RecNForget.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for CloseOrBackgroundDialog.xaml
	/// </summary>
	public partial class CloseOrBackgroundDialog : Window
	{
		private bool ContinueInBackground { get; set; }

		public CloseOrBackgroundDialog()
		{
			InitializeComponent();

			this.Title = "Close or continue in background?";
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			ContinueInBackground = false;
			DialogResult = ContinueInBackground;
		}

		private void Background_Click(object sender, RoutedEventArgs e)
		{
			ContinueInBackground = true;
			DialogResult = ContinueInBackground;
		}
	}
}
