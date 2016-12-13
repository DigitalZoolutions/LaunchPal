using System.Globalization;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.UWP.Helper;
using LaunchPal.Manager;
using Microsoft.Toolkit.Uwp.Notifications;

[assembly: Xamarin.Forms.Dependency(typeof(LiveTileImplementation))]
namespace LaunchPal.UWP.Helper
{
    class LiveTileImplementation : ICreateTile
    {
        public void SetLaunch()
        {
            var tileNotification = CreateLiveTile(LaunchPal.App.Settings.TrackedLaunchOnHomescreen);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        private TileNotification CreateLiveTile(SimpleLaunchData launchData)
        {
            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = new TileBinding
                    {
                        Branding = TileBranding.None,
                        Content = new TileBindingContentAdaptive
                        {
                            PeekImage = new TilePeekImage()
                            {
                                Source = "Assets/Square150x150Logo.scale-200.png"
                            },
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "Next Launch:",
                                    HintWrap = true,
                                    HintStyle = AdaptiveTextStyle.Caption
                                },
                                new AdaptiveText(),
                                new AdaptiveText()
                                {
                                    Text = launchData.LaunchNet,
                                    HintWrap = true,
                                    HintStyle = AdaptiveTextStyle.Caption
                                }
                            }
                        }
                    },

                    TileWide = new TileBinding()
                    {
                        Branding = TileBranding.NameAndLogo,
                        DisplayName = "LaunchPal",
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = launchData.Name,
                                    HintWrap = false,
                                    HintStyle = AdaptiveTextStyle.Body
                                },

                                new AdaptiveText()
                                {
                                    Text = launchData.LaunchNet,
                                    HintWrap = false,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                                },

                                new AdaptiveText()
                                {
                                    Text = "Status: " + launchData.Status,
                                    HintWrap = false,
                                    HintStyle = AdaptiveTextStyle.Body
                                },                                
                            }
                        }
                    },

                    TileLarge = new TileBinding
                    {
                        Branding = TileBranding.NameAndLogo,
                        DisplayName = "LaunchPal",
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = launchData.Name,
                                    HintWrap = false,
                                    HintStyle = AdaptiveTextStyle.Body
                                },

                                new AdaptiveText()
                                {
                                    Text = launchData.LaunchNet,
                                    HintWrap = false,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                                },

                                new AdaptiveText()
                                {
                                    Text = "Status - " + launchData.Status,
                                    HintWrap = false,
                                    HintStyle = AdaptiveTextStyle.Body
                                },

                                new AdaptiveText(),

                                new AdaptiveText
                                {
                                    Text = "Mission Description",
                                    HintWrap = false,
                                    HintStyle = AdaptiveTextStyle.Body
                                },

                                new AdaptiveText()
                                {
                                    Text = launchData.Description,
                                    HintWrap = true,
                                    HintStyle = AdaptiveTextStyle.Caption
                                },
                            }
                        }
                    }

                }
            };

            return new TileNotification(content.GetXml())
            {
                ExpirationTime = launchData.Net.AddMinutes(30)
            };
        }
    }
}
