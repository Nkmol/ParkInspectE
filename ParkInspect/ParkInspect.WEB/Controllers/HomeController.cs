using System.IO;
using System.Web.Mvc;

namespace ParkInspect.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var path = Server.MapPath("~/App_LocalResources");
            var x = Directory.GetFiles(path);

            for (var i = 0; i < x.Length; i++)
                x[i] = x[i].Replace('\\', '/');

            ViewBag.pdf = x;

            return View();
        }

        public ActionResult Details(string path)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ViewBag.pdf = path;

            return View();
        }

        public FileStreamResult GetPdf(string path)
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return File(stream, "application/pdf");
        }
    }
}