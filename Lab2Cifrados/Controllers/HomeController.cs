using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab2Cifrados.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LlamadoZigZag()
        {
            return RedirectToAction("Index", "ZigZag");
        }

        public ActionResult LlamadoEspiral()
        {
            return RedirectToAction("Index", "Espiral");
        }

        public ActionResult LlamadoCesar()
        {
            return RedirectToAction("Index", "Cesar");
        }
    }
}