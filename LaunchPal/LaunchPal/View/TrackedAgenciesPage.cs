using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Enums;
using LaunchPal.Helper;
using LaunchPal.Manager;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class TrackedAgenciesPage : ContentPage
    {
        public TrackedAgenciesPage()
        {
            Title = "Track Agencies";
            BackgroundColor = Theme.BackgroundColor;

            Content = new ScrollView
            {
                Padding = new Thickness(10),
                Content = new StackLayout
                {
                    Padding = new Thickness(15),
                    Children =
                    {
                        GenerateAgencyLabel(AgencyType.Nasa.ToFriendlyString()),
                        GenerateSwitch(AgencyType.Nasa),
                        GenerateAgencyLabel(AgencyType.Esa.ToFriendlyString()),
                        GenerateSwitch(AgencyType.Esa),
                        GenerateAgencyLabel(AgencyType.Roscosmos.ToFriendlyString()),
                        GenerateSwitch(AgencyType.Roscosmos),
                        GenerateAgencyLabel(AgencyType.ChinaSpaceAdministration.ToFriendlyString()),
                        GenerateSwitch(AgencyType.ChinaSpaceAdministration),
                        GenerateAgencyLabel(AgencyType.IndianSpaceOrganization.ToFriendlyString()),
                        GenerateSwitch(AgencyType.IndianSpaceOrganization),
                        GenerateAgencyLabel(AgencyType.SpaceX.ToFriendlyString()),
                        GenerateSwitch(AgencyType.SpaceX),
                        GenerateAgencyLabel(AgencyType.UnitedLaunchAlliance.ToFriendlyString()),
                        GenerateSwitch(AgencyType.UnitedLaunchAlliance),
                        GenerateAgencyLabel(AgencyType.OrbitalATK.ToFriendlyString()),
                        GenerateSwitch(AgencyType.OrbitalATK),
                        GenerateAgencyLabel(AgencyType.BlueOrigin.ToFriendlyString()),
                        GenerateSwitch(AgencyType.BlueOrigin),
                    }
                }

            };
        }

        private Label GenerateAgencyLabel(string agencyName)
        {
            return new Label
            {
                Text = $"Track {agencyName}'s Launches",
                TextColor = Theme.HeaderColor,
                FontSize = 22
            };
        }

        private Switch GenerateSwitch(AgencyType type)
        {
            var Switch = new Switch
            {
                IsToggled = TrackingManager.IsAgencyBeingTracked(type),
                Margin = new Thickness(0, 0, 0, 10)
            };

            Switch.Toggled += (sender, args) =>
            {
                if (sender.GetType() != typeof(Switch))
                    return;

                if (((Switch)sender).IsToggled)
                {
                    TrackingManager.AddTrackedAgency(type);
                }
                else
                {
                    TrackingManager.RemoveTrackedAgency(type);
                }
            };

            return Switch;
        }
    }
}
