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
                if((Convert.ToInt32(primoP) >= 17 && Convert.ToInt32(primoQ) >= 19) || (Convert.ToInt32(primoP) >= 19 && Convert.ToInt32(primoQ) >= 17))
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
                            using (var Escritor = new BinaryWriter(writeStream))
                            {
                                e = NRSA.CalcularE(n, phiN);
                                Escritor.Write($"{e},{n}");
                            }
                        }

                        using (var writeStream = new FileStream(RutaKPrivada, FileMode.OpenOrCreate))
                        {
                            using (var Escritor = new BinaryWriter(writeStream))
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
                    TempData["shortMessage"] = "p y q deben ser al menos 17 y 19";
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
                    //Lectura de llave
                    var NombreLlave = KeyCifrar.FileName;
                    var NameKey = NombreLlave.Split('.');
                    var RutaLecturaLlave = Server.MapPath($"~/Cargados/{NombreLlave}");
                    KeyCifrar.SaveAs(RutaLecturaLlave);
                    var key = new string[2];
                    var cadena = string.Empty;
                    using (var stream = new FileStream(RutaLecturaLlave, FileMode.Open))
                    {
                        using (var Lector = new BinaryReader(stream))
                        {
                            while (Lector.BaseStream.Position != Lector.BaseStream.Length)
                            {
                                cadena = Lector.ReadString();
                            }
                        }
                    }
                    key = cadena.Split(',');
                    
                    var NombreArchivo = ACifrar.FileName;
                    var Name = NombreArchivo.Split('.');
                    var RutaLectura = Server.MapPath($"~/Cargados/{NombreArchivo}");
                    var RutaEscritura = Server.MapPath($"~/Cifrados/{Name[0]}.rsacif");
                    ACifrar.SaveAs(RutaLectura);

                    const int TBuffer = 1024;

                    using (var stream = new FileStream(RutaLectura, FileMode.Open))
                    {
                        using (var Lector = new BinaryReader(stream))
                        {
                            using (var writeStream = new FileStream(RutaEscritura, FileMode.OpenOrCreate))
                            {
                                using (var Escritor = new BinaryWriter(writeStream))
                                {
                                    var BytesBuffer = new byte[TBuffer];
                                    var bytesCompletos = 0;
                                    var restantes = 0;
                                    while (Lector.BaseStream.Position != Lector.BaseStream.Length)
                                    {
                                        BytesBuffer = Lector.ReadBytes(TBuffer);
                                        foreach (var Caracter in BytesBuffer)
                                        {
                                            var Leido = Convert.ToInt32(Caracter);
                                            var cifrado = NRSA.CifYDescifNumero(Leido, Convert.ToInt32(key[0]), Convert.ToInt32(key[1]));
                                            var x = 0;
                                            if (cifrado < 256)
                                            {
                                                restantes = cifrado;
                                            }
                                            else
                                            {
                                                restantes = cifrado;
                                                while (restantes >= 256)
                                                {
                                                    restantes -= 255;
                                                    bytesCompletos++;
                                                }
                                            }
                                            Escritor.Write(Convert.ToByte(Convert.ToInt32(bytesCompletos)));
                                            Escritor.Write(Convert.ToByte(Convert.ToInt32(restantes)));
                                            bytesCompletos = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }

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
                    var NRSA = new RSA();
                    //Lectura de llave
                    var NombreLlave = KeyDescifrar.FileName;
                    var NameKey = NombreLlave.Split('.');
                    var RutaLecturaLlave = Server.MapPath($"~/Cargados/{NombreLlave}");
                    KeyDescifrar.SaveAs(RutaLecturaLlave);
                    var key = new string[2];
                    var cadena = string.Empty;
                    using (var stream = new FileStream(RutaLecturaLlave, FileMode.Open))
                    {
                        using (var Lector = new BinaryReader(stream))
                        {
                            while (Lector.BaseStream.Position != Lector.BaseStream.Length)
                            {
                                cadena = Lector.ReadString();
                            }
                        }
                    }
                    key = cadena.Split(',');

                    var NombreArchivo = ADescifrar.FileName;
                    var Name = NombreArchivo.Split('.');
                    var RutaLectura = Server.MapPath($"~/Cargados/{NombreArchivo}");
                    var RutaEscritura = Server.MapPath($"~/Descifrados/{Name[0]}.txt");
                    ADescifrar.SaveAs(RutaLectura);

                    const int TBuffer = 1024;
                    using (var stream = new FileStream(RutaLectura, FileMode.Open))
                    {
                        using (var Lector = new BinaryReader(stream))
                        {
                            using (var writeStream = new FileStream(RutaEscritura, FileMode.OpenOrCreate))
                            {
                                using (var Escritor = new BinaryWriter(writeStream))
                                {
                                    var BytesBuffer = new byte[TBuffer];
                                    var numBase = 0;
                                    while (Lector.BaseStream.Position != Lector.BaseStream.Length)
                                    {
                                        BytesBuffer = Lector.ReadBytes(TBuffer);
                                        for (int i = 0; i < BytesBuffer.Length; i += 2)
                                        {
                                            numBase = Convert.ToInt32(BytesBuffer[i]) * 255 + Convert.ToInt32(BytesBuffer[i + 1]);
                                            var cifrado = NRSA.CifYDescifNumero(numBase, Convert.ToInt32(key[0]), Convert.ToInt32(key[1]));
                                            Escritor.Write(Convert.ToByte(cifrado));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return View();
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
