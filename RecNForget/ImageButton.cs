﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RecNForget
{
	// https://stackoverflow.com/a/18482034
	public class ImageButton : Button
	{
		static ImageButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton),
				new FrameworkPropertyMetadata(typeof(ImageButton)));
		}

		public ImageSource DefaultImage
		{
			get { return (ImageSource)GetValue(DefaultImageProperty); }
			set { SetValue(DefaultImageProperty, value); }
		}

		public ImageSource DisabledImage
		{
			get { return (ImageSource)GetValue(DisabledImageProperty); }
			set { SetValue(DisabledImageProperty, value); }
		}

		public ImageSource SelectedImage
		{
			get { return (ImageSource)GetValue(SelectedImageProperty); }
			set { SetValue(SelectedImageProperty, value); }
		}

		public ImageSource PressedImage
		{
			get { return (ImageSource)GetValue(PressedImageProperty); }
			set { SetValue(PressedImageProperty, value); }
		}

		public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register("DefaultImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));
		public static readonly DependencyProperty DisabledImageProperty = DependencyProperty.Register("DisabledImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));
		public static readonly DependencyProperty SelectedImageProperty = DependencyProperty.Register("SelectedImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));
		public static readonly DependencyProperty PressedImageProperty = DependencyProperty.Register("PressedImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));

	}
}