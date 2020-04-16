using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RecNForget.Controls
{
    // https://stackoverflow.com/a/18482034
    public class ImageButton : Button
    {
        public static readonly DependencyProperty ImagePaddingProperty = DependencyProperty.Register("ImagePadding", typeof(string), typeof(ImageButton), new PropertyMetadata("0,0,0,0"));
        public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register("ImageMargin", typeof(string), typeof(ImageButton), new PropertyMetadata("0,0,0,0"));

        public static readonly DependencyProperty IsEnabledCondition1Property = DependencyProperty.Register("IsEnabledCondition1", typeof(bool), typeof(ImageButton), new PropertyMetadata(true));
        public static readonly DependencyProperty IsEnabledCondition2Property = DependencyProperty.Register("IsEnabledCondition2", typeof(bool), typeof(ImageButton), new PropertyMetadata(true));

        public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register("DefaultImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));
        public static readonly DependencyProperty DisabledImageProperty = DependencyProperty.Register("DisabledImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));
        public static readonly DependencyProperty SelectedImageProperty = DependencyProperty.Register("SelectedImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));
        public static readonly DependencyProperty PressedImageProperty = DependencyProperty.Register("PressedImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ImageButton),
                new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public string ImagePadding
        {
            get { return (string)GetValue(ImagePaddingProperty); }
            set { SetValue(ImagePaddingProperty, value); }
        }

        public string ImageMargin
        {
            get { return (string)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
        }

        public bool IsEnabledCondition1
        {
            get { return (bool)GetValue(IsEnabledCondition1Property); }
            set { SetValue(IsEnabledCondition1Property, value); }
        }

        public bool IsEnabledCondition2
        {
            get { return (bool)GetValue(IsEnabledCondition1Property); }
            set { SetValue(IsEnabledCondition1Property, value); }
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
    }
}
