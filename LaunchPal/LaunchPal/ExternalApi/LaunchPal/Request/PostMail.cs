using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchPal.JsonObject;

namespace LaunchPal.ExternalApi.LaunchPal.Request
{
    internal class PostMail
    {
        internal static async Task<bool> SendNewMail(Mail newMail)
        {
            const string apiUrl = "Mail/Post";
            return await HttpCaller.PostLaunchpalApiAsync<bool>(apiUrl + newMail);
        }
    }
}
