﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using NAudio.Wave;
using Notifications.Wpf.Core;
using RecNForget.Controls.Extensions;
using RecNForget.Controls.IoC;
using RecNForget.Services.Contracts;
using RecNForget.Services.Designer;
using RecNForget.WPF.Services.Contracts;
using Unity;

namespace RecNForget.Controls
{
    /// <summary>
    /// Interaction logic for SelectedFileControl.xaml
    /// </summary>
    public partial class SelectedFileControl : UserControl, INotifyPropertyChanged
    {
        private readonly NotificationManager _notificationManager = new NotificationManager();
        private readonly IActionService actionService = null;
        private readonly IAppSettingService appSettingService = null;
        private readonly IAudioPlaybackService audioPlaybackService = null;

        private ISelectedFileService selectedFileService = null;

        public SelectedFileControl()
        {
            DataContext = this;
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.actionService = new DesignerActionService();
                this.appSettingService = new DesignerAppSettingService();
                this.audioPlaybackService = new DesignerAudioPlaybackService();

                this.SelectedFileService = new DesignerSelectedFileService();
            }
            else
            {
                // ToDo: Evil Hack to have the cake (see actual design in design mode) and eat it too (have different styles at runtime)
                this.Resources = null;
                this.actionService = UnityHandler.UnityContainer.Resolve<IActionService>();
                this.appSettingService = UnityHandler.UnityContainer.Resolve<IAppSettingService>();
                this.audioPlaybackService = UnityHandler.UnityContainer.Resolve<IAudioPlaybackService>();

                this.SelectedFileService = UnityHandler.UnityContainer.Resolve<ISelectedFileService>();

                SelectedFileService.PropertyChanged += SelectedFileService_PropertyChanged;
            }
        }

        ~SelectedFileControl()
        {
            SelectedFileService.PropertyChanged -= SelectedFileService_PropertyChanged;
        }

        public ISelectedFileService SelectedFileService
        {
            get
            {
                return selectedFileService;
            }

            set
            {
                selectedFileService = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SelectedFileService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedFileService.HasSelectedFile) && SelectedFileService.HasSelectedFile)
            {
                try
                {
                    var audioFileLengthString = (new AudioFileReader(SelectedFileService.SelectedFile.FullName)).TotalTime.ToString("g") + " s";
                    var fileSizeString = (SelectedFileService.SelectedFile.Length / (double)1024).ToString("N2") + " kB";

                    FileInfoLabel.Content = audioFileLengthString + " (" + fileSizeString + ")";
                }
                catch (Exception ex)
                {
                    FileInfoLabel.Content = "error trying to read file size";

                    _notificationManager.ShowAsync(
                        content: new NotificationContent()
                        {
                            Title = "Error trying to read file size",
                            Message = "an error occurred while trying to parse audio file: " + ex.Message,
                            Type = NotificationType.Error
                        },
                        expirationTime: TimeSpan.FromSeconds(10));
                }
            }
        }

        private void ExportSelectedFileNameButton_Clicked(object sender, RoutedEventArgs e)
        {
            actionService.ExportSelectedFile();
        }

        private void ChangeSelectedFileNameButton_Clicked(object sender, RoutedEventArgs e)
        {
            actionService.ChangeSelectedFileName();
        }

        private void DeleteSelectedFileButton_Clicked(object sender, RoutedEventArgs e)
        {
            actionService.DeleteSelectedFile();
        }
    }
}
