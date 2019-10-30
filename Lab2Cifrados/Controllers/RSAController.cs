using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Lab2Cifrados.Models;

namespace Lab2Cifrados.Controllers
{
    public class RSAController : Controller
    {
        // GET: RSA
        public ActionResult Index()
        {
            if (TempData["shortMessage"] != null)
            {
                ViewBag.Msg = TempData["shortMessage"].ToString();
            }

            var ubicacionesKeys = Server.MapPath($"~/Claves");
            var directorioKeys = new DirectoryInfo(ubicacionesKeys);
            var filesKeys = directorioKeys.GetFiles("*.*");
            var listDescarga = new List<string>();
            foreach (var item in filesKeys)
            {
                listDescarga.Add(item.Name);
            }
            return View(listDescarga);
        }
        
        public ActionResult KeysDescargables(string fileName)
        {
            var name = fileName.Split('.');
            var direccionCompleta = string.Empty;
            direccionCompleta = Path.Combine(Server.MapPath($"~//Claves"), fileName);

            return File(direccionCompleta, "Claves", $"{name[0]}.{name[1]}");
        }

        public ActionResult GenerarLlaves()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerarLlaves(string primoP, string primoQ)
        {
            if ((primoP != null) && (primoQ != null))
            {
                var NRSA = new RSA();
                var p = Convert.ToInt32(primoP);
                var q = Convert.ToInt32(primoQ);
                var n = new int();
                var phiN = new int();
                var e = new int();
                var d = new int();
                
                if (NRSA.EsPrimo(p) && NRSA.EsPrimo(q))
                {
                    n = p * q;
                    phiN = (p - 1) * (q - 1);

                    var RutaKPrivada = Server.MapPath($"~/Claves/private.key");
                    var RutaKPublica = Server.MapPath($"~/Claves/public.key");

                    using (var writeStream = new FileStream(RutaKPublica, FileMode.OpenOrCreate))
                    {
                        using (var Escritor = new StreamWriter(writeStream))
                        {
                            e = NRSA.CalcularE(n, phiN);
                            Escritor.Write($"{e},{n}");
                        }
                    }

                    using (var writeStream = new FileStream(RutaKPrivada, FileMode.OpenOrCreate))
                    {
                        using (var Escritor = new StreamWriter(writeStream))
                        {
                            d = NRSA.CalcularD(e, phiN);
                            Escritor.Write($"{d},{n}");
                        }
                    }

                    TempData["shortMessage"] = "Se han generado correctamente las llaves";
                }
                else
                {
                    TempData["shortMessage"] = "Alguno de los números no es primo";
                }
            }
            else
            {
                TempData["shortMessage"] = "Falta ingresar algún número primo";
            }
            return RedirectToAction("Index");
        }

        public ActionResult CargaParaCifrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargaParaCifrar(HttpPostedFileBase ACifrar, HttpPostedFileBase KeyCifrar)
        {
            if (ACifrar != null)
            {
                if (KeyCifrar != null)
                {
                    var NRSA = new RSA();
                    var NombreArchivo = ACifrar.FileName;
                    var NombreLlave = KeyCifrar.FileName;
                    var Name = NombreArchivo.Split('.');
                    var NameKey = NombreLlave.Split('.');
                    var RutaLectura = Server.MapPath($"~/Claves/{NombreArchivo}");
                    var RutaEscritura = Server.MapPath($"~/Cifrados/{Name[0]}.cif");
                    var RutaPublicK = Server.MapPath($"~/Claves/public.key");
                    var RutaPrivateK = Server.MapPath($"~/Claves/private.key");
                    var publicK = NRSA.ObtenerKeys(RutaPublicK);
                    var privateK = NRSA.ObtenerKeys(RutaPrivateK);


                    ViewBag.Msg = "El archivo ha sido cifrado con exito";
                }
                else
                {
                    ViewBag.Msg = "No se selecciono ninguna llave";
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
        public ActionResult CargaParaDescifrar(HttpPostedFileBase ADescifrar, HttpPostedFileBase KeyDescifrar)
        {
            if (ADescifrar != null)
            {
                if (KeyDescifrar != null)
                {

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
    }
}
