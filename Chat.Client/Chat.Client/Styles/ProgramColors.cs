using System.Windows;
using System.Windows.Media;

namespace Chat.Client.Styles
{
    public static class ProgramColors
    {
        public static SolidColorBrush AccentLightColor3 { get; set; } = (SolidColorBrush)Application.Current.Resources[nameof(AccentLightColor3)];
        public static SolidColorBrush AccentLightColor2 { get; set; } = (SolidColorBrush)Application.Current.Resources[nameof(AccentLightColor2)];
        public static SolidColorBrush AccentLightColor1 { get; set; } = (SolidColorBrush) Application.Current.Resources[nameof(AccentLightColor1)];
        public static SolidColorBrush MainColor { get; set; } = (SolidColorBrush) Application.Current.Resources[nameof(MainColor)];
        public static SolidColorBrush AccentDarkColor1 { get; set; } = (SolidColorBrush) Application.Current.Resources[nameof(AccentDarkColor1)];
     }
}
