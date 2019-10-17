using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Lab2Cifrados.Models;

namespace Lab2Cifrados.Controllers
{
    public class SDESController : Controller
    {
        // GET: SDES
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargaParaCifrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargaParaCifrar(HttpPostedFileBase ACifrar, string Clave)
        {
            if (ACifrar != null)
            {
                if (Clave != null)
                {
                    var Llave = Convert.ToInt32(Clave);
                    if (Llave < 1024)
                    {
                        SDES NSDES = new SDES();
                        Clave = Convert.ToString(Llave, 2);
                        NSDES.ObtenerKas(Clave);
                    }
                    else
                    {
                        ViewBag.Msg = "La clave debe ser menor a 1024";
                    }
                }
                else
                {
                    ViewBag.Msg = "No se ingreso ninguna clave";
                }
            }
            else
            {
                ViewBag.Msg = "No se selecciono ningun archivo";
            }
            return View();
        }

        public ActionResult CargaParaDescifrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargaParaDescifrar(HttpPostedFileBase ADescifrar, string Clave)
        {
            return View();
        }
    }
}