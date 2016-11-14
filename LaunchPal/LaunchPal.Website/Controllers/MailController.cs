using System.Web.Http;
using Launchpal.Website.Models;

namespace Launchpal.Website.Controllers
{
    public class MailController : ApiController
    {
        // POST api/<controller>
        public void Post([FromBody]MailModel newMail)
        {
            
        }
    }
}