using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Lab2Cifrados.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var ubicacionC = Server.MapPath($"~/Cifrados");
            var ubicacionD = Server.MapPath($"~/Descifrados");
            var directorioC = new DirectoryInfo(ubicacionC);
            var directorioD = new DirectoryInfo(ubicacionD);
            var filesC = directorioC.GetFiles("*.*");
            var filesD = directorioD.GetFiles("*.*");
            var listDescarga = new List<string>();
            foreach (var item in filesC)
            {
                listDescarga.Add(item.Name);
            }
            foreach (var item in filesD)
            {
                listDescarga.Add(item.Name);
            }
            return View(listDescarga);
        }

        public ActionResult Descargables(string fileName)

        {
            var name = fileName.Split('.');
            var DireccionCompleta = string.Empty;
            if (name[1] == "cif")
            {
                DireccionCompleta = Path.Combine(Server.MapPath($"~//Cifrados"), fileName);
                return File(DireccionCompleta, "Cifrados", $"{name[0]}.{name[1]}");
            }
            else
            {
                DireccionCompleta = Path.Combine(Server.MapPath($"~//Descifrados"), fileName);
                return File(DireccionCompleta, "Descifrados", $"{name[0]}.{name[1]}");
            }
            
        }

        public ActionResult LlamadoHome()
        {
            return RedirectToAction("Index", "Home");
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

        public ActionResult LlamadoSDES()
        {
            return RedirectToAction("Index", "SDES");
        }
    }
}