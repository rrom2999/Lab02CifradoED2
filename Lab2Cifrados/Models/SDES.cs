﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab2Cifrados.Models
{
    public class SDES
    {
        public string K1;
        public string K2;
        public string[,] S0 = new string[4, 4];
        public string[,] S1 = new string[4, 4];

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

        public string IP(string Binario, bool Inverso)
        {
            var Secuencia = "70615243";
            var Nuevo = "";
            if (!Inverso)
            {
                for (int i = 0; i < 8; i++)
                {
                    var Posicion = Convert.ToInt32(Convert.ToString(Secuencia[i]));
                    Nuevo = $"{Nuevo}{Binario[Posicion]}";
                }
            }
            else
            {
                var SecuenciaInversa = "";
                string[] VectorInverso = new string[8];
                for(int i = 0; i < 8; i++)
                {
                    VectorInverso[Convert.ToInt32(Convert.ToString(Secuencia[i]))] = Convert.ToString(i);
                }
                
                foreach(var Valor in VectorInverso)
                {
                    SecuenciaInversa = $"{SecuenciaInversa}{Valor}";
                }

                for (int i = 0; i < 8; i++)
                {
                    var Posicion = Convert.ToInt32(Convert.ToString(SecuenciaInversa[i]));
                    Nuevo = $"{Nuevo}{Binario[Posicion]}";
                }
            }

            return Nuevo;
        }

        public string XOR(string Uno, string Dos)
        {
            var ResultadoXOR = "";
            for (int i = 0; i < Uno.Length; i++)
            {
                if (Uno[i] != Dos[i]) ResultadoXOR += "1";
                else ResultadoXOR += "0";
            }

            return ResultadoXOR;
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

        public string ObtenerDeSBoxes(string Binario)
        {
            var AuxFila = $"{Binario[3]}{Binario[0]}"; var AuxCol = $"{Binario[2]}{Binario[1]}"; //Al reves para facilitar la conversion a int
            var FilaS0 = 0; var ColS0 = 0;
            for (int i = 0; i < 2; i++)
            {
                if (AuxFila[i] == '1')
                    FilaS0 += (int)Math.Pow(2, i);

                if (AuxCol[i] == '1')
                    ColS0 += (int)Math.Pow(2, i);
            }

            AuxFila = $"{Binario[7]}{Binario[4]}"; AuxCol = $"{Binario[6]}{Binario[5]}";  //Al reves para facilitar conversion a int
            var FilaS1 = 0; var ColS1 = 0;
            for (int i = 0; i < 2; i++)
            {
                if (AuxFila[i] == '1')
                    FilaS1 += (int)Math.Pow(2, i);

                if (AuxCol[i] == '1')
                    ColS1 += (int)Math.Pow(2, i);
            }

            var ResultadoSBoxes = $"{S0[FilaS0, ColS0]}{S1[FilaS1, ColS1]}";
            return ResultadoSBoxes;
        }

        public string CifrarByte(string ByteLeido)
        {
            var Binario = IP(ByteLeido, false); //Paso1

            var ParteA = Binario.Substring(0, 4);
            var ParteB = Binario.Substring(4, 4);

            var NuevaParteB = EP(ParteB); //Paso2
            NuevaParteB = XOR(NuevaParteB, K1); //Paso3
            
            var ResultadoSBoxes1 = ObtenerDeSBoxes(NuevaParteB); //Paso4
            ResultadoSBoxes1 = P4(ResultadoSBoxes1); //Paso5
            var ParteC = XOR(ResultadoSBoxes1, ParteA); //Paso6

            var NuevoBinario = $"{ParteB}{ParteC}";//Paso7 y Paso8

            var ParteD = NuevoBinario.Substring(0, 4);
            var ParteE = NuevoBinario.Substring(4, 4);

            var NuevaParteE = EP(ParteE); //Paso9
            NuevaParteE = XOR(NuevaParteE, K2); //Paso10

            var ResultadoSBoxes2 = ObtenerDeSBoxes(NuevaParteE); //Paso11
            ResultadoSBoxes2 = P4(ResultadoSBoxes2); //Paso12
            var ParteF = XOR(ResultadoSBoxes2, ParteD); //Paso13

            var Nuevo = $"{ParteF}{ParteE}"; //Paso14
            Nuevo = IP(Nuevo, true); //Paso15
            return Nuevo;
        }
    }
}