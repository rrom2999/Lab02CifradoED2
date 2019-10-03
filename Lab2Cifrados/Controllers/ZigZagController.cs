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
                var RutaEscritura = Server.MapPath($"~/Cifrados/{Name[0]}.cif");
                ACifrarZZ.SaveAs(RutaLectura);

                const int TBuffer = 1024;
                var LineaVector = 0;
                var CLineas = Convert.ToInt32(Clave);
                var ArregloEscritor = new string[CLineas];
                var Sentido = 0;
                var TopeSup = true;

                using (var stream = new FileStream(RutaLectura, FileMode.Open))
                {
                    using (var Lector = new BinaryReader(stream))
                    {
                        var BytesBuffer = new byte[TBuffer];
                        while (Lector.BaseStream.Position != Lector.BaseStream.Length)
                        {
                            BytesBuffer = Lector.ReadBytes(TBuffer);
                            for (int i = 0; i < BytesBuffer.Length; i++)
                            {
                                if (Sentido == 0) //Hacia abajo
                                {
                                    if (LineaVector == CLineas - 1) // Llego hasta abajo
                                    {
                                        Sentido = 1;
                                        ArregloEscritor[LineaVector] = $"{ArregloEscritor[LineaVector]}{Convert.ToString(Convert.ToChar(BytesBuffer[i]))}";
                                        LineaVector--;
                                    }
                                    else
                                    {
                                        Sentido = 0;
                                        TopeSup = false;
                                        ArregloEscritor[LineaVector] = $"{ArregloEscritor[LineaVector]}{Convert.ToString(Convert.ToChar(BytesBuffer[i]))}";
                                        LineaVector++;
                                    }
                                }
                                else       //Hacia arriba
                                {
                                    if (LineaVector == 0) // Llego hasta arriba
                                    {
                                        Sentido = 0;
                                        TopeSup = true;
                                        ArregloEscritor[LineaVector] = $"{ArregloEscritor[LineaVector]}{Convert.ToString(Convert.ToChar(BytesBuffer[i]))}";
                                        LineaVector++;
                                    }
                                    else
                                    {
                                        Sentido = 1;
                                        ArregloEscritor[LineaVector] = $"{ArregloEscritor[LineaVector]}{Convert.ToString(Convert.ToChar(BytesBuffer[i]))}";
                                        LineaVector--;
                                    }
                                }
                            }
                        }

                        //Relleno de espacios calculando valores que deberian ser

                        if (!TopeSup)
                        {
                            if (Sentido != 0) //Se quedo escribiendo hacia arriba
                            {
                                while (LineaVector != 0)
                                {
                                    ArregloEscritor[LineaVector] = $"{ArregloEscritor[LineaVector]}~";
                                    LineaVector--;
                                }
                                ArregloEscritor[LineaVector] = $"{ArregloEscritor[LineaVector]}~";
                            }
                            else               //Se quedo escribiendo hacia abajo
                            {
                                while (LineaVector < CLineas - 1)
                                {
                                    ArregloEscritor[LineaVector] = $"{ArregloEscritor[LineaVector]}~";
                                    LineaVector++;
                                }
                                ArregloEscritor[LineaVector] = $"{ArregloEscritor[LineaVector]}~"; //Escribe el de hasta abajo
                                LineaVector--;
                                while (LineaVector != 0)
                                {
                                    ArregloEscritor[LineaVector] = $"{ArregloEscritor[LineaVector]}~";
                                    LineaVector--;
                                }
                                ArregloEscritor[LineaVector] = $"{ArregloEscritor[LineaVector]}~";
                            }
                        }

                        using (var writeStream = new FileStream(RutaEscritura, FileMode.OpenOrCreate))
                        {
                            using (var Escritor = new BinaryWriter(writeStream))
                            {
                                foreach(var Franja in ArregloEscritor)
                                {
                                    foreach(var Caracter in Franja)
                                    {
                                        Escritor.Write(Convert.ToByte(Convert.ToInt32(Convert.ToChar(Caracter))));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return View();
        }

        public ActionResult CargaDescifradoZZ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargaDescifradoZZ(HttpPostedFileBase ADescifrarZZ, string Clave)
        {
            if (ADescifrarZZ != null)
            {
                var NombreArchivo = ADescifrarZZ.FileName;
                var Niveles = Convert.ToInt32(Clave);
                var Name = NombreArchivo.Split('.');
                var RutaLectura = Server.MapPath($"~/Cargados/{NombreArchivo}");
                var RutaEscritura = Server.MapPath($"~/Descifrados/{Name[0]}.cif");
                ADescifrarZZ.SaveAs(RutaLectura);

                const int TBuffer = 1024;
                var CLineas = Convert.ToInt32(Clave);
                var ArregloEscritor = new string[CLineas];

                using (var stream = new FileStream(RutaLectura, FileMode.Open))
                {
                    using (var Lector = new BinaryReader(stream))
                    {
                    }
                }
            }
            return View();
        }
    }
}