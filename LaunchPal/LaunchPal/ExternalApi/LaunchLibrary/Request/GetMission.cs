using System.Linq;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace LaunchPal.ExternalApi.LaunchLibrary.Request
{
    internal static class GetMission
    {
        private const string Mode = "?mode=verbose";

        internal static async Task<Mission> NextMissionById(int id)
        {
            const string apiUrl = "mission/";
            var missionList = await HttpCaller.CallLaunchLibraryApi<MissionList>(apiUrl + id + Mode);

            return missionList.Missions.FirstOrDefault(x => x.Id == id) ?? new Mission();
        }

        internal static async Task<Mission> NextMissionByLaunchId(int id)
        {
            const string apiUrl = "mission?launchid=";

            var missionList = await HttpCaller.CallLaunchLibraryApi<MissionList>(apiUrl + id + "&mode=verbose");
            var result = missionList.Missions.FirstOrDefault(x => x.Launch?.Id == id) ?? new Mission();
            if (result.Id == 0 && missionList.Missions.Length > 0)
            {
                return missionList.Missions[0];
            }
            return result;
        }
    }
}
