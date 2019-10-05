using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab2Cifrados.Models;
using System.IO;

namespace Lab2Cifrados.Controllers
{
    public class EspiralController : Controller
    {
        Logic objeto = new Logic();

        // GET: Espiral
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CargadoEspiral()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CargadoEspiral(HttpPostedFileBase ACifrarEs, string Clave, string Direccion)
        {
            if (ACifrarEs != null)
            {
                var NombreArchivo = ACifrarEs.FileName;
                var Niveles = Convert.ToInt32(Clave);
                var Name = NombreArchivo.Split('.');
                var RutaLectura = Server.MapPath($"~/Cargados/{NombreArchivo}");
                var RutaEscritura = Server.MapPath($"~/Cifrados/{Name[0]}.cif");
                ACifrarEs.SaveAs(RutaLectura);

                const int TBuffer = 1024;
                var textoCifrado = "";
                var texto = "";
                var m = 0;
                var n = 0;

                using (var stream = new FileStream(RutaLectura, FileMode.Open))
                {
                    using (var Lector = new StreamReader(stream))
                    {
                        var BytesBuffer = new byte[TBuffer];
                        while (!Lector.EndOfStream)
                        {
                            texto = Lector.ReadToEnd();
                        }

                        n = Convert.ToInt32(Clave);

                        m = objeto.CalculaM(n, texto.Length);
                        texto = objeto.CompletarTexto(texto, m, n);
                        var matrizCifrado = new char[m, n];

                        if (Convert.ToInt32(Direccion) == 1)
                        {
                            objeto.LlenarMatrizAbajo(texto, matrizCifrado, n, m);
                            textoCifrado = objeto.LeerEspiralHorario(m, n, matrizCifrado);
                        }
                        else if (Convert.ToInt32(Direccion) == 2)
                        {
                            objeto.LlenarMatrizAlLado(texto, matrizCifrado, n, m);
                            textoCifrado = objeto.LeerEspiralAntiHorario(m, n, matrizCifrado);
                        }

                        using (var writeStream = new FileStream(RutaEscritura, FileMode.OpenOrCreate))
                        {
                            using (var Escritor = new BinaryWriter(writeStream))
                            {
                                foreach (var item in textoCifrado)
                                {
                                    Escritor.Write(Convert.ToByte(item));
                                }
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CargaDescifradoEspiral()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargaDescifradoEspiral(HttpPostedFileBase ADescifrarEs, string Clave, string Direccion)
        {
            if (ADescifrarEs != null)
            {
                var NombreArchivo = ADescifrarEs.FileName;
                var Niveles = Convert.ToInt32(Clave);
                var Name = NombreArchivo.Split('.');
                var RutaLectura = Server.MapPath($"~/Cargados/{NombreArchivo}");
                var RutaEscritura = Server.MapPath($"~/Descifrados/{Name[0]}.txt");
                ADescifrarEs.SaveAs(RutaLectura);

                const int TBuffer = 1024;
                var textoCifrado = "";
                var texto = "";
                var m = 0;
                var n = 0;

                using (var stream = new FileStream(RutaLectura, FileMode.Open))
                {
                    using (var Lector = new StreamReader(stream))
                    {
                        var BytesBuffer = new byte[TBuffer];
                        while (!Lector.EndOfStream)
                        {
                            textoCifrado = Lector.ReadToEnd();
                        }

                        n = Convert.ToInt32(Clave);

                        m = objeto.CalculaM(n, textoCifrado.Length);
                        textoCifrado = objeto.CompletarTexto(textoCifrado, m, n);
                        var matrizDescifrado = new char[m, n];

                        if (Convert.ToInt32(Direccion) == 1)
                        {
                            matrizDescifrado = objeto.AgregarEnEspiralAntiHorario(n, m, textoCifrado);
                            texto = objeto.LeerMatrizAlLado(matrizDescifrado, m, n);
                        }
                        else if (Convert.ToInt32(Direccion) == 2)
                        {
                            matrizDescifrado = objeto.AgregarEnEspiralHorario(n, m, textoCifrado);
                            texto = objeto.LeerMatrizAbajo(matrizDescifrado, m, n);
                        }

                        using (var writeStream = new FileStream(RutaEscritura, FileMode.OpenOrCreate))
                        {
                            using (var Escritor = new BinaryWriter(writeStream))
                            {
                                foreach (var item in texto)
                                {
                                    Escritor.Write(Convert.ToByte(Convert.ToChar(item)));
                                }
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Espiral/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Espiral/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Espiral/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Espiral/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Espiral/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Espiral/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Espiral/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
