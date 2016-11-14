using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.ExternalApi.LaunchPal.JsonObject;

namespace LaunchPal.ExternalApi.LaunchPal.Request
{
    internal class PostMail
    {
        internal static async Task<bool> SendNewMail(Mail newMail)
        {
            const string apiUrl = "Mail/Post";
            return await HttpCaller.PostLaunchpalApi<bool>(apiUrl + newMail);
        }
    }
}
