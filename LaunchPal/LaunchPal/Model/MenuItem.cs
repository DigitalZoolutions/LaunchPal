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
                TargetType = typeof(OverviewPage)
            });

            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "Launch.png", "Assets/Menu/Launch.png") :
                    Device.OnPlatform("", "LaunchBlack.png", "Assets/Menu/LaunchBlack.png")
                    ),
                Name = "Launch",
                TargetType = typeof(LaunchPage)
            });

            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "SpaceNews.png", "Assets/Menu/SpaceNews.png") :
                    Device.OnPlatform("", "SpaceNewsBlack.png", "Assets/Menu/SpaceNewsBlack.png")
                    ),
                Name = "Space News",
                TargetType = typeof(NewsPage)
            });

            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "PeopleInSpace.png", "Assets/Menu/PeopleInSpace.png") :
                    Device.OnPlatform("", "PeopleInSpaceBlack.png", "Assets/Menu/PeopleInSpaceBlack.png")
                    ),
                Name = "Astronauts",
                TargetType = typeof(AstronautsPage)
            });

            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "Search.png", "Assets/Menu/Search.png") :
                    Device.OnPlatform("", "SearchBlack.png", "Assets/Menu/SearchBlack.png")
                    ),
                Name = "Search",
                TargetType = typeof(SearchPage)
            });

            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "Support.png", "Assets/Menu/Support.png") :
                    Device.OnPlatform("", "SupportBlack.png", "Assets/Menu/SupportBlack.png")
                    ),
                Name = "Feedback",
                TargetType = typeof(FeedbackPage)
            });

            this.Add(new MenuItem
            {
                IconSource = ImageSource.FromFile(
                    Theme.UseLightIcons ?
                    Device.OnPlatform("", "Settings.png", "Assets/Menu/Settings.png") :
                    Device.OnPlatform("", "SettingsBlack.png", "Assets/Menu/SettingsBlack.png")
                    ),
                Name = "Settings",
                TargetType = typeof(View.SettingsPage)
            });
        }
    }
}
