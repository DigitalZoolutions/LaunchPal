using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using Launchpal.Website.IAP;

namespace Launchpal.Website.Controllers
{
    public class IAPController : ApiController
    {
        // GET: api/IAP
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/IAP/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/IAP
        public HttpResponseMessage Post([FromBody]string xmlString)
        {
            XmlDocument xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.LoadXml(xmlString); // suppose that myXmlString contains "<Names>...</Names>"
            }
            catch (XmlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Not valid Xml: " + ex);
            }

            return ReceiptVerification.VerifyReceiptIsValid(xmlDocument)                                            // If receipt validates
                ? Request.CreateResponse(HttpStatusCode.OK, "Receipt was validated.")                               // return OK
                : Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Receipt could not be validated.");     // else fail
        }

        // PUT: api/IAP/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE: api/IAP/5
        public void Delete(int id)
        {
        }
    }
}
