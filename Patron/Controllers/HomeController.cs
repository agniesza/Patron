using System.Web.Mvc;

namespace Patron.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string phrase)
        {
            var patrons = db.Patrons.
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}