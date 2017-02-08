using System;
using System.Collections.Generic;
using LaunchPal.ExternalApi.PeopleInSpace.JsonObject;
using LaunchPal.Manager;
using LaunchPal.Template;
using Xamarin.Forms;

namespace LaunchPal.ViewModel
{
    class AstronautsViewModel : ErrorViewModel
    {
        public int NumberOfAstronautsInSpace { get; set; }
        public ListView Astronouts { get; set; }

        public AstronautsViewModel()
        {
            List<Person> astronouts = new List<Person>();

            try
            {
                astronouts = CacheManager.TryGetAstronautsInSpace().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SetError(ex);
                return;
            }

            NumberOfAstronautsInSpace = astronouts.Count;
            Astronouts = new AstronoutListTemplate(astronouts);
        }
    }
}
