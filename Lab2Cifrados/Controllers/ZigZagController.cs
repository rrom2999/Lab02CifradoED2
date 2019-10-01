using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Lab2Cifrados.Controllers
{
    public class ZigZagController : Controller
    {
        // GET: ZigZag
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargaCifradoZZ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargaCifradoZZ(HttpPostedFileBase ACifrarZZ, string Clave)
        {
            if (ACifrarZZ != null)
            {
                var NombreArchivo = ACifrarZZ.FileName;
                var Niveles = Convert.ToInt32(Clave);
                var Name = NombreArchivo.Split('.');
                var RutaLectura = Server.MapPath($"~/Cargados/{NombreArchivo}");
                var RutaEscritura = Server.MapPath($"~/CifradoZZ/{Name[0]}.cif");
                ACifrarZZ.SaveAs(RutaLectura);

                const int TBuffer = 1024;
                var LineaVector = 0;
                var CLineas = Convert.ToInt32(Clave);
                var ArregloEscritor = new string[CLineas];

                using (var stream = new FileStream(RutaLectura, FileMode.Open))
                {
                    using (var Lector = new BinaryReader(stream))
                    {
                        using (var writeStream = new FileStream(RutaEscritura, FileMode.OpenOrCreate))
                        {
                            using (var Escritor = new BinaryWriter(writeStream))
                            {

                            }
                        }
                    }
                }
            }

            return View();
        }

    }
}