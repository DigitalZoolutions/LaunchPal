using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Helper;
using LaunchPal.View;
using Xamarin.Forms;

namespace LaunchPal.Model
{
    class MenuItem
    {
        public ImageSource IconSource { get; set; }
        public string Name { get; set; }
        public Type TargetType { get; set; }
    }

    internal class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "Overview.png", "Assets/Menu/Overview.png") :
                    Device.OnPlatform("", "OverviewBlack.png", "Assets/Menu/OverviewBlack.png")
                    ),
                Name = "Overview",
                TargetType = typeof(Overview)
            });

            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "Launch.png", "Assets/Menu/Launch.png") :
                    Device.OnPlatform("", "LaunchBlack.png", "Assets/Menu/LaunchBlack.png")
                    ),
                Name = "Launch",
                TargetType = typeof(Launch)
            });

            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "Search.png", "Assets/Menu/Search.png") :
                    Device.OnPlatform("", "SearchBlack.png", "Assets/Menu/SearchBlack.png")
                    ),
                Name = "Search",
                TargetType = typeof(Search)
            });

            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "Support.png", "Assets/Menu/Support.png") :
                    Device.OnPlatform("", "SupportBlack.png", "Assets/Menu/SupportBlack.png")
                    ),
                Name = "Feedback",
                TargetType = typeof(Feedback)
            });

            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "Settings.png", "Assets/Menu/Settings.png") :
                    Device.OnPlatform("", "SettingsBlack.png", "Assets/Menu/SettingsBlack.png")
                    ),
                Name = "Settings",
                TargetType = typeof(View.Settings)
            });
        }
    }
}
