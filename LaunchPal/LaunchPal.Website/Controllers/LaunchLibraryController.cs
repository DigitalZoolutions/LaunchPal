using System.Threading.Tasks;
using System.Web.Http;
using LaunchPal.ExternalApi;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace Launchpal.Website.Controllers
{
    public class LaunchLibraryController : ApiController
    {
        // GET: api/LaunchLibrary
        public async Task<Launch> Get()
        {
            var launch = await ApiManager.NextLaunch();
            return launch;
        }

        // GET: api/LaunchLibrary/5
        public async Task<Launch> Get(int id)
        {
            return await ApiManager.NextLaunchById(id);
        }

        // POST: api/LaunchLibrary
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LaunchLibrary/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LaunchLibrary/5
        public void Delete(int id)
        {
        }
    }
}
