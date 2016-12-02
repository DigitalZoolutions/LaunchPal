using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.PeopleInSpace.JsonObject;

namespace LaunchPal.ExternalApi.PeopleInSpace.Request
{
    class GetPeopleInSpace
    {
        internal static async Task<JsonObject.PeopleInSpace> GetAll()
        {
            const string apiUrl = "peopleinspace.json";
            var peopleInSpace = await HttpCaller.CallLaunchLibraryApi<JsonObject.PeopleInSpace>(apiUrl);

            return peopleInSpace ?? new JsonObject.PeopleInSpace {Number = 0, People = new List<Person>()};
        }
    }
}
