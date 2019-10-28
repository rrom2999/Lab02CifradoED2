using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab2Cifrados.Models
{
    public class Logic
    {
        public int CalculaM(int n, int Longitud)
        {
            var m = 0;
            if ((Longitud % n) == 0)
            {
                m = Longitud / n;
            }
            else
            {
                m = (Longitud / n) + 1;
            }
            return m;
        }

        public string CompletarTexto(string texto, int m, int n)
        {
            var area = m * n;
            while (texto.Length < area)
            {
                texto = texto + "$";
            }
            return texto;
        }

        //Para cifrar
        public void LlenarMatrizAbajo(string texto, char[,] matriz, int n, int m)
        {
            var contador = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matriz[j, i] = texto[contador];
                    contador++;
                }
            }
        }

        public string LeerEspiralHorario(int m, int n, char[,] matriz)
        {
            int i, filaAux = 0, colAux = 0;
            var textoCifrado = "";

            while (filaAux < m && colAux < n)
            {
                for (i = colAux; i < n; i++)
                {
                    textoCifrado = textoCifrado + matriz[filaAux, i];
                }
                filaAux++;


                for (i = filaAux; i < m; i++)
                {
                    textoCifrado = textoCifrado + matriz[i, n - 1];
                }
                n--;


                if (filaAux < m)
                {
                    for (i = n - 1; i >= colAux; i--)
                    {
                        textoCifrado = textoCifrado + matriz[m - 1, i];
                    }
                    m--;
                }


                if (colAux < n)
                {
                    for (i = m - 1; i >= filaAux; i--)
                    {
                        textoCifrado = textoCifrado + matriz[i, colAux];
                    }
                    colAux++;
                }
            }
            return textoCifrado;
        }

        public void LlenarMatrizAlLado(string texto, char[,] matriz, int n, int m)
        {
            var contador = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matriz[i, j] = texto[contador];
                    contador++;
                }
            }
        }

        public string LeerEspiralAntiHorario(int m, int n, char[,] matriz)
        {
            int i, filaAux = 0, colAux = 0;
            var textoCifrado = "";

            while (filaAux < m && colAux < n)
            {
                for (i = filaAux; i < m; i++)
                {
                    textoCifrado = textoCifrado + matriz[i, colAux];
                }
                colAux++;

                for (i = colAux; i < n; i++)
                {
                    textoCifrado = textoCifrado + matriz[m - 1, i];
                }
                m--;

                if (colAux < n)
                {
                    for (i = m - 1; i >= filaAux; i--)
                    {
                        textoCifrado = textoCifrado + matriz[i, n - 1];
                    }
                    n--;
                }

                if (filaAux < m)
                {
                    for (i = n - 1; i >= colAux; i--)
                    {
                        textoCifrado = textoCifrado + matriz[filaAux, i];
                    }
                    filaAux++;
                }
            }
            return textoCifrado;
        }

        //Para descifrar
        public char[,] AgregarEnEspiralHorario(int m, int n, string textoCifrado)
        {
            int i, filaAux = 0, colAux = 0, contador = 0; ;
            var matrizDescifrado = new char[m, n];

            while (filaAux < m && colAux < n)
            {
                for (i = colAux; i < n; i++)
                {
                    matrizDescifrado[filaAux, i] = textoCifrado[contador];
                    contador++;
                }
                filaAux++;


                for (i = filaAux; i < m; i++)
                {
                    matrizDescifrado[i, n - 1] = textoCifrado[contador];
                    contador++;
                }
                n--;


                if (filaAux < m)
                {
                    for (i = n - 1; i >= colAux; i--)
                    {
                        matrizDescifrado[m - 1, i] = textoCifrado[contador];
                        contador++;
                    }
                    m--;
                }


                if (colAux < n)
                {
                    for (i = m - 1; i >= filaAux; i--)
                    {
                        matrizDescifrado[i, colAux] = textoCifrado[contador];
                        contador++;
                    }
                    colAux++;
                }
            }
            return matrizDescifrado;
        }

        public string LeerMatrizAbajo(char[,] matrizCifrada, int n, int m)
        {
            var textoDescifrado = "";

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    textoDescifrado = textoDescifrado + matrizCifrada[j, i];
                }
            }
            return textoDescifrado;
        }

        public char[,] AgregarEnEspiralAntiHorario(int m, int n, string textoCifrado)
        {
            int i, filaAux = 0, colAux = 0, contador = 0; ;
            var matrizDescifrado = new char[m, n];

            while (filaAux < m && colAux < n)
            {
                for (i = filaAux; i < m; i++)
                {
                    matrizDescifrado[i, colAux] = textoCifrado[contador];
                    contador++;
                }
                colAux++;

                for (i = colAux; i < n; i++)
                {
                    matrizDescifrado[m - 1, i] = textoCifrado[contador];
                    contador++;
                }
                m--;

                if (colAux < n)
                {
                    for (i = m - 1; i >= filaAux; i--)
                    {
                        matrizDescifrado[i, n - 1] = textoCifrado[contador];
                        contador++;
                    }
                    n--;
                }

                if (filaAux < m)
                {
                    for (i = n - 1; i >= colAux; i--)
                    {
                        matrizDescifrado[filaAux, i] = textoCifrado[contador];
                        contador++;
                    }
                    filaAux++;
                }
            }
            return matrizDescifrado;
        }

        public string LeerMatrizAlLado(char[,] matrizCifrada, int n, int m)
        {
            var textoDescifrado = "";

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    textoDescifrado = textoDescifrado + matrizCifrada[i, j];
                }
            }
            return textoDescifrado;
        }
    }
}