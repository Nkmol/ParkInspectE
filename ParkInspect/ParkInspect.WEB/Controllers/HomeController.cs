using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkInspect.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            string path = Server.MapPath("~/App_LocalResources");
          var x =  Directory.GetFiles(path);

            for (int i = 0; i < x.Length; i++)
            {
                x[i] = x[i].Replace('\\', '/');
            }

            ViewBag.pdf = x;

            return View();
        }

        public FileStreamResult GetPdf(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return File(stream, "application/pdf");
        }

    }
}