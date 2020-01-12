using RecNForget.Services;
using RecNForget.Services.Extensions;
using RecNForget.Services.Types;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace RecNForget
{
	/// <summary>
	/// Interaction logic for HelpWindow.xaml
	/// </summary>
	public partial class HelpWindow : Window
	{
		private List<HelpFeature> allFeatures;
		private HelpFeature quickStart;

		public HelpWindow()
		{
            InitializeComponent();

			quickStart = new Help.General.QuickStart();
			this.allFeatures = Services.Types.HelpFeature.All;
			int topicRowCount = 0;

			var quickStartrowDefinition = new RowDefinition();
			quickStartrowDefinition.Height = GridLength.Auto;
			TopicListGrid.RowDefinitions.Add(quickStartrowDefinition);

			var quickStartButton = new Button();
			quickStartButton.Name = quickStart.Id;
			quickStartButton.Content = quickStart.Title;
			quickStartButton.HorizontalAlignment = HorizontalAlignment.Stretch;

			quickStartButton.Click += HelpButton_Click;
			Style quickStartButtonStyle = (Style)FindResource("HelpTopicButtonLayout");
			if (quickStartButtonStyle != null)
			{
				quickStartButton.Style = quickStartButtonStyle;
			}
			quickStartButton.HorizontalAlignment = HorizontalAlignment.Stretch;
			quickStartButton.IsEnabled = true;

			Grid.SetRow(quickStartButton, topicRowCount);
			Grid.SetColumn(quickStartButton, 0);

			TopicListGrid.Children.Add(quickStartButton);
			topicRowCount++;

			foreach (var feature in allFeatures)
			{
				var rowDefinition = new RowDefinition();
				rowDefinition.Height = GridLength.Auto;
				TopicListGrid.RowDefinitions.Add(rowDefinition);

				var button = new Button();
				button.Name = feature.Id;
				button.Content = feature.Title;
				button.HorizontalAlignment = HorizontalAlignment.Stretch;

				button.Click += HelpButton_Click;
				Style buttonStyle = (Style)FindResource("HelpTopicButtonLayout");
				if (buttonStyle != null)
				{
					button.Style = buttonStyle;
				}
				button.HorizontalAlignment = HorizontalAlignment.Stretch;

				button.IsEnabled = true;
				
				Grid.SetRow(button, topicRowCount);
				Grid.SetColumn(button, 0);

				TopicListGrid.Children.Add(button);
				topicRowCount++;
			}

			quickStartButton.PerformClick();
		}

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void HelpButton_Click(object sender, RoutedEventArgs e)
		{
			var clickedControlName = ((Button)e.Source).Name;
			var clickedFeature = clickedControlName == quickStart.Id ? quickStart : this.allFeatures.First(f => f.Id == clickedControlName);

			TopicTitle.Content = clickedFeature.Title;
			Style topicLabelStyle = (Style)FindResource("HeadlineLabelStyle");
			if (topicLabelStyle != null)
			{
				TopicTitle.Style = topicLabelStyle;
			}

			HelpLinesGrid.RowDefinitions.Clear();
			HelpLinesGrid.Children.Clear();

			int helpLineCount = 0;

			foreach (var helpLine in clickedFeature.HelpLines)
			{
				var rowDefinition = new RowDefinition();
				rowDefinition.Height = GridLength.Auto;
				HelpLinesGrid.RowDefinitions.Add(rowDefinition);

				var textBlock = new TextBlock();
				textBlock.HorizontalAlignment = HorizontalAlignment.Left;
				Style textBlockStyle = (Style)FindResource("HelpLineTextBlockStyle");
				if (textBlockStyle != null)
				{
					textBlock.Style = textBlockStyle;
				}

				textBlock.Text = helpLine.Content;
				Grid.SetRow(textBlock, helpLineCount);
				Grid.SetColumn(textBlock, 0);

				HelpLinesGrid.Children.Add(textBlock);
				helpLineCount++;
			}

			//this.Close();
		}
	}
}
