using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab2Cifrados.Models
{
    public class SDES
    {
        public string K1;
        public string K2;

        public string P10(string Binario)  //Cambiar a que lo lea en txt
        {
            string Secuencia = "9743521806";
            string Nuevo = "";
            if (Binario.Length < 10) Binario.PadLeft(10, '0');

            for (int i = 0; i < 10; i++)
            {
                int Posicion = Convert.ToInt32(Convert.ToString(Secuencia[i]));
                Nuevo = $"{Nuevo}{Binario[Posicion]}";
            }
            return Nuevo;
        }

        public string P8(string Binario)  //Cambiar a que lea en txt
        {
            var Secuencia = "37541028";
            var Nuevo = "";

            for (int i = 0; i < 8; i++)
            {
                int Posicion = Convert.ToInt32(Convert.ToString(Secuencia[i]));
                Nuevo = $"{Nuevo}{Binario[Posicion]}";
            }

            return Nuevo;
        }

        public string P4(string Binario)  //Cambiar a que lea en txt
        {
            var Secuencia = "2310";
            var Nuevo = "";

            for (int i = 0; i < 4; i++)
            {
                var Posicion = Convert.ToInt32(Convert.ToString(Secuencia[i]));
                Nuevo = $"{Nuevo}{Binario[Posicion]}";
            }

            return Nuevo;
        }

        public string EP(string Binario)  //Cambiar a que lea en txt
        {
            var Secuencia = "20133021";
            var Nuevo = "";

            for (int i = 0; i < 8; i++)
            {
                var Posicion = Convert.ToInt32(Convert.ToString(Secuencia[i]));
                Nuevo = $"{Nuevo}{Binario[Posicion]}";
            }

            return Nuevo;
        }
    }
}