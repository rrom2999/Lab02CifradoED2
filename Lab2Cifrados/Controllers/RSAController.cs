﻿using System;
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

                    ViewBag.Msg = "Se han generado correctamente las llaves";
                }
                else
                {
                    ViewBag.Msg = "Alguno de los números no es primo";
                }
            }
            else
            {
                ViewBag.Msg = "Falta ingresar algún número primo";
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
        public ActionResult CargaParaDescifrar(HttpPostedFileBase ADescifrar, string Clave)
        {
            if (ADescifrar != null)
            {
                if (Clave != null)
                {
                    var Llave = Convert.ToInt32(Clave);
                    if (Llave < 1024)
                    {
                        SDES NSDES = new SDES();
                        NSDES.S0[0, 0] = "01"; NSDES.S0[0, 1] = "00"; NSDES.S0[0, 2] = "11"; NSDES.S0[0, 3] = "10";
                        NSDES.S0[1, 0] = "11"; NSDES.S0[1, 1] = "10"; NSDES.S0[1, 2] = "01"; NSDES.S0[1, 3] = "00";
                        NSDES.S0[2, 0] = "00"; NSDES.S0[2, 1] = "10"; NSDES.S0[2, 2] = "01"; NSDES.S0[2, 3] = "11";
                        NSDES.S0[3, 0] = "11"; NSDES.S0[3, 1] = "01"; NSDES.S0[3, 2] = "11"; NSDES.S0[3, 3] = "10";

                        NSDES.S1[0, 0] = "00"; NSDES.S1[0, 1] = "01"; NSDES.S1[0, 2] = "10"; NSDES.S1[0, 3] = "11";
                        NSDES.S1[1, 0] = "10"; NSDES.S1[1, 1] = "00"; NSDES.S1[1, 2] = "01"; NSDES.S1[1, 3] = "11";
                        NSDES.S1[2, 0] = "11"; NSDES.S1[2, 1] = "00"; NSDES.S1[2, 2] = "01"; NSDES.S1[2, 3] = "00";
                        NSDES.S1[3, 0] = "10"; NSDES.S1[3, 1] = "01"; NSDES.S1[3, 2] = "00"; NSDES.S1[3, 3] = "11";

                        Clave = Convert.ToString(Llave, 2);
                        while (Clave.Length < 10)
                        {
                            Clave = $"0{Clave}";
                        }

                        var NombreArchivo = ADescifrar.FileName;
                        var Name = NombreArchivo.Split('.');
                        var RutaLectura = Server.MapPath($"~/Cargados/{NombreArchivo}");
                        var RutaEscritura = Server.MapPath($"~/Descifrados/{Name[0]}.txt");
                        var RutaContantes = Server.MapPath($"~/Constantes.txt");
                        var Permutaciones = NSDES.ObtenerPermutaciones(RutaContantes);
                        NSDES.ObtenerKas(Clave, Permutaciones);
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
                                        while (Lector.BaseStream.Position != Lector.BaseStream.Length)
                                        {
                                            BytesBuffer = Lector.ReadBytes(TBuffer);
                                            foreach (var Caracter in BytesBuffer)
                                            {
                                                var ByteLeido = Convert.ToString(Caracter, 2); //Enviar ByteLeido a metodo para cifrar
                                                while (ByteLeido.Length < 8)
                                                {
                                                    ByteLeido = $"0{ByteLeido}";
                                                }
                                                var ByteCifrado = NSDES.DescifrarByte(ByteLeido, Permutaciones);
                                                Escritor.Write(Convert.ToByte(ByteCifrado));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        ViewBag.Msg = "El archivo ha sido descifrado con exito";
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
    }
}