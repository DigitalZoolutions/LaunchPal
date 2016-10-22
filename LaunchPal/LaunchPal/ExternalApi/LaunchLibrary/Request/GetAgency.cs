using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace LaunchPal.ExternalApi.LaunchLibrary.Request
{
    class GetAgency
    {
        private const string Mode = "?mode=verbose";

        internal static async Task<Agency> NextMissionById(int id)
        {
            const string apiUrl = "agency/";
            var agency = await HttpCaller.CallLaunchLibraryApi<Agency>(apiUrl + id + Mode);

            return agency;
        }

        
    }
}
