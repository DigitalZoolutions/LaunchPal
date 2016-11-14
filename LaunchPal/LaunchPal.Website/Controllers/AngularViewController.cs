using System.Web.Mvc;

namespace Launchpal.Website.Controllers
{
    public class AngularViewController : Controller
    {
        // GET: AngularView
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }
    }
}