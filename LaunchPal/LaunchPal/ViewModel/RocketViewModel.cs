using System;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Manager;
using Xamarin.Forms;

namespace LaunchPal.ViewModel
{
    internal class RocketViewModel : ErrorViewModel
    {
        public string RocketName { get; set; }
        public string RocketConfiguration { get; set; }
        public string RocketFamilyName { get; set; }
        public Image RocketImage { get; set; }

        public RocketViewModel(int rocketId)
        {
            try
            {
                var rocket = CacheManager.TryGetRocketByRocketId(rocketId).GetAwaiter().GetResult();

                PopulateViewModel(rocket);
            }
            catch (Exception ex)
            {
                this.SetError(ex);
            }
        }

        private void PopulateViewModel(Rocket rocket)
        {
            RocketName = rocket.Name;
            RocketConfiguration = rocket.Configuration;
            RocketFamilyName = rocket.Family.Name;

            var image = CacheManager.TryGetImageFromUriAndCache(rocket.ImageUrl);
            image.Opacity = 0.85;
            if (!rocket.ImageUrl.Contains("placeholder"))
            {
                RocketImage = image;
            }
        }
    }
}
