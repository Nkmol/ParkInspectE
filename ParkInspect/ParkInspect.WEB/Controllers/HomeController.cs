using System;
using System.IO;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Text;
using System.Diagnostics;

namespace ParkInspect.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult UploadFile(int? ID, string pdfcontent)
        {
            if (Directory.Exists("rapportage"))
            {
                Directory.CreateDirectory("rapportage");
            }
            Debug.WriteLine(String.Format("{0}\\{1}{2}.pdf", Directory.GetCurrentDirectory(), ID, DateTime.Now.ToString()));

            var file =  System.IO.File.Create(String.Format("{0}\\{1}{2}f.pdf", Directory.GetCurrentDirectory(), ID , DateTime.Now.ToString().Replace(":" , " ")));
            
            byte[] bytes = Encoding.ASCII.GetBytes(pdfcontent);
            file.Write(bytes, 0 , bytes.Length);
            file.Close();

            return Content(" ");
        }

        public ActionResult Index()
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToAction("Login", "Account");

            //var path = Server.MapPath("~/App_LocalResources");
            //var x = Directory.GetFiles(path);

            //for (var i = 0; i < x.Length; i++)
            //    x[i] = x[i].Replace('\\', '/');

            //ViewBag.pdf = x;

            return View();
        }

        public ActionResult Details(string path)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToAction("Login", "Account");

            //ViewBag.pdf = path;

            return View();
        }

        //public FileStreamResult GetPdf(string path)
        //{
        //    var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        //    return File(stream, "application/pdf");
        //}
    }
}