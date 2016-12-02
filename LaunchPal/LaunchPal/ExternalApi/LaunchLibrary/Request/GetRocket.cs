using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace LaunchPal.ExternalApi.LaunchLibrary.Request
{
    class GetRocket
    {
        private const string Mode = "?mode=verbose";

        internal static async Task<Rocket> ById(int rocketId)
        {
            const string apiUrl = "rocket/";
            var result = await HttpCaller.CallLaunchLibraryApi<SearchRocket>(apiUrl + rocketId + Mode);

            return result.rockets[0] ?? new Rocket();
        }
    }
}
