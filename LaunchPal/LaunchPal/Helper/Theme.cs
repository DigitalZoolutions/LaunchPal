using Xamarin.Forms;

namespace LaunchPal.Helper
{
    public static class Theme
    {
        private static AppTheme _currentTheme;
        private static bool _useLightIcons;
        private static Color _backgroundColor;
        private static Color _navBackgroundColor;
        private static Color _textColor;
        private static Color _headerColor;
        private static Color _linkColor;
        private static Color _frameColor;
        private static Color _frameBorderColor;
        private static Color _buttonBackgroundColor;
        private static Color _buttonTextColor;

        public enum AppTheme
        {
            Light,
            Dark,
            Night,
            Contrast
        }

        internal static bool UseLightIcons => _useLightIcons;
        internal static Color BackgroundColor => _backgroundColor;
        internal static Color NavBackgroundColor => _navBackgroundColor;
        internal static Color TextColor => _textColor;
        internal static Color HeaderColor => _headerColor;
        internal static Color LinkColor => _linkColor;
        internal static Color FrameColor => _frameColor;
        internal static Color FrameBorderColor => _frameBorderColor;
        internal static Color ButtonBackgroundColor => _buttonBackgroundColor;
        internal static Color ButtonTextColor => _buttonTextColor;


        internal static void SetTheme(AppTheme theme)
        {
            switch (theme)
            {
                case AppTheme.Light:
                    SetLightTheme();
                    break;
                case AppTheme.Dark:
                    SetDarkTheme();
                    break;
                case AppTheme.Night:
                    SetNightTheme();
                    break;
                case AppTheme.Contrast:
                    SetContrastTheme();
                    break;
                default:
                    SetLightTheme();
                    break;
            }
        }

        internal static void SetTheme(int value)
        {
            var theme = (AppTheme)value;
            SetTheme(theme);
        }

        internal static AppTheme GetCurrentTheme()
        {
            return _currentTheme;
        }

        internal static int GetThemeIntValue(AppTheme theme)
        {
            return (int) theme;
        }

        internal static int GetCurrentThemeIntValue()
        {
            return (int)_currentTheme;
        }

        private static void SetLightTheme()
        {
            _currentTheme = AppTheme.Light;
            _useLightIcons = false;
            _backgroundColor = Color.FromHex("EEFCED");
            _navBackgroundColor = Color.FromHex("C0D1CF");
            _textColor = Color.FromHex("686963");
            _headerColor = Color.FromHex("809DB5");
            _linkColor = Color.FromHex("DB5461");
            _frameColor = Color.FromHex("C0D1CF");
            _frameBorderColor = Color.FromHex("809DB5");
            _buttonBackgroundColor = Color.FromHex("C0D1CF");
            _buttonTextColor = Color.FromHex("DB5461");
        }

        private static void SetDarkTheme()
        {
            _currentTheme = AppTheme.Dark;
            _useLightIcons = true;
            _backgroundColor = Color.FromHex("21252B");
            _navBackgroundColor = Color.FromHex("282C34");
            _textColor = Color.FromHex("9EA6B5");
            _headerColor = Color.FromHex("BFC8D7");
            _linkColor = Color.FromHex("548AF7");
            _frameColor = Color.FromHex("282C34");
            _frameBorderColor = Color.FromHex("9EA6B5");
            _buttonBackgroundColor = Color.FromHex("282C34");
            _buttonTextColor = Color.FromHex("548AF7");
        }

        private static void SetNightTheme()
        {
            _currentTheme = AppTheme.Night;
            _useLightIcons = false;
            _backgroundColor = Color.FromHex("2A0000");
            _navBackgroundColor = Color.FromHex("360000");
            _textColor = Color.FromHex("730000");
            _headerColor = Color.FromHex("A80000");
            _linkColor = Color.FromHex("6B2B2B");
            _frameColor = Color.FromHex("460303");
            _frameBorderColor = Color.FromHex("A80000");
            _buttonBackgroundColor = Color.FromHex("460303");
            _buttonTextColor = Color.FromHex("6B2B2B");
        }

        private static void SetContrastTheme()
        {
            _currentTheme = AppTheme.Contrast;
            _useLightIcons = true;
            _backgroundColor = Color.Black;
            _navBackgroundColor = Color.Black;
            _textColor = Color.Yellow;
            _headerColor = Color.Lime;
            _linkColor = Color.Red;
            _frameColor = Color.Black;
            _frameBorderColor = Color.Silver;
            _buttonBackgroundColor = Color.Yellow;
            _buttonTextColor = Color.Red;
        }
    }
}
