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
                        
                        var NombreArchivo = ACifrar.FileName;
                        var Name = NombreArchivo.Split('.');
                        var RutaLectura = Server.MapPath($"~/Cargados/{NombreArchivo}");
                        var RutaEscritura = Server.MapPath($"~/Cifrados/{Name[0]}.cif");
                        var RutaContantes = Server.MapPath($"~/Constantes.txt");
                        var Permutaciones = NSDES.ObtenerPermutaciones(RutaContantes);
                        NSDES.ObtenerKas(Clave, Permutaciones);
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
                                        while (Lector.BaseStream.Position != Lector.BaseStream.Length)
                                        {
                                            BytesBuffer = Lector.ReadBytes(TBuffer);
                                            foreach (var Caracter in BytesBuffer)
                                            {
                                                var ByteLeido = Convert.ToString(Caracter, 2); //Enviar ByteLeido a metodo para cifrar
                                                while(ByteLeido.Length < 8)
                                                {
                                                    ByteLeido = $"0{ByteLeido}";
                                                }
                                                var ByteCifrado = NSDES.CifrarByte(ByteLeido, Permutaciones);
                                                Escritor.Write(Convert.ToByte(ByteCifrado));
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