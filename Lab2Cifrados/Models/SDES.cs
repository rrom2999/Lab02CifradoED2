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

        public void ObtenerKas(string Llave)
        {
            var ParteA = "";
            var ParteB = "";

            var Binario = P10(Llave);
            for (int a = 0; a < 5; a++)
            {
                ParteA = $"{ParteA}{Binario[a]}";
            }

            for (int b = 5; b < 10; b++)
            {
                ParteB = $"{ParteB}{Binario[b]}";
            }
            var ShifteadoUno = "";
            var Temporal = "";
            for (int a = 1; a < 5; a++)
            {
                Temporal = $"{Temporal}{ParteA[a]}";
            }
            ShifteadoUno = $"{Temporal}{ParteA[0]}"; //Primeros 5 con LS1
            Temporal = "";
            for (int b = 1; b < 5; b++)
            {
                Temporal = $"{Temporal}{ParteB[b]}";
            }
            ShifteadoUno = $"{ShifteadoUno}{Temporal}{ParteB[0]}";
            K1 = P8(ShifteadoUno);

            //Volver a hacer para K2 sobre ShifteadoUno
            ParteA = "";
            ParteB = "";

            for (int a = 0; a < 5; a++)
            {
                ParteA = $"{ParteA}{ShifteadoUno[a]}";
            }

            for (int b = 5; b < 10; b++)
            {
                ParteB = $"{ParteB}{ShifteadoUno[b]}";
            }

            var ShifteadoDos = "";
            Temporal = "";
            for (int a = 2; a < 5; a++)
            {
                Temporal = $"{Temporal}{ParteA[a]}";
            }
            ShifteadoDos = $"{Temporal}{ParteA[0]}{ParteA[1]}"; //Primeros 5 con LS2 
            Temporal = "";
            for (int b = 2; b < 5; b++)
            {
                Temporal = $"{Temporal}{ParteB[b]}";
            }
            ShifteadoDos = $"{ShifteadoDos}{Temporal}{ParteB[0]}{ParteB[1]}";
            K2 = P8(ShifteadoDos);
        }
    }
}