using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LaunchPal.Helper
{
    public static class Theme
    {
        public enum AppTheme
        {
            Light,
            Dark,
            Night,
            Contrast
        }

        internal static bool UseLightIcons { get; set; }
        internal static Color BackgroundColor { get; set; }
        internal static Color NavBackgroundColor { get; set; }
        internal static Color TextColor { get; set; }
        internal static Color HeaderColor { get; set; }
        internal static Color LinkColor { get; set; }
        internal static Color FrameColor { get; set; }
        internal static Color FrameBorderColor { get; set; }
        internal static Color ButtonBackgroundColor { get; set; }


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

        internal static int GetThemeIntValue(AppTheme theme)
        {
            return (int) theme;
        }

        private static void SetLightTheme()
        {
            UseLightIcons = false;
            BackgroundColor = Color.White;
            NavBackgroundColor = Color.Silver;
            TextColor = Color.Black;
            HeaderColor = Color.Gray;
            LinkColor = Color.Blue;
            FrameColor = Color.Silver;
            FrameBorderColor = Color.Silver;
            ButtonBackgroundColor = Color.Gray;
        }

        private static void SetDarkTheme()
        {
            UseLightIcons = true;
            BackgroundColor = Color.Black;
            NavBackgroundColor = Color.FromHex("333333");
            TextColor = Color.White;
            HeaderColor = Color.Silver;
            LinkColor = Color.Blue;
            FrameColor = Color.FromHex("333333");
            FrameBorderColor = Color.Silver;
            ButtonBackgroundColor = Color.Silver;
        }

        private static void SetNightTheme()
        {
            UseLightIcons = false;
            BackgroundColor = Color.Black;
            NavBackgroundColor = Color.Maroon;
            TextColor = Color.Red;
            HeaderColor = Color.Red;
            LinkColor = Color.Fuchsia;
            FrameColor = Color.Maroon;
            FrameBorderColor = Color.Red;
            ButtonBackgroundColor = Color.Maroon;
        }

        private static void SetContrastTheme()
        {
            UseLightIcons = true;
            BackgroundColor = Color.Black;
            NavBackgroundColor = Color.Black;
            TextColor = Color.Yellow;
            HeaderColor = Color.Lime;
            LinkColor = Color.Red;
            FrameColor = Color.Black;
            FrameBorderColor = Color.Silver;
            ButtonBackgroundColor = Color.Yellow;
        }
    }
}
