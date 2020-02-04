using FMUtils.KeyboardHook;
using Ookii.Dialogs.Wpf;
using RecNForget.Services;
using RecNForget.Services.Extensions;
using RecNForget.Services.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

namespace RecNForget.Windows
{
	/// <summary>
	/// Interaction logic for QuickTipDialog.xaml
	/// </summary>
	public partial class QuickTipDialog : INotifyPropertyChanged
	{
		public QuickTipDialog()
		{
			this.KeyDown += Window_KeyDown;

			InitializeComponent();
			DataContext = this;

			this.Title = "Did you know?";

			GenerateRandomTip();
		}

		private void GenerateRandomTip()
		{
			var allFeatures = Services.Types.HelpFeature.All.Where(f => f.FeatureClass == HelpFeatureClass.NewFeature).ToList();

			int randomNumber = (new Random()).Next(0, allFeatures.Count - 1);
			var randomFeature = allFeatures[randomNumber];

			FeatureCaption = randomFeature.Title;
			FeatureContents = randomFeature.HelpLinesAsString();
		}

		private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				this.Close();
			}
		}
		private string featureCaption;
		private string featureContents;

		public string FeatureCaption
		{
			get
			{
				return featureCaption;
			}

			set
			{
				featureCaption = value;
				OnPropertyChanged();
			}
		}

		public string FeatureContents
		{
			get
			{
				return featureContents;
			}

			set
			{
				featureContents = value;
				OnPropertyChanged();
			}
		}

		public bool ShowTipsOnApplicationStart
		{
			get
			{
				return Convert.ToBoolean(AppSettingHelper.GetAppConfigSetting(AppSettingHelper.ShowTipsAtApplicationStart));
			}

			set
			{
				AppSettingHelper.SetAppConfigSetting(AppSettingHelper.ShowTipsAtApplicationStart, value.ToString());
				OnPropertyChanged();
			}
		}

		private void GenerateAnotherTip_Click(object sender, RoutedEventArgs e)
		{
			GenerateRandomTip();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		#region configuration event handlers

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
