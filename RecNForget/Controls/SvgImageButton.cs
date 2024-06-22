using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RecNForget.Controls
{
    // https://stackoverflow.com/a/18482034
    public class SvgImageButton1 : Button
    {
        static SvgImageButton1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SvgImageButton1),
                new FrameworkPropertyMetadata(typeof(SvgImageButton1)));
        }

        public static readonly DependencyProperty ImagePaddingProperty = DependencyProperty.Register("ImagePadding", typeof(string), typeof(SvgImageButton1), new PropertyMetadata("0,0,0,0"));
        public string ImagePadding
        {
            get { return (string)GetValue(ImagePaddingProperty); }
            set { SetValue(ImagePaddingProperty, value); }
        }

        public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register("ImageMargin", typeof(string), typeof(SvgImageButton1), new PropertyMetadata("0,0,0,0"));
        public string ImageMargin
        {
            get { return (string)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
        }

        public static readonly DependencyProperty IsEnabledCondition1Property = DependencyProperty.Register("IsEnabledCondition1", typeof(bool), typeof(SvgImageButton1), new PropertyMetadata(true));
        public bool IsEnabledCondition1
        {
            get { return (bool)GetValue(IsEnabledCondition1Property); }
            set { SetValue(IsEnabledCondition1Property, value); }
        }

        public static readonly DependencyProperty IsEnabledCondition2Property = DependencyProperty.Register("IsEnabledCondition2", typeof(bool), typeof(SvgImageButton1), new PropertyMetadata(true));
        public bool IsEnabledCondition2
        {
            get { return (bool)GetValue(IsEnabledCondition1Property); }
            set { SetValue(IsEnabledCondition1Property, value); }
        }

        public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register("DefaultImage", typeof(string), typeof(SvgImageButton1), new PropertyMetadata(default(string)));
        public string DefaultImage
        {
            get { return (string)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        public static readonly DependencyProperty DisabledImageProperty = DependencyProperty.Register("DisabledImage", typeof(string), typeof(SvgImageButton1), new PropertyMetadata(default(string)));
        public string DisabledImage
        {
            get { return (string)GetValue(DisabledImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }

        public static readonly DependencyProperty SelectedImageProperty = DependencyProperty.Register("SelectedImage", typeof(string), typeof(SvgImageButton1), new PropertyMetadata(default(string)));
        public string SelectedImage
        {
            get { return (string)GetValue(SelectedImageProperty); }
            set { SetValue(SelectedImageProperty, value); }
        }

        public static readonly DependencyProperty PressedImageProperty = DependencyProperty.Register("PressedImage", typeof(string), typeof(SvgImageButton1), new PropertyMetadata(default(string)));
        public string PressedImage
        {
            get { return (string)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }
    }
}
