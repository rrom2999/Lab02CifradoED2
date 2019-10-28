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
                                        TopeSup = false;
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
                                        TopeSup = false;
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

            return RedirectToAction("Index", "Home");
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
                var RutaEscritura = Server.MapPath($"~/Descifrados/{Name[0]}.txt");
                ADescifrarZZ.SaveAs(RutaLectura);
                List<string> Guia = new List<string>();

                
                var CBytes = ADescifrarZZ.ContentLength;
                var CLineas = Convert.ToInt32(Clave);
                var M = (CBytes - 3 + (2 * CLineas)) / ((2 * CLineas) - 2);
                var ArregloEscritor = new string[CLineas];
                
                //Llenado de ArregloEscritor
                using (var stream = new FileStream(RutaLectura, FileMode.Open))
                {
                    using (var Lector = new BinaryReader(stream))
                    {
                        var BytesBuffer = new byte[ADescifrarZZ.ContentLength]; //Solo un buffer
                        while (Lector.BaseStream.Position != Lector.BaseStream.Length)
                        {
                            BytesBuffer = Lector.ReadBytes(M); //llenado de primera linea
                            for (int i = 0; i < M; i++)
                            {
                                ArregloEscritor[0] = $"{ArregloEscritor[0]}{Convert.ToString(Convert.ToChar(BytesBuffer[i]))}";
                            }
                            for (int P = 1; P <= (CLineas-2); P++)
                            {
                                BytesBuffer = Lector.ReadBytes(2 * (M - 1));
                                for (int j = 0; j < BytesBuffer.Length; j++)
                                {
                                    ArregloEscritor[P] = $"{ArregloEscritor[P]}{Convert.ToString(Convert.ToChar(BytesBuffer[j]))}";
                                }
                            }
                            BytesBuffer = Lector.ReadBytes(M - 1);
                            for (int i = 0; i < BytesBuffer.Length; i++)
                            {
                                ArregloEscritor[ArregloEscritor.Length - 1] = $"{ArregloEscritor[ArregloEscritor.Length - 1]}{Convert.ToString(Convert.ToChar(BytesBuffer[i]))}";
                            }
                            if(ArregloEscritor[ArregloEscritor.Length - 1].Length < (M-1))
                            {
                                ArregloEscritor[ArregloEscritor.Length - 1].PadRight((M - 1), '~');
                            }
                        }

                    }
                }

                var IndiceExtremo = 0;
                var IndiceMedio = 0;
                using (var writeStream = new FileStream(RutaEscritura, FileMode.OpenOrCreate))
                {
                    using (var Escritor = new BinaryWriter(writeStream))
                    {
                        /*foreach(var Cadena in ArregloEscritor)
                        {
                            var CharCadena = Cadena.ToCharArray;
                        }*/
                        while(IndiceExtremo < (M-1))
                        {
                            //Lectura hacia abajo
                            for(int i = 0; i < ArregloEscritor.Length; i++)
                            {
                                if(i == 0) //Primera linea
                                {
                                    Escritor.Write(Convert.ToByte(Convert.ToInt32(Convert.ToChar(ArregloEscritor[i][IndiceExtremo]))));
                                }
                                else if(i == ArregloEscritor.Length-1) //Ultima linea
                                {
                                    Escritor.Write(Convert.ToByte(Convert.ToInt32(Convert.ToChar(ArregloEscritor[i][IndiceExtremo]))));
                                    IndiceExtremo++;
                                    IndiceMedio++;
                                }
                                else
                                {
                                    Escritor.Write(Convert.ToByte(Convert.ToInt32(Convert.ToChar(ArregloEscritor[i][IndiceMedio]))));
                                }
                            }
                            //Lectura hacia arriba
                            for (int j = (ArregloEscritor.Length-2); j >= 0 ; j--)
                            {
                                if(j > 0) //Empieza a escribir lineas medias
                                {
                                    Escritor.Write(Convert.ToByte(Convert.ToInt32(Convert.ToChar(ArregloEscritor[j][IndiceMedio]))));
                                }
                                else if(j == 0)
                                {
                                    IndiceMedio++;
                                }
                            }
                        }
                        Escritor.Write(Convert.ToByte(Convert.ToInt32(Convert.ToChar(ArregloEscritor[0][IndiceExtremo]))));
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}