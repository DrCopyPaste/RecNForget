using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecNForget.Controls
{
	/// <summary>
	/// Interaction logic for CustomMessageBox.xaml
	/// 
	/// simple dialog that provides simple information, an icon (standard is application logo, or error icon, information icon, question icon)
	/// allows ok-ing
	/// OR
	/// ok-ing or cancelling
	/// </summary>
	public partial class CustomMessageBox : Window
	{
		public bool Ok { get; set; } = false;

		private CustomMessageBoxPromptValidation promptValidationMode = CustomMessageBoxPromptValidation.None;
		private System.Windows.Controls.TextBox promptTextBox = null;
		private CustomMessageBoxFocus controlFocus = CustomMessageBoxFocus.Default;

		public string PromptContent
		{
			get
			{
				return promptTextBox != null ? promptTextBox.Text : null;
			}
		}

		public CustomMessageBox(
			string caption,
			CustomMessageBoxIcon icon = CustomMessageBoxIcon.Default,
			CustomMessageBoxButtons buttons = CustomMessageBoxButtons.None,
			List<string> messageRows = null,
			string prompt = null,
			CustomMessageBoxPromptValidation promptValidationMode = CustomMessageBoxPromptValidation.None,
			CustomMessageBoxFocus controlFocus = CustomMessageBoxFocus.Default)
		{
			DataContext = this;
			InitializeComponent();

			int currentIndex = 0;

			this.Title = caption;
			this.promptValidationMode = promptValidationMode;
			this.controlFocus = controlFocus;

			var rowCaption = new RowDefinition();
			rowCaption.Height = GridLength.Auto;
			ContentGrid.RowDefinitions.Add(rowCaption);

			var captionTextBox = new System.Windows.Controls.TextBlock();

			var captionStyle = (Style)FindResource("VersionName_TextBox_Style");
			if (captionStyle != null)
			{
				captionTextBox.Style = captionStyle;
			}

			Grid.SetRow(captionTextBox, currentIndex);
			Grid.SetColumn(captionTextBox, 0);
			captionTextBox.Text = caption;
			ContentGrid.Children.Add(captionTextBox);
			currentIndex++;

			switch (icon)
			{
				case CustomMessageBoxIcon.Information:
				{
					var informationStyle = (Style)FindResource("CustomMessageBoxInformationIconStyle");
					if (informationStyle != null)
					{
						DialogImage.Style = informationStyle;
					}
					break;
				}
				case CustomMessageBoxIcon.Question:
				{
					var questionStyle = (Style)FindResource("CustomMessageBoxQuestionIconStyle");
					if (questionStyle != null)
					{
						DialogImage.Style = questionStyle;
					}
					break;
				}
				case CustomMessageBoxIcon.Error:
				{
					var errorStyle = (Style)FindResource("CustomMessageBoxErrorIconStyle");
					if (errorStyle != null)
					{
						DialogImage.Style = errorStyle;
					}
					break;
				}
				case CustomMessageBoxIcon.Default:
				{
					var defaultStyle = (Style)FindResource("CustomMessageBoxDefaultIconStyle");
					if (defaultStyle != null)
					{
						DialogImage.Style = defaultStyle;
					}
					break;
				}
			}

			var textblockStyle = (Style)FindResource("CustomMessageBox_TextBlock_Style");

			if (messageRows != null && messageRows.Any())
			{
				foreach (var messgeRow in messageRows)
				{
					var rowDefinition = new RowDefinition();
					rowDefinition.Height = GridLength.Auto;
					ContentGrid.RowDefinitions.Add(rowDefinition);

					var tempTextBox = new System.Windows.Controls.TextBlock();
					if (textblockStyle != null)
					{
						tempTextBox.Style = textblockStyle;
					}

					Grid.SetRow(tempTextBox, currentIndex);
					Grid.SetColumn(tempTextBox, 0);
					tempTextBox.Text = messgeRow;
					ContentGrid.Children.Add(tempTextBox);
					currentIndex++;
				}
			}

			if (prompt != null)
			{
				var promptRow = new RowDefinition();
				promptRow.Height = GridLength.Auto;
				ContentGrid.RowDefinitions.Add(promptRow);

				promptTextBox = new System.Windows.Controls.TextBox();
				var promptStyle = (Style)FindResource("MainWindowTextBoxStyle");
				if (promptStyle != null)
				{
					promptTextBox.Style = promptStyle;
				}
				promptTextBox.Name = "DialogPromptTextBox";
				promptTextBox.IsEnabled = true;
				promptTextBox.IsReadOnly = false;
				Grid.SetRow(promptTextBox, currentIndex);
				Grid.SetColumn(promptTextBox, 0);
				promptTextBox.TextChanged += promptTextBox_TextChanged;
				promptTextBox.Text = prompt;
				ContentGrid.Children.Add(promptTextBox);

				if (controlFocus == CustomMessageBoxFocus.Prompt)
				{
					promptTextBox.Focus();
					promptTextBox.Select(0, prompt.Length);
				}

				currentIndex++;
			}

			if (buttons != CustomMessageBoxButtons.None)
			{
				var tempButton = new ImageButton();
				var buttonStyle = (Style)FindResource("AcceptButton");
				if (buttonStyle != null)
				{
					tempButton.Style = buttonStyle;
				}
				tempButton.HorizontalAlignment = HorizontalAlignment.Right;
				tempButton.Click += okButton_Click;
				tempButton.IsDefault = true;
				Grid.SetRow(tempButton, 3);
				Grid.SetColumn(tempButton, 1);
				DialogGrid.Children.Add(tempButton);

				if (controlFocus == CustomMessageBoxFocus.Ok)
				{
					tempButton.Focus();
				}

				if (buttons == CustomMessageBoxButtons.OkAndCancel)
				{
					var cancelButton = new ImageButton();
					var cancelButtonStyle = (Style)FindResource("CancelButton");
					if (cancelButtonStyle != null)
					{
						cancelButton.Style = cancelButtonStyle;
					}
					cancelButton.HorizontalAlignment = HorizontalAlignment.Left;
					cancelButton.Click += cancelButton_Click;
					Grid.SetRow(cancelButton, 3);
					Grid.SetColumn(cancelButton, 0);
					DialogGrid.Children.Add(cancelButton);

					if (controlFocus == CustomMessageBoxFocus.Cancel)
					{
						cancelButton.Focus();
					}
				}

				currentIndex++;
			}
		}

		private void promptTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (promptTextBox != null && promptValidationMode == CustomMessageBoxPromptValidation.EraseIllegalPathCharacters)
			{
				string sanitizedString = string.Concat(promptTextBox.Text.Split(Path.GetInvalidFileNameChars()));

				if (sanitizedString.Length != promptTextBox.Text.Length)
				{
					promptTextBox.Text = string.Concat(promptTextBox.Text.Split(Path.GetInvalidFileNameChars()));
					promptTextBox.CaretIndex = promptTextBox.Text.Length;
				}
			}
		}

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			Ok = true;

			this.DialogResult = true;
			this.Close();
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			Ok = false;

			// prefer this to .isCancel, because isCancel loses the ability to just press ESC to close a dialog
			this.DialogResult = false;
			this.Close();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
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
;