using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chat.Client.Controls
{
    /// <summary>
    /// Interaction logic for StandardView.xaml
    /// </summary>
    public partial class StandardView : UserControl
    {
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            "BackgroundColor", typeof(SolidColorBrush), typeof(StandardView),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255))));

        public static readonly DependencyProperty ViewContentProperty = DependencyProperty.Register(
            "ViewContent", typeof(object), typeof(StandardView), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(int), typeof(StandardView), new PropertyMetadata(15));

        public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(
            "ContentPadding", typeof(int), typeof(StandardView), new PropertyMetadata(5));

        public int ContentPadding
        {
            get { return (int) GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }

        public int CornerRadius
        {
            get { return (int) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public object ViewContent
        {
            get { return (object) GetValue(ViewContentProperty); }
            set { SetValue(ViewContentProperty, value); }
        }

        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush) GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public StandardView()
        {
            InitializeComponent();
        }
    }
}
