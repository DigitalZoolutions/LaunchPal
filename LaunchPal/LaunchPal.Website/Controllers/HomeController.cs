using System.Threading.Tasks;
using System.Web.Mvc;
using LaunchPal.ExternalApi;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace Launchpal.Website.Controllers
{
    public class HomeController : AsyncController
    {
        public async Task<ActionResult> Index()
        {
            Launch model = await ApiManager.NextLaunch();

            return View(model);
        }

        public FileResult Android()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"~/AppInstalationFiles/LaunchPal.Droid-Signed.apk");
            string fileName = "LaunchPal.Droid-Signed.apk";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

    }
}